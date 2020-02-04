using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EEG_ReelCinemasWebsite.Common
{
   public static class WebRequest
    {
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
    }

   
}
