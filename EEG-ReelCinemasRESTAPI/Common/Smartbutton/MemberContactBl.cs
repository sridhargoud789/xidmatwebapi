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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
    public class MemberContactBl
    {
        public SaveMemberEmailAddressReturn SaveMemberEmailAddress(EnrollUByEmaarMember oEnrollUByEmaarMember)
        {
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            Int64 logId = 0;
            MemberContact oMemberContact = new MemberContact();
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();

            oMemberContact.Url = ServiceSettings.Rows[0]["SmartbuttonMemberContactUrl"].ToString();
            oMemberContact.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                oMemberContact.Proxy = myProxy;
            }
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    logId = 0;
                    logId = analizer.InsertServiceLog(0, "request"
                        , ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() + "," + oEnrollUByEmaarMember.MobileNo + ", 0," + oEnrollUByEmaarMember.Email + ", 1"
                        , oEnrollUByEmaarMember.DeviceType
                        , oEnrollUByEmaarMember.MobileNo, string.Empty, string.Empty, System.Reflection.MethodBase.GetCurrentMethod().Name
                        , "SaveMemberEmailAddress", String.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            SaveMemberEmailAddressReturn oSaveMemberEmailAddressReturn = oMemberContact.SaveMemberEmailAddress(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString()
                , oEnrollUByEmaarMember.MobileNo, 0, oEnrollUByEmaarMember.Email, 1);
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oSaveMemberEmailAddressReturn), string.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }
            return oSaveMemberEmailAddressReturn;
        }
    }
}