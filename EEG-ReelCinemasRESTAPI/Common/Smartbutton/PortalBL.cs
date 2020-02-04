using EEG_ReelCinemasRESTAPI.Models;
using EEG_ReelCinemasRESTAPI.Smartbutton.Portal;
using Newtonsoft.Json;
using ReelDAO;
using ReelDvo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using System.Web;

namespace EEG_ReelCinemasRESTAPI.Common.Smartbutton
{
    public class PortalBl
    {
        private Analizer analizer = null;
        private BaseAnalizer baseAnalizer = null;
        public respExternalLogin GetPortalExternalLogin(ValidateUBEMemberReq oValidateUBEMemberReq, DataTable ServiceSettings)
        {
            analizer = new Analizer();
            baseAnalizer = new BaseAnalizer();
            long logId = 0;
            Portal oPortal = new Portal();
            oPortal.Url = ServiceSettings.Rows[0]["SmartbuttonPortalUrl"].ToString();
            oPortal.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                oPortal.Proxy = myProxy;
            }

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    logId = 0;
                    logId = analizer.InsertServiceLog(0, "request"
                        , ServiceSettings.Rows[0]["SecurityTokenForSmartButtonServices"].ToString() + "," + oValidateUBEMemberReq.UBEMemberId
                        , oValidateUBEMemberReq.DeviceType
                        , oValidateUBEMemberReq.UBEMemberId, string.Empty, string.Empty, System.Reflection.MethodBase.GetCurrentMethod().Name
                        , "ExternalLogin", String.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in MemberRankingLevelForSet-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            //respExternalLogin oRespExternalLogin = oPortal.ExternalLogin(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonServices"].ToString(), "REEL_CINEMAS_MOB_TDM", oValidateUBEMemberReq.UBEMemberId, oValidateUBEMemberReq.UBEMemberPassword);
            respExternalLogin oRespExternalLogin = oPortal.ExternalLogin(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonServices"].ToString(), "REEL_CINEMAS_WEB", oValidateUBEMemberReq.UBEMemberId, oValidateUBEMemberReq.UBEMemberPassword);
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oRespExternalLogin), string.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in ExternalLogin-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }
            return oRespExternalLogin;
        }
    }
}