using EEG_ReelCinemasRESTAPI.Models;
using EEG_ReelCinemasRESTAPI.Smartbutton.Offer;
using Newtonsoft.Json;
using ReelDAO;
using ReelDvo;
using ReelDVO;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace EEG_ReelCinemasRESTAPI.Common.Smartbutton
{
    public class OfferBl
    {
        public MemberEligibleChoiceRewardItem MemberEligibleChoiceRewards(TransactionDetailsDvo oTransactionDetailsDvo)
        {
            Analizer analizer = new Analizer();
            long logId = 0;
            Offer oOffer = new Offer();
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            MemberEligibleChoiceRewardsReturn oMemberEligibleChoiceRewardsReturn = null;
            MemberEligibleChoiceRewardItem[] oListMemberEligibleChoiceRewardItem = null;
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();
            oOffer.Url = ServiceSettings.Rows[0]["SmartbuttonOfferUrl"].ToString();
            oOffer.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                oOffer.Proxy = myProxy;
            }

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                logId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oOffer), oTransactionDetailsDvo.CreatedBy
                    , oTransactionDetailsDvo.UserSessionId, oTransactionDetailsDvo.SessionId, oTransactionDetailsDvo.CinemaId, System.Reflection.MethodBase.GetCurrentMethod().Name
                    , "MemberEligibleChoiceRewards", string.Empty);
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            oMemberEligibleChoiceRewardsReturn = oOffer.MemberEligibleChoiceRewards(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString(), oTransactionDetailsDvo.MemberId, 0);

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oMemberEligibleChoiceRewardsReturn), oMemberEligibleChoiceRewardsReturn.ReturnCode.ToString());
            }

            oListMemberEligibleChoiceRewardItem = oMemberEligibleChoiceRewardsReturn.MemberEligibleChoiceRewards;

            MemberEligibleChoiceRewardItem oThreshold = oListMemberEligibleChoiceRewardItem.Where(item => item.RewardTypeExternalReference == "ERG_THRESHOLD").First();

            return oThreshold;

        }

        public CancelMemberRewardsReturn CancelMemberRewards(string securityToken, int[] rewardId, TransactionDetailsDvo transactionDetails,DataTable ServiceSettings)
        {
            Offer offerClient = null;
            CancelMemberRewardsReturn oCancelMemberRewardsReturn = null;
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            try
            {
                offerClient = new Offer();
                offerClient.Url = ServiceSettings.Rows[0]["SmartbuttonOfferUrl"].ToString();
                if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
                {
                    WebProxy myProxy = null;
                    myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                    if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                    {
                        myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                        ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                    }
                    offerClient.Proxy = myProxy;
                }
                offerClient.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                oCancelMemberRewardsReturn = offerClient.CancelMemberRewards(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString(), rewardId);
                return oCancelMemberRewardsReturn;
            }
            catch (Exception ex)
            {
                analizer.WriteOptionalEventAndExceptions("CancelMemberRewards Error for Transaction No : " + transactionDetails.ReferenceNo, " ", transactionDetails.UserSessionId, baseAnalizer.Exception);
                return null;
            }
            finally
            {
                offerClient = null;
            }
        }

        public RedeemMemberRewardsReturn RedeemMemberRewards(string securityToken, string locationExternalReference, int[] rewardId, TransactionDetailsDvo transactionDetails, DataTable ServiceSettings)
        {
            Offer offerClient = null;
            RedeemMemberRewardsReturn oRedeemMemberRewardsReturn = null;
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            try
            {
                offerClient = new Offer();
                offerClient.Url = ServiceSettings.Rows[0]["SmartbuttonOfferUrl"].ToString();
                if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
                {
                    WebProxy myProxy = null;
                    myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                    if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                    {
                        myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                        ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                    }
                    offerClient.Proxy = myProxy;
                }
                offerClient.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                oRedeemMemberRewardsReturn = offerClient.RedeemMemberRewards(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString()
                    , locationExternalReference, rewardId);
                return oRedeemMemberRewardsReturn;
            }
            catch (Exception ex)
            {
                analizer.WriteOptionalEventAndExceptions("RedeemMemberRewards Error for Transaction No : " + transactionDetails.ReferenceNo, " ", transactionDetails.UserSessionId, baseAnalizer.Exception);
                return null;
            }
            finally
            {
                offerClient = null;
            }
        }
    }
}