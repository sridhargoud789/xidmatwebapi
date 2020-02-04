using EEG_ReelCinemasRESTAPI.Common;
using EEG_ReelCinemasRESTAPI.Common.Smartbutton;
using EEG_ReelCinemasRESTAPI.Models;
using EEG_ReelCinemasRESTAPI.Models.CompleteOrderByUBELoyalty;
using EEG_ReelCinemasRESTAPI.Models.Response;
using EEG_ReelCinemasRESTAPI.Smartbutton.Member;
using EEG_ReelCinemasRESTAPI.Smartbutton.MemberActivity;
using EEG_ReelCinemasRESTAPI.Smartbutton.MemberContact;
using EEG_ReelCinemasRESTAPI.Smartbutton.MemberSecurity;
using EEG_ReelCinemasRESTAPI.Smartbutton.Offer;
using EEG_ReelCinemasRESTAPI.Smartbutton.Portal;
using EEG_ReelCinemasRESTAPI.Smartbutton.Reward;
using EEG_ReelCinemasRESTAPI.Smartbutton.Transaction;
using Newtonsoft.Json;
using ReelDAO;
using ReelDvo;
using ReelDVO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NsAddConcession = EEG_ReelCinemasRESTAPI.Models.Response.AddConcession;
using NsAddTicket = EEG_ReelCinemasRESTAPI.Models.Response.AddTicket;
using NsConcessionList = EEG_ReelCinemasRESTAPI.Models.Response.ConcessionList;
using NsGetOrder = EEG_ReelCinemasRESTAPI.Models.Response.GetOrder;
using NsGiftCard = EEG_ReelCinemasRESTAPI.Models.Response.GiftCard;
using NsSeatPlan = EEG_ReelCinemasRESTAPI.Models.Response.SeatPlan;
using NsSetSeat = EEG_ReelCinemasRESTAPI.Models.Response.SetSeat;
using NsTicketTypes = EEG_ReelCinemasRESTAPI.Models.Response.TicketTypes;

namespace EEG_ReelCinemasRESTAPI.Common.Smartbutton
{
    public class RewardBl
    {
        public respIssueVariableReward IssueVariableReward(CompleteOrderByUBELoyaltyReq oCompleteOrderByUBELoyaltyReq, TransactionDetailsDvo oTransactionDetailsDvo)
        {
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            Int64 logId = 0;
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();

            Reward oReward = null;
            reqIssueVariableReward oReqIssueVariableReward = null;
            respIssueVariableReward oRespIssueVariableReward = null;

            // analizer.WriteOptionalEventAndExceptions("CompleteOrderLoyaltyCard RespExternalPayStart result  for Transaction No : " + oTransactionDetailsDvo.ReferenceNo + " is " + respExternalPayStart.Result.ToString(), " ", oTransactionDetailsDvo.UserSessionId, baseAnalizer.Exception);

            oReqIssueVariableReward = new reqIssueVariableReward();
            oRespIssueVariableReward = new respIssueVariableReward();

            oReqIssueVariableReward.SecurityToken = oCompleteOrderByUBELoyaltyReq.SecurityToken;
            oReqIssueVariableReward.CardNumber = oTransactionDetailsDvo.MemberId;
            oReqIssueVariableReward.RewardTypeExternalReference = oCompleteOrderByUBELoyaltyReq.RewardTypeExternalReference;
            oReqIssueVariableReward.LocationExternalReference = oCompleteOrderByUBELoyaltyReq.LocationExternalReference;
            oReqIssueVariableReward.DollarAmountToRedeem = oCompleteOrderByUBELoyaltyReq.DollarAmountToRedeem;

            oReward = new Reward();
            oReward.Url = ServiceSettings.Rows[0]["SmartbuttonRewardUrl"].ToString();
            oReward.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            // block reward points
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                oReward.Proxy = myProxy;
            }

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                logId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReqIssueVariableReward), oTransactionDetailsDvo.CreatedBy
                    , oTransactionDetailsDvo.UserSessionId, oTransactionDetailsDvo.SessionId, oTransactionDetailsDvo.CinemaId, System.Reflection.MethodBase.GetCurrentMethod().Name
                    , "Reward.IssueVariableReward", String.Empty);
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            oRespIssueVariableReward = oReward.IssueVariableReward(oReqIssueVariableReward);

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oRespIssueVariableReward), oRespIssueVariableReward.ReturnCode.ToString());
            }
            return oRespIssueVariableReward;
        }
    }
}