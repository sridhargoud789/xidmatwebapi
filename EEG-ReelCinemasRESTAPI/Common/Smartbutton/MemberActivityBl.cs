using EEG_ReelCinemasRESTAPI.Models;
using EEG_ReelCinemasRESTAPI.Smartbutton.MemberActivity;
using Newtonsoft.Json;
using ReelDAO;
using ReelDvo;
using System;
using System.Data;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace EEG_ReelCinemasRESTAPI.Common.Smartbutton
{
    public class MemberActivityBl
    {
        public FetchMemberPointBalancesReturn GetFetchMemberPointBalancesReturn(ValidateUBEMemberReq oValidateUBEMemberReq, DataTable ServiceSettings)
        {
            long logId = 0;
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();

            MemberActivity oMemberActivity = new MemberActivity();
            oMemberActivity.Url = ServiceSettings.Rows[0]["SmartbuttonMemberActivityUrl"].ToString();
            oMemberActivity.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                oMemberActivity.Proxy = myProxy;
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
            FetchMemberPointBalancesReturn oFetchMemberPointBalancesReturn = oMemberActivity.FetchMemberPointBalances("QD9z9DYmmp5V07tlLhysj40YSOQSkyTco8hK6L5R88yoWzrs72Lsx4t0sHP5Joc1", oValidateUBEMemberReq.UBEMemberId, 0);
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oFetchMemberPointBalancesReturn), string.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in ExternalLogin-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }

            return oFetchMemberPointBalancesReturn;
        }
    }
}