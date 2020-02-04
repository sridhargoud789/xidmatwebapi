using EEG_ReelCinemasRESTAPI.Models;
using EEG_ReelCinemasRESTAPI.Smartbutton.Transaction;
using Newtonsoft.Json;
using ReelDAO;
using System;
using System.Data;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace EEG_ReelCinemasRESTAPI.Common.Smartbutton
{
    public class TransactionBl
    {
        public IssueTransactionNoBasketReturn GetIssueTransactionNoBasket(EarnUByEmaarpoints oEarnUByEmaarpoints, DataTable dtBookingDetails)
        {
            Analizer analizer = new Analizer();
            Int64 logId = 0;
            Transaction oTransaction = new Transaction();
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();
            IssueTransactionNoBasketReturn oIssueTransactionNoBasketReturn = new IssueTransactionNoBasketReturn();
            oTransaction.Url = ServiceSettings.Rows[0]["SmartbuttonTransactionUrl"].ToString();
            oTransaction.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                oTransaction.Proxy = myProxy;
            }

            try
            {
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    logId = analizer.InsertServiceLog(0, "request"
                        , ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() +
                            "," + oEarnUByEmaarpoints.MemberId + "," + "0" + "," + ServiceSettings.Rows[0]["LocationExternalReference"].ToString()
                            + "," + ServiceSettings.Rows[0]["TransactionTypeExternalReference"].ToString() + "," + DateTime.Now.ToString() + "," +
                            oEarnUByEmaarpoints.MemberId + "," + dtBookingDetails.Rows[0]["TotalPrice"].ToString() + "," + "0" + "," + "0" + "," + "0" + "," + "1" + "," + "0"
                        , oEarnUByEmaarpoints.DeviceType
                        , dtBookingDetails.Rows[0]["UserSessionId"].ToString(), dtBookingDetails.Rows[0]["SessionId"].ToString()
                        , dtBookingDetails.Rows[0]["CinemaId"].ToString()
                        , System.Reflection.MethodBase.GetCurrentMethod().Name
                        , "IssueTransactionNoBasket", String.Empty);
                }

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                oIssueTransactionNoBasketReturn = oTransaction.IssueTransactionNoBasket(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString()
              , oEarnUByEmaarpoints.MemberId, 0, ServiceSettings.Rows[0]["LocationExternalReference"].ToString()
              , ServiceSettings.Rows[0]["TransactionTypeExternalReference"].ToString(), DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"),
              oEarnUByEmaarpoints.MemberId, double.Parse(dtBookingDetails.Rows[0]["TotalPrice"].ToString()), 0, 0, 0, 1, 0);
            }
            catch (Exception ex)
            {
                oIssueTransactionNoBasketReturn.ReturnCode = -1;
            }


            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oIssueTransactionNoBasketReturn), oIssueTransactionNoBasketReturn.ReturnCode.ToString());
            }

            return oIssueTransactionNoBasketReturn;
        }

        public IssueTransactionNoBasketReturn GetIssueTransactionNoBasket(EarnUByEmaarpoints oEarnUByEmaarpoints)
        {
            Analizer analizer = null;
            Int64 logId = 0;
            Transaction oTransaction = new Transaction();
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();
            IssueTransactionNoBasketReturn oIssueTransactionNoBasketReturn = new IssueTransactionNoBasketReturn();
            oTransaction.Url = ServiceSettings.Rows[0]["SmartbuttonTransactionUrl"].ToString();
            oTransaction.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());

            MobileBookingDao mobileDao = new MobileBookingDao();
     //       string transactionId = oEarnUByEmaarpoints.MobileBookingId.Split('_')[1];
            DataTable dtBookingDetails = null;//mobileDao.GetMobileBookingDetailsByTransactionId(int.Parse(transactionId));

            try
            {
                analizer = new Analizer();
                if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
                {
                    WebProxy myProxy = null;
                    myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                    if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                    {
                        myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                        ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                    }
                    oTransaction.Proxy = myProxy;
                }

                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    logId = analizer.InsertServiceLog(0, "request"
                        , ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() +
                            "," + oEarnUByEmaarpoints.MemberId + "," + "0" + "," + ServiceSettings.Rows[0]["LocationExternalReference"].ToString()
                            + "," + ServiceSettings.Rows[0]["TransactionTypeExternalReference"].ToString() + "," + DateTime.Now.ToString() + "," +
                            oEarnUByEmaarpoints.MemberId + "," + dtBookingDetails.Rows[0]["TotalPrice"].ToString() + "," + "0" + "," + "0" + "," + "0" + "," + "1" + "," + "0"
                        , oEarnUByEmaarpoints.DeviceType
                        , dtBookingDetails.Rows[0]["UserSessionId"].ToString(), dtBookingDetails.Rows[0]["SessionId"].ToString()
                        , dtBookingDetails.Rows[0]["CinemaId"].ToString()
                        , System.Reflection.MethodBase.GetCurrentMethod().Name
                        , "IssueTransactionNoBasket", String.Empty);
                }

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                oIssueTransactionNoBasketReturn = oTransaction.IssueTransactionNoBasket(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString()
                     , oEarnUByEmaarpoints.MemberId, 0, ServiceSettings.Rows[0]["LocationExternalReference"].ToString()
                     , ServiceSettings.Rows[0]["TransactionTypeExternalReference"].ToString(), DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"),
                     oEarnUByEmaarpoints.MemberId, double.Parse(dtBookingDetails.Rows[0]["TotalPrice"].ToString()), 0, 0, 0, 1, 0);

                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oIssueTransactionNoBasketReturn), oIssueTransactionNoBasketReturn.ReturnCode.ToString());
                }
            }
            catch (Exception ex)
            {
                oIssueTransactionNoBasketReturn.ReturnCode = -1;
            }

            return oIssueTransactionNoBasketReturn;
        }
    }
}