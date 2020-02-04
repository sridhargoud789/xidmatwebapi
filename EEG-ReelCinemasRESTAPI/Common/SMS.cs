using EEG_ReelCinemasRESTAPI.Models;
using ReelDAO;
using ReelDvo;
using System;
using System.Data;
using System.IO;
using System.Net;

namespace EEG_ReelCinemasRESTAPI.Common
{
    public class Sms
    {
        public string SMSServiceRequest(EnrollUByEmaarMember oEnrollUByEmaarMember,string password)
        {
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            long logId = 0;
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();

            string enrollmentMessage = ServiceSettings.Rows[0]["UbeEnrollEarnSMSText"].ToString()
                                 .Replace("#MEMBERID#", oEnrollUByEmaarMember.MobileNo).Replace("#PASSOWRD#", password);

            string smsServiceRequest = ServiceSettings.Rows[0]["SmsServiceUrl"].ToString()
                + "?user=" + ServiceSettings.Rows[0]["UBESMSUser"].ToString()
                + "&password=" + ServiceSettings.Rows[0]["UBESMSPass"].ToString()
                + "&sender=" + ServiceSettings.Rows[0]["UBESMSSender"].ToString()
                + "&SMSText=" + enrollmentMessage
                + "&GSM=" + oEnrollUByEmaarMember.MobileNo;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(smsServiceRequest);
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                req.Proxy = myProxy;
            }
            req.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            req.Credentials = System.Net.CredentialCache.DefaultCredentials;
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    logId = analizer.InsertServiceLog(0, "request"
                        , smsServiceRequest
                        , oEnrollUByEmaarMember.DeviceType
                        , oEnrollUByEmaarMember.MobileNo, string.Empty, string.Empty, System.Reflection.MethodBase.GetCurrentMethod().Name
                        , "SMS", String.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }

            // Trust All Certificates.
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            //response
            using (WebResponse wResponse = req.GetResponse())
            {

                using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                {
                    string jsonResponse = readStream.ReadToEnd();

                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        try
                        {
                            analizer.UpdateServiceLog(logId, "response", jsonResponse, string.Empty);
                        }
                        catch (Exception ex)
                        {
                            analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                        }
                    }
                }
            }
            return enrollmentMessage;
        }
    }
}