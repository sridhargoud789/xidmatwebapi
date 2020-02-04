using EEG_ReelCinemasRESTAPI.Models;
using EEG_ReelCinemasRESTAPI.Smartbutton.Member;
using EEG_ReelCinemasRESTAPI.Smartbutton.MemberSecurity;
using Newtonsoft.Json;
using ReelDAO;
using ReelDvo;
using System;
using System.Data;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EEG_ReelCinemasRESTAPI.Common.Smartbutton
{
    public class MemberSecurityBl
    {
        public SetPasswordReturn SetPassword(EnrollUByEmaarMember oEnrollUByEmaarMember,out string password)
        {
            password = "";
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            Int64 logId = 0;
            Member oMember = new Member();
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();
            MemberSecurity oMemberSecurity = new MemberSecurity();
            oMemberSecurity.Url = ServiceSettings.Rows[0]["SmartbuttonMemberSecurityUrl"].ToString();
            oMemberSecurity.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                oMemberSecurity.Proxy = myProxy;
            }

            password = GetUniqueKey(10);// "Qyetio3$";
            password = "Qyetio3$";

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    logId = analizer.InsertServiceLog(0, "request"
                        , ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() + "," + oEnrollUByEmaarMember.MobileNo + ", -1," + password
                        , oEnrollUByEmaarMember.DeviceType
                        , oEnrollUByEmaarMember.MobileNo, string.Empty, string.Empty, System.Reflection.MethodBase.GetCurrentMethod().Name
                        , "SetPassword", String.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            SetPasswordReturn oSetPasswordReturn = oMemberSecurity.SetPassword(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString(), oEnrollUByEmaarMember.MobileNo, -1, password);

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oSetPasswordReturn), string.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }
            return oSetPasswordReturn;
        }

        public SetPasswordReturn SetPasswordUserReg(EnrollUByEmaarMember oEnrollUByEmaarMember, string password)
        {
            password = "";
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            Int64 logId = 0;
            Member oMember = new Member();
            MobileBookingDao oMobileBookingDao = new MobileBookingDao();
            DataTable ServiceSettings = oMobileBookingDao.GetServiceSettings();
            MemberSecurity oMemberSecurity = new MemberSecurity();
            oMemberSecurity.Url = ServiceSettings.Rows[0]["SmartbuttonMemberSecurityUrl"].ToString();
            oMemberSecurity.Timeout = int.Parse(ServiceSettings.Rows[0]["ServiceTimeOutTime"].ToString());
            if (ServiceSettings.Rows[0]["PROXYENABLED"].ToString() == "1")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ServiceSettings.Rows[0]["ProxyAddress"].ToString(), int.Parse(ServiceSettings.Rows[0]["ProxyPort"].ToString()));
                if (ServiceSettings.Rows[0]["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ServiceSettings.Rows[0]["ProxyUser"].ToString(),
                    ServiceSettings.Rows[0]["ProxyPassword"].ToString());
                }
                oMemberSecurity.Proxy = myProxy;
            }


            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    logId = analizer.InsertServiceLog(0, "request"
                        , ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() + "," + oEnrollUByEmaarMember.MobileNo + ", -1," + password
                        , oEnrollUByEmaarMember.DeviceType
                        , oEnrollUByEmaarMember.MobileNo, string.Empty, string.Empty, System.Reflection.MethodBase.GetCurrentMethod().Name
                        , "SetPassword", String.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            SetPasswordReturn oSetPasswordReturn = oMemberSecurity.SetPassword(ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString(), oEnrollUByEmaarMember.MobileNo, -1, password);

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                try
                {
                    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oSetPasswordReturn), string.Empty);
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in EnrollUByEmaarMember-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, string.Empty, string.Empty, baseAnalizer.EXCEPTION);
                }
            }
            return oSetPasswordReturn;
        }
        private string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}