using EEG_ReelCinemasRESTAPI.Models;
using EEG_ReelCinemasRESTAPI.Smartbutton.Member;
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
    public class MemberBl
    {
        public EnrollMemberReturn EnrollMemberWithMobilePhoneNumber(EnrollUByEmaarMember oEnrollUByEmaarMember)
        {
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            Int64 logId = 0;
            Member oMember = new Member();
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();
            oMember.Url = ServiceSettings.Rows[0]["SmartbuttonMemberUrl"].ToString();
            oMember.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                oMember.Proxy = myProxy;
            }

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    logId = analizer.InsertServiceLog(0, "request"
                        , ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() + ",Valued, , Customer,"
                        + ", 1, 1, 1900, 0," + ServiceSettings.Rows[0]["LocationExternalReference"].ToString() + ",0," + oEnrollUByEmaarMember.MobileNo
                        , oEnrollUByEmaarMember.DeviceType
                        , oEnrollUByEmaarMember.MobileNo, string.Empty, string.Empty, System.Reflection.MethodBase.GetCurrentMethod().Name
                        , "EnrollMemberWithMobilePhoneNumber", String.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            EnrollMemberReturn oEnrollMemberReturn = oMember.EnrollMemberWithMobilePhoneNumber(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString(),
                "Valued", string.Empty, "Customer", string.Empty, 1, 1, 1900, 0, ServiceSettings.Rows[0]["LocationExternalReference"].ToString(),
                0, oEnrollUByEmaarMember.MobileNo);
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oEnrollMemberReturn), string.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }
            return oEnrollMemberReturn;
        }

        public AddMemberToClubReturn AddMemberToClub(EnrollUByEmaarMember oEnrollUByEmaarMember)
        {
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            Int64 logId = 0;
            Member oMember = new Member();
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();

            string clubExtRef = ServiceSettings.Rows[0]["UBE-RegistrationClubExtRef"].ToString();

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    logId = analizer.InsertServiceLog(0, "request"
                        , ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() + "," + oEnrollUByEmaarMember.MobileNo + ", 0," + clubExtRef
                        , oEnrollUByEmaarMember.DeviceType
                        , oEnrollUByEmaarMember.MobileNo, string.Empty, string.Empty, System.Reflection.MethodBase.GetCurrentMethod().Name
                        , "AddMemberToClub", String.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            AddMemberToClubReturn oAddMemberToClubReturn = oMember.AddMemberToClub(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString(), oEnrollUByEmaarMember.MobileNo, 0, clubExtRef);

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oAddMemberToClubReturn), string.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }
            return oAddMemberToClubReturn;
        }
    }
}