using EEG_ReelCinemasRESTAPI.Models;
using EEG_ReelCinemasRESTAPI.Models.Response;
using EEG_ReelCinemasRESTAPI.Models.Response.SeatPlan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReelDAO;
using ReelDvo;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Common
{
    public static class WebRequestForMobile
    {

        public class DecimalJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(decimal));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.String)
                {
                    string value = (string)reader.Value;
                    if (value == string.Empty)
                    {
                        return decimal.MinValue;
                    }
                }
                else if (reader.TokenType == JsonToken.Float ||
                         reader.TokenType == JsonToken.Integer)
                {
                    return Convert.ToDecimal(reader.Value);
                }

                throw new JsonSerializationException("Unexpected token type: " + reader.TokenType.ToString());
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                decimal dec = (decimal)value;
                if (dec == decimal.MinValue)
                {
                    writer.WriteValue(string.Empty);
                }
                else
                {
                    writer.WriteValue(dec);
                }
            }
        }

        public static string CreateWebRequest(string url, string apitoken)
        {
            string responseText = "";

            var httprequest = (HttpWebRequest)HttpWebRequest.Create(url);

            try
            {
                //Console.WriteLine(url);
                //Console.WriteLine(apitoken);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httprequest.PreAuthenticate = true;
                httprequest.Method = "GET";
                httprequest.ContentType = "application/json";

                httprequest.Headers.Add("connectapitoken", apitoken);
                WebResponse resp = httprequest.GetResponse();
                //Console.WriteLine(resp);
                using (var reader = new System.IO.StreamReader(resp.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
                //Console.WriteLine(responseText);
                resp.Close();

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                string reftext = "Exception occured on (ReelBAL)- CreateWebRequest  Reftext: $$RequestUrl:" + url + "";
                //objCom.ExceptionLogForDevelopers("OdataSync", reftext, ex);
                //LogMessage(reftext + ">>>" + ex.ToString());
            }

            return responseText;
        }
        public async static Task<VISTAResponse> CreateWebRequest<T>(string url, string apitoken, string methodName, string deviceType, string userSessionId)
        {
            object result = null;
            HttpResponseMessage response = new HttpResponseMessage();
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                //if (methodName != "GetTickets" && methodName != "GetConcessionList")
                //{
                //    client.DefaultRequestHeaders.Add("connectapitoken", apitoken);
                //}
                client.DefaultRequestHeaders.Add("connectapitoken", apitoken);
                try
                {
                    response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        if (methodName == "GetSeatPlan")
                        {
                            result = JsonConvert.DeserializeObject<GetSeatPlanResp>(content);
                        }
                        else
                        {
                            result = JsonConvert.DeserializeObject<T>(content);
                        }




                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception((int)response.StatusCode + "-" + response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in " + methodName + "-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, deviceType, userSessionId, baseAnalizer.EXCEPTION);
                }
            }
            VISTAResponse vistaResponse = new VISTAResponse();
            vistaResponse.Response = response;
            vistaResponse.Result = result;
            return vistaResponse;
        }
        public async static Task<VISTAResponse> CreateWebPostRequest<T>(string url, string apitoken, object data, string methodName, string deviceType, string userSessionId)
        {

            // T result = default(T);
            object result = null;
            HttpResponseMessage response = new HttpResponseMessage();
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var content = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("connectapitoken", apitoken);
                try
                {
                    response = await client.PostAsync(client.BaseAddress, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        string contentResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<T>(contentResponse);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception((int)response.StatusCode + "-" + response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in " + methodName + "-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, deviceType, userSessionId, baseAnalizer.EXCEPTION);
                }
            }

            VISTAResponse vistaResponse = new VISTAResponse();
            vistaResponse.Response = response;
            vistaResponse.Result = result;
            return vistaResponse;
        }

        public static string CreateWPR(string url, string apitoken, string data, string methodName, string deviceType, string userSessionId)
        {

            // T result = default(T);
            string result = null;
            HttpResponseMessage response = new HttpResponseMessage();
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();

            HttpWebRequest myRequest = null;
            byte[] bData = null;
            Stream oStream = null;
            HttpWebResponse resp = null;
            StreamReader responseReader = null;
            try
            {
                myRequest = (HttpWebRequest)WebRequest.Create(url);


                myRequest.ContentType = "application/json; charset=utf-8";


                myRequest.Headers["connectapitoken"] = apitoken;
                myRequest.Timeout = 120000;
                var content = JsonConvert.SerializeObject(data);
                bData = Encoding.UTF8.GetBytes(data);
                myRequest.Method = "POST";



                myRequest.ContentLength = bData.Length;
                oStream = myRequest.GetRequestStream();
                oStream.Write(bData, 0, bData.Length);

                using (resp = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (responseReader = new StreamReader(resp.GetResponseStream()))
                    {
                        result = responseReader.ReadToEnd();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
                analizer.WriteEventAndExceptions("Exception in " + methodName + "-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, deviceType, userSessionId, baseAnalizer.EXCEPTION);
            }
            return result;
        }


        public static string WebServicesDoPost(string url, string postData)
        {
            string userName = "";
            string userPassword = "";
            string oResp = string.Empty;
            HttpWebRequest myRequest = null;
            byte[] bData = null;
            Stream oStream = null;
            //string encU = null;
            //string encP = null;
            HttpWebResponse resp = null;
            StreamReader responseReader = null;
            try
            {


                //     url = AuthenticationHelper.AesDecrypt(url);

                myRequest = (HttpWebRequest)WebRequest.Create(url);


                myRequest.ContentType = "application/json; charset=utf-8";

                if (!string.IsNullOrEmpty(userName))
                {
                    string authInfo = "";// AuthenticationHelper.AesDecrypt(userName) + ":" + AuthenticationHelper.AesDecrypt(userPassword);
                    //  string authInfo = userName + ":" + userPassword;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    myRequest.Headers["Authorization"] = "Basic " + authInfo;
                }

                myRequest.Headers["accesstoken"] = ConfigurationManager.AppSettings["InfluxAccessToken"].ToString();
                myRequest.Timeout = 120000;
                bData = Encoding.UTF8.GetBytes(postData);
                myRequest.Method = "POST";



                myRequest.ContentLength = bData.Length;
                oStream = myRequest.GetRequestStream();
                oStream.Write(bData, 0, bData.Length);

                using (resp = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (responseReader = new StreamReader(resp.GetResponseStream()))
                    {
                        oResp = responseReader.ReadToEnd();
                    }
                }
            }
            catch
            {

                oResp = string.Empty;
            }
            finally
            {
                myRequest = null;
                bData = null;
                if (oStream != null)
                {
                    oStream.Close();
                    oStream.Dispose();
                    oStream = null;
                }
                if (resp != null)
                {
                    resp.Close();
                    resp.Dispose();
                    resp = null;
                }
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader.Dispose();
                    responseReader = null;
                }
                //rijndaelKey = null;
            }
            return oResp;
        }

        public static string EmaarOTPWebRequest(string JSON, string url)
        {
            var dataToSend = Encoding.UTF8.GetBytes(JSON);
            var httprequest = (HttpWebRequest)HttpWebRequest.Create(url);
            string response = "";
            try
            {

                //CreateLog("", "EMAAR-INFO", "Common", "EmaarWebRequest", "", "EMAAR-REQUEST", url);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //To enable TLS 1.2
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                httprequest.PreAuthenticate = true;
                httprequest.Method = "POST";
                httprequest.ContentType = "application/json";
                httprequest.Headers.Add("ClientProfileId", "RC_DG_ID");
                httprequest.AllowAutoRedirect = true;

                httprequest.ContentLength = dataToSend.Length;
                using (Stream requestStream = httprequest.GetRequestStream())
                {
                    requestStream.Write(dataToSend, 0, dataToSend.Length);
                }

                WebResponse resp = httprequest.GetResponse();
                var encoding = Encoding.UTF8;
                using (var reader = new StreamReader(resp.GetResponseStream(), encoding))
                {
                    response = reader.ReadToEnd();
                }
                resp.Close();
                
            }
            catch (Exception ex)
            {

                
            }
            return response;
        }

        public static string EmaarWebRequest(string JSON, string url)
        {
            var dataToSend = Encoding.UTF8.GetBytes(JSON);
            var httprequest = (HttpWebRequest)HttpWebRequest.Create(url);
            string response = "";
            try
            {

                //CreateLog("", "EMAAR-INFO", "Common", "EmaarWebRequest", "", "EMAAR-REQUEST", url);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //To enable TLS 1.2
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                httprequest.PreAuthenticate = true;
                httprequest.Method = "POST";
                httprequest.ContentType = "application/json";
                httprequest.Headers.Add("ClientProfileId", "RC_DG_ID");
                httprequest.AllowAutoRedirect = true;

                httprequest.ContentLength = dataToSend.Length;


                if (ConfigurationManager.AppSettings["isProxyEnabled"].ToString() == "True")
                {
                    var proxy = new WebProxy(ConfigurationManager.AppSettings["ProxyURL"].ToString());
                    proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ProxyUserName"].ToString(), ConfigurationManager.AppSettings["ProxyPassword"].ToString());
                    httprequest.Proxy = proxy;
                }
                using (Stream requestStream = httprequest.GetRequestStream())
                {
                    requestStream.Write(dataToSend, 0, dataToSend.Length);
                }

                WebResponse resp = httprequest.GetResponse();
                var encoding = Encoding.UTF8;
                using (var reader = new StreamReader(resp.GetResponseStream(), encoding))
                {
                    response = reader.ReadToEnd();
                }
                resp.Close();
             
            }
            catch (Exception ex)
            {

               
            }
            return response;
        }

        public static string WebRequestGet(string url, out bool isException, out string strExceptionMsg)
        {
            string oResp = string.Empty;
            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", ConfigurationManager.AppSettings["NoonPaymentAuthKey"].ToString());


                if (ConfigurationManager.AppSettings["isProxyEnabled"].ToString() == "True")
                {
                    var proxy = new WebProxy(ConfigurationManager.AppSettings["ProxyURL"].ToString());
                    proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ProxyUserName"].ToString(), ConfigurationManager.AppSettings["ProxyPassword"].ToString());
                    client.Proxy = proxy;
                }

                IRestResponse response = client.Execute(request);
                oResp = response.Content;
                isException = false;
                strExceptionMsg = "";
            }
            catch (Exception ex)
            {
                isException = true;
                strExceptionMsg = ex.Message;
            }
            return oResp;
        }

        public static string NoonPaymentWebRequest(string url, string postData, out bool isException, out string strExceptionMsg, string Method="POST")
        {
            string oResp = string.Empty;
            HttpWebRequest myRequest = null;
            byte[] bData = null;
            Stream oStream = null;
            //string encU = null;
            //string encP = null;
            HttpWebResponse resp = null;
            StreamReader responseReader = null;
            try
            {
                myRequest = (HttpWebRequest)WebRequest.Create(url);

                if (ConfigurationManager.AppSettings["isProxyEnabled"].ToString() == "True")
                {
                    var proxy = new WebProxy(ConfigurationManager.AppSettings["ProxyURL"].ToString());
                    proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ProxyUserName"].ToString(), ConfigurationManager.AppSettings["ProxyPassword"].ToString());
                    myRequest.Proxy = proxy;
                }            

                myRequest.ContentType = "application/json; charset=utf-8";

                myRequest.Headers["Authorization"] = ConfigurationManager.AppSettings["NoonPaymentAuthKey"].ToString();

                myRequest.Timeout = 120000;
                if (!string.IsNullOrEmpty(postData))
                {
                    bData = Encoding.UTF8.GetBytes(postData);
                    myRequest.ContentLength = bData.Length;

                }

                myRequest.Method = Method;

                oStream = myRequest.GetRequestStream();
                oStream.Write(bData, 0, bData.Length);

                using (resp = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (responseReader = new StreamReader(resp.GetResponseStream()))
                    {
                        oResp = responseReader.ReadToEnd();
                    }
                }
                isException = false;
                strExceptionMsg = "";
            }
            catch (Exception ex)
            {
                isException = true;
                strExceptionMsg = ex.Message;
                oResp = string.Empty;
            }
            finally
            {
                myRequest = null;
                bData = null;
                if (oStream != null)
                {
                    oStream.Close();
                    oStream.Dispose();
                    oStream = null;
                }
                if (resp != null)
                {
                    resp.Close();
                    resp.Dispose();
                    resp = null;
                }
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader.Dispose();
                    responseReader = null;
                }
                //rijndaelKey = null;
            }
            return oResp;
        }
        public async static Task<InfluxResponse> CreateInfluxWebPostRequest<T>(string url, string apitoken, object data, string methodName, string deviceType, string userSessionId)
        {
            // T result = default(T);
            object result = null;
            HttpResponseMessage response = new HttpResponseMessage();
            Analizer analizer = new Analizer();
            BaseAnalizer baseAnalizer = new BaseAnalizer();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var content = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("accesstoken", apitoken);
                try
                {
                    response = await client.PostAsync(url, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        string contentResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<T>(contentResponse);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception((int)response.StatusCode + "-" + response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    analizer.WriteEventAndExceptions("Exception in " + methodName + "-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, deviceType, userSessionId, baseAnalizer.EXCEPTION);
                }
            }

            InfluxResponse influxResponse = new InfluxResponse();
            influxResponse.Response = response;
            influxResponse.Result = result;
            return influxResponse;
        }
    }
}