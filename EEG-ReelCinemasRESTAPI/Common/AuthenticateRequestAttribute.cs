using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EEG_ReelCinemasRESTAPI.Common
{
    public class AuthenticateRequestAttribute: ActionFilterAttribute
    {

        #region Public Methods
        public override void OnActionExecuting(HttpActionContext context)
        {
            if (ConfigurationManager.AppSettings["IsAuthenticationOn"].ToString() == "1")
            {
                IEnumerable<string> values;
                object resultModel = null;
                try
                {
                    var b = context.Request.Content;
                    if (context.Request.Headers.TryGetValues("authUserName", out values) && values.First() == ConfigurationManager.AppSettings["authUserName"].ToString())
                    {
                        if (!(context.Request.Headers.TryGetValues("authPassword", out values) && values.First() == ConfigurationManager.AppSettings["authPassword"].ToString()))
                        {
                            //resultModel = new object;
                            //resultModel.Result = DisplayException("Kidzania API Authentication Failed.");
                            context.Response = new HttpResponseMessage()
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(resultModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), System.Text.Encoding.UTF8, "application/json")
                            };
                            return;

                        }
                    }
                    else
                    {
                        //resultModel = new LookupModel();
                        //resultModel.Result = DisplayException("Request header parameters are missing.Kidzania API Authentication Failed.");
                        context.Response = new HttpResponseMessage()
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(resultModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), System.Text.Encoding.UTF8, "application/json")
                        };
                        return;

                    }
                }
                catch (Exception ex)
                {
                    //resultModel = new LookupModel();
                    //resultModel.Result = DisplayException(ex.Message.ToString() + "Kidzania API Authentication Failed.");
                    context.Response = new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(resultModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), System.Text.Encoding.UTF8, "application/json")
                    };
                    return;
                }
            }

        }
        #endregion Public Methods

        #region Decryption

        public static string AesDecrypt(string cipherString)
        {
            //Set up the encryption objects
            using (RijndaelManaged acsp = GetProvider(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["SecurityKey"].ToString())))
            {
                byte[] rawBytes = StringToByteArray(cipherString);
                ICryptoTransform ictD = acsp.CreateDecryptor();

                //RawBytes now contains original byte array, still in Encrypted state

                //Decrypt into stream
                MemoryStream msD = new MemoryStream(rawBytes, 0, rawBytes.Length);
                CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                //csD now contains original byte array, fully decrypted

                //return the content of msD as a regular string
                return (new StreamReader(csD)).ReadToEnd();
            }
        }
        #endregion

        #region Encryption
        public static string AesEncrypt(string toEncrypt)
        {
            using (RijndaelManaged acsp = GetProvider(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["SecurityKey"].ToString())))
            {
                string hex = string.Empty;
                try
                {
                    byte[] sourceBytes = Encoding.ASCII.GetBytes(toEncrypt);
                    ICryptoTransform ictE = acsp.CreateEncryptor();

                    //Set up stream to contain the encryption
                    MemoryStream msS = new MemoryStream();

                    //Perform the encrpytion, storing output into the stream
                    CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                    csS.Write(sourceBytes, 0, sourceBytes.Length);
                    csS.FlushFinalBlock();

                    //sourceBytes are now encrypted as an array of secure bytes
                    byte[] encryptedBytes = msS.ToArray(); //.ToArray() is important, don't mess with the buffer

                    hex = BitConverter.ToString(encryptedBytes);
                    hex = hex.Replace("-", "").ToLower();
                }
                catch (Exception)
                {
                    throw;
                }

                return hex;
                //return the encrypted bytes as a BASE64 encoded string
                //return Convert.ToBase64String(encryptedBytes);
            }
        }
        #endregion

        #region Private
        private static RijndaelManaged GetProvider(byte[] key)
        {
            //LogRepository //logRepository = null;
            RijndaelManaged result = new RijndaelManaged();
            try
            {
                result.BlockSize = 128;
                result.KeySize = 128;
                result.Mode = CipherMode.CBC;
                result.Padding = PaddingMode.PKCS7;

                result.GenerateIV();
                result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                byte[] realKey = GetKey(key, result);
                result.Key = realKey;
                //result.IV = RealKey;
            }
            catch (Exception ex)
            {
                //logRepository = new LogRepository();
                //logRepository.InsertErrorLog("GetProvider - AuthenticateRequestAttribute.cs", ex.Message.ToString() + " StackTrace " + ex.StackTrace.ToString());
            }
            finally
            {
                //logRepository = null;
            }
            return result;
        }

        public static byte[] StringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }
        #endregion
    }
}