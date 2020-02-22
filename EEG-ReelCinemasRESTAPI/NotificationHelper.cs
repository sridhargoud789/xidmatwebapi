using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using RestSharp;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using EEG_ReelCinemasRESTAPI.Models.Response;
using EEG_ReelCinemasRESTAPI.Models;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using ReelDAO;
using System.Xml.Serialization;

namespace EEG_ReelCinemasRESTAPI
{
    public class NotificationHelper
    {
        public string SendSMSNotification(string sessionId, string VistaBookingId, string SeatList, string Phone)
        {
            string strOutput = string.Empty;
            try
            {
                DateTime MovieDateTime = DateTime.Now; ;
                string CinemaName = string.Empty;
                string MovieName = string.Empty;
                string ScreenName = string.Empty;


                var sessionlist = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["SessionListJSONPath"]);

                List<SessionListRespData> oSL = new List<SessionListRespData>();
                oSL = JsonConvert.DeserializeObject<List<SessionListRespData>>(sessionlist);

                foreach (var ms in oSL)
                {
                    foreach (var s in ms.MSLst)
                    {
                        if (s.SID == sessionId)
                        {
                            MovieDateTime = Convert.ToDateTime(s.SD);
                            CinemaName = s.CN;
                            MovieName = ms.MN;
                            ScreenName = s.SC;
                        }
                    }
                }
                string MDateTime = MovieDateTime.ToString("MMM dd, yyyy HH:mm");

                string strSMSText = "BookingNo-" + VistaBookingId + ", Cinema-" + CinemaName + ", Movie-" + MovieName + ", Screen-" +
                    ScreenName + ", Show-" + MDateTime + ", Seat-" + SeatList.Replace("|", "");

                // + ",Ticket-" + TICKETDETAIL.Replace("|", "") + ",Price-AED " + dr["GrandTotal"].ToString() + ",VAT 5% Incl"

                if (ConfigurationManager.AppSettings["SMSSendBarcode"].ToString() == "True")
                {
                    strSMSText = strSMSText + ". " + ConfigurationManager.AppSettings["SMSBarcode"].ToString() + "/" + VistaBookingId;
                }


                Phone = "00" + Phone;

                long externalRequestLogId = 0;

                MobileBookingDao oMobileBookingDao = new MobileBookingDao();

                Analizer analizer = null;
                analizer = new Analizer();


                string SMSURL = "http://jwey4.api.infobip.com/sms/2/text/single";
                externalRequestLogId = analizer.InsertServiceLog(0, "request", strSMSText, null, Phone, null, null, "SendSMSNotification", "restTicketing.SendSMSNotification", string.Empty);


                string resp = SendSMS(Phone, strSMSText);
                ////Send SMS Request
                //var client = new RestClient(SMSURL);
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("accept", "application/json");
                //request.AddHeader("content-type", "application/json");
                //request.AddHeader("authorization", "Basic cnVzZXI6S1VTNHVaa2U=");
                //request.AddParameter("application/json", "{\"from\":\"REELCINEMAS\", \"to\":\"" + Phone + "\",\"text\":\"" + strSMSText + "\"}", ParameterType.RequestBody);

                ////To establish trust relationship for the SSL/TLS secure channel.
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ////To enable TLS 1.2
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //if (ConfigurationManager.AppSettings["isProxyEnabled"].ToString() == "True")
                //{
                //    var proxy = new WebProxy(ConfigurationManager.AppSettings["ProxyURL"].ToString());
                //    proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ProxyUserName"].ToString(), ConfigurationManager.AppSettings["ProxyPassword"].ToString());
                //    client.Proxy = proxy;
                //}


                //// Trust All Certificates.
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                //IRestResponse response = client.Execute(request); 

                //analizer.UpdateServiceLog(externalRequestLogId, "response", response.Content, "");


                //string currentDateTime11 = DateTime.Now.ToString("yyyyMMdd");
                //string strPath11 = @"" + ConfigurationManager.AppSettings["ExceptionFolder"].ToString() + currentDateTime11 + "SMSResp";

                //using (StreamWriter sw = File.AppendText(strPath11))
                //{
                //    sw.WriteLine("=============SMS Sender Response ===========");
                //    sw.WriteLine("===========Start============= " + DateTime.Now);
                //    sw.WriteLine("Phone: " + Phone);
                //    sw.WriteLine("Response Message: " + response.Content);
                //    sw.WriteLine("===========End============= " + DateTime.Now);
                //    sw.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);

                //}


            }
            catch (System.Exception ex)
            {
                //Exception Logging
                string currentDateTime1 = DateTime.Now.ToString("yyyyMMdd");
                string strPath1 = @"" + ConfigurationManager.AppSettings["ExceptionFolder"].ToString() + currentDateTime1 + "SMSException";

                using (StreamWriter sw = System.IO.File.AppendText(strPath1))
                {
                    sw.WriteLine("=============Error Logging WEBSITE ===========");
                    sw.WriteLine("===========Start============= " + DateTime.Now);
                    sw.WriteLine("Error Message: " + ex.Message);
                    sw.WriteLine("Stack Trace: " + ex.StackTrace);
                    sw.WriteLine("===========End============= " + DateTime.Now);
                    sw.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);

                }
            }
            return strOutput;
            //SMS Sender
        }

        private string SendSMS(string strPhoneNo, string strSMSText)
        {
            string SMSServiceUsername = ConfigurationManager.AppSettings["SMSServiceUsername"].ToString();
            string SMSServicePwd = ConfigurationManager.AppSettings["SMSServicePwd"].ToString();            
            string strSMSWebRequestURL = ConfigurationManager.AppSettings["SMSServiceURL"].ToString() + "?user=" + SMSServiceUsername + "&password=" + SMSServicePwd;
            strSMSWebRequestURL += "&sender=" + "REELCINEMAS" + "&SMSText=" + strSMSText + "&GSM=" + strPhoneNo;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strSMSWebRequestURL);
            if (ConfigurationManager.AppSettings["isProxyEnabled"].ToString() == "True")
            {
                WebProxy myProxy = null;
                myProxy = new WebProxy(ConfigurationManager.AppSettings["ProxyAddress"].ToString(), int.Parse(ConfigurationManager.AppSettings["ProxyPort"].ToString()));
                if (ConfigurationManager.AppSettings["ProxyUserREQ"].ToString() == "1")
                {
                    myProxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ProxyUserName"].ToString(),
                    ConfigurationManager.AppSettings["ProxyPassword"].ToString());
                }
                req.Proxy = myProxy;
            }

            // Trust All Certificates.
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
             ((sender, certificate, chain, sslPolicyErrors) => true);
            //SMS OTP Send response
            SMSResponseServiceModel objSMSResponse = new SMSResponseServiceModel();
            using (WebResponse wResponse = req.GetResponse())
            {
                using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                {
                    string jsonResponse = readStream.ReadToEnd();
                    XmlSerializer serializer = new XmlSerializer(typeof(SMSResponseServiceModel));
                    using (StringReader reader = new StringReader(jsonResponse))
                    {
                        objSMSResponse = (SMSResponseServiceModel)(serializer.Deserialize(reader));
                    }
                }
            }

            return objSMSResponse.SMSResponse.Status;
        }
        public string BookingQRcode(string bookingId)
        {
            string qrCodeUrl = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(bookingId))
                {
                    if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/QRCode/") + bookingId + ".png"))
                    {
                        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                        String encoding = "Byte";
                        if (encoding == "Byte")
                        {
                            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                        }

                        try
                        {
                            int scale = Convert.ToInt16(5);
                            qrCodeEncoder.QRCodeScale = scale;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            int version = Convert.ToInt16(1);
                            qrCodeEncoder.QRCodeVersion = version;
                        }
                        catch (Exception)
                        {
                        }
                        string errorCorrect = "L";
                        if (errorCorrect == "L")
                            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                        else if (errorCorrect == "M")
                            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                        else if (errorCorrect == "Q")
                            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                        else if (errorCorrect == "H")
                            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;

                        System.Drawing.Image image;
                        String data = bookingId;
                        image = qrCodeEncoder.Encode(data);
                        image.Save(HttpContext.Current.Server.MapPath("~/QRCode/") + bookingId + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                    qrCodeUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/WebApi/QRCode/" + bookingId + ".png";
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return qrCodeUrl;
        }


        public string BookingEmailHTML(BookingSummary bookingSummary)
        {
            string html = string.Empty;
            
            try
            {
                string foodDesc = string.Empty;
                decimal foodAmount = 0;
                if (bookingSummary.FoodInfoList != null)
                {


                    if (bookingSummary.FoodInfoList.Count > 0)
                    {

                        foodDesc = foodDesc + "<table width = '100%' border = '0' cellspacing = '0' cellpadding = '0'>";
                        foodDesc = foodDesc + "<tbody>";
                        foodDesc = foodDesc + "<tr>";
                        foodDesc = foodDesc + "<td width='22' align='center'><img src ='" + HttpContext.Current.Request.Url.Authority + "/webapi/Emailer/images/icon-food.jpg' width = '22' height='17' alt = ''/></td>";
                        foodDesc = foodDesc + "<td width = '12'> &nbsp;</td>";
                        foodDesc = foodDesc + "<td style = 'font-size:13px; color:#222222; font-family:'HelveticaNeue',Helvetica,Arial,Verdana,sans-serif;'><strong>FoodDetails:</strong></td>";
                        foodDesc = foodDesc + "</tr>";
                        foodDesc = foodDesc + "</tbody>";
                        foodDesc = foodDesc + "</table>";


                        foodDesc = foodDesc + "<table width = '100 %' border = '0' cellspacing = '0' cellpadding = '0'>";
                        foodDesc = foodDesc + "<tbody>";
                        bool row = true;
                        foreach (var item in bookingSummary.FoodInfoList)
                        {
                            if (row)
                            {
                                foodDesc = foodDesc + "<tr>";
                                foodDesc = foodDesc + "<td height ='10'></td>";
                                foodDesc = foodDesc + "<td></td>";
                                foodDesc = foodDesc + "<td></td>";
                                foodDesc = foodDesc + "<td></td>";
                                foodDesc = foodDesc + "</tr>";

                                foodDesc = foodDesc + "<tr>";
                                foodDesc = foodDesc + "<td width='34'></td>";
                                foodDesc = foodDesc + "<td width = '236' style = 'font-size: 13px; color: #222222; font-family: 'Helvetica Neue', Helvetica, Arial, Verdana, sans-serif; line-height: 20px;'>";
                                foodDesc = foodDesc + item.Description + " - " + item.Quantity + "<br/>";
                                //foodDesc = foodDesc + "<span style = 'font -size: 12px; color: #767676;'> Add - on(Jalapeno, Fries, Cheese)</span>";
                                foodDesc = foodDesc + "</td>";
                                row = false;
                            }
                            else
                            {
                                foodDesc = foodDesc + "<td width ='10'></td>";
                                foodDesc = foodDesc + "<td style = 'font -size: 13px; color: #222222; font-family: 'Helvetica Neue', Helvetica, Arial, Verdana, sans-serif; line-height: 20px;'>";
                                foodDesc = foodDesc + item.Description + " - " + item.Quantity + "<br/>";
                                //foodDesc = foodDesc + "<span style = 'font -size: 12px; color: #767676;' > Add - on(Jalapeno, Fries, Cheese) </ span > ";
                                foodDesc = foodDesc + "</td>";
                                foodDesc = foodDesc + "</tr> ";
                                row = true;
                            }



                            //foodDesc += item.Name + " " + item.Description + " AED " + Convert.ToDecimal(item.UnitAmount * item.Quantity).ToString("0.00") + Environment.NewLine;
                            //foodAmount += item.UnitAmount * item.Quantity;
                        }

                        if (!row)
                        {
                            foodDesc = foodDesc + "</tr> ";
                        }

                        foodDesc = foodDesc + "</tbody>";
                        foodDesc = foodDesc + "</table>";
                    }
                }
                html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Emailer/" + bookingSummary.EmailTemplate));

                decimal totalticketprice = bookingSummary.TicketAmount - bookingSummary.VatAmount;

                //html = html.Replace("$Name$", bookingSummary.FirstName + " " + bookingSummary.LastName).
                html = html.Replace("$DomainName$", HttpContext.Current.Request.Url.Authority).
                Replace("$BookingId$", bookingSummary.BookingId).
                Replace("$BookingInfoId$", Convert.ToString(bookingSummary.BookingInfoId)).
                Replace("$NAME$", Convert.ToString(bookingSummary.FirstName)).
                Replace("$MovieId$", Convert.ToString(bookingSummary.MovieId)).
                //Replace("$Email$", bookingSummary.EmailId).
                //Replace("$PhoneNo$", bookingSummary.PhoneNo).
                Replace("$Barcodeimage$", bookingSummary.QRCodeUrl == null ? "" : bookingSummary.QRCodeUrl.ToString()).
                Replace("$MovieName$", bookingSummary.MovieName).
                Replace("$Rating$", bookingSummary.Rating).
                Replace("$Location$", bookingSummary.CinemaName).
                Replace("$ShowDate$", bookingSummary.Showtime.ToString("ddd dd MMM yyyy")).
                Replace("$ShowTime$", bookingSummary.Showtime.ToString("HH:mm")).
                Replace("$Screen$", bookingSummary.ScreenName).
                Replace("$TicketDetails$", bookingSummary.TicketInfoList==null?"": TicketTypeText(bookingSummary.TicketInfoList)).
                Replace("$CONCESSIONDETAIL$", foodDesc).
                Replace("$SeatDetails$", bookingSummary.TicketInfoList == null?"": string.Join(",", bookingSummary.TicketInfoList.Select(l => l.SeatInfo).ToList())).
                Replace("$TotalTicketAmount$", totalticketprice.ToString("0.00")).
                Replace("$VatAmount$", Convert.ToDecimal(bookingSummary.VatAmount).ToString("0.00")).
                //Replace("$Tickets$", bookingSummary.TicketInfoList.Sum(item => item.Quantity) + " TICKET(S)").
                Replace("$Total$", Convert.ToDecimal(bookingSummary.TotalAmount).ToString("0.00")).
                Replace("$DISCOUNT$", bookingSummary.OfferMessage==null?"":Convert.ToString(bookingSummary.OfferMessage)).
                Replace("$LocationPageUrl$", bookingSummary.LocationPageUrl ==null?"":HttpContext.Current.Request.Url.Authority + Convert.ToString(bookingSummary.LocationPageUrl)).
                Replace("$AddWalletUrl$", bookingSummary.AddWalletUrl==null?"":Convert.ToString(bookingSummary.AddWalletUrl)).
                Replace("$AddCalendarUrl$", bookingSummary.AddCalendarUrl == null ? "" : Convert.ToString(bookingSummary.AddCalendarUrl));

                ConfirmationBanners confirmationBanners = OffersBanners(bookingSummary.CinemaId, bookingSummary.Experience);
                if (confirmationBanners != null)
                {
                    html = html.Replace("$OffersBanners1HTML$", confirmationBanners.Banner1 == null?"": OffersBanners1HTML(confirmationBanners.Banner1));
                    html = html.Replace("$OffersBanners2HTML$", confirmationBanners.Banner2 == null ? "" : OffersBanners2HTML(confirmationBanners.Banner2));
                }
            }
            catch (Exception exception)
            {
               
            }
            return html;
        }
        public string OffersBanners1HTML(Banner1 banner1)
        {
            string banner1HTML = string.Empty;
            
            try
            {
                if (banner1.ShowBanner)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td bgcolor='#f8f8f8'>&nbsp;</td>");
                    stringBuilder.Append("<td bgcolor='#f8f8f8'>");
                    if (!string.IsNullOrEmpty(banner1.BannerImageUrl))
                    {
                        stringBuilder.Append("<table width='600' cellpadding ='0' cellspacing='0' border ='0' bgcolor='#002d44' align='center' valign ='top' style ='width:600px;background:url('" + banner1.BannerImageUrl + "') no-repeat left top;background-color:#002d44;background-size:cover;'>");
                    }
                    else
                    {
                        stringBuilder.Append("<table width='600' cellpadding ='0' cellspacing='0' border ='0' bgcolor='#002d44' align='center' valign ='top' style ='width:600px;background-color:#002d44;background-size:cover;'>");
                    }
                    stringBuilder.Append("<tr><td align='center' valign ='top'>");
                    stringBuilder.Append("<table width ='560' cellpadding ='0' cellspacing='0' border='0' align ='center' valign='top' style ='width:560px;'>");
                    stringBuilder.Append("<tr><td height='20' style='height:20px;'></td></tr>");

                    if (!string.IsNullOrEmpty(banner1.BannerTitle))
                    {
                        stringBuilder.Append("<tr><td align='left' style ='font-family:sans-serif;font-size:20px;line-height:24px;color:#a3ecfc;text-align:left;vertical-align:top;'>" + banner1.BannerTitle + "</td></tr>");
                    }

                    stringBuilder.Append("<tr><td height='10' style ='height:10px;'></td></tr>");

                    if (!string.IsNullOrEmpty(banner1.BannerInnerContent))
                    {
                        stringBuilder.Append("<tr><td align ='left' style='font-family:sans-serif;font-size:14px;line-height:16px;color:#ffffff;text-align:left;vertical-align:top;'>" + banner1.BannerInnerContent + "</td></tr>");
                    }

                    stringBuilder.Append("<tr><td height='10' style='height:10px;'></td></tr>");

                    if (!string.IsNullOrEmpty(banner1.BannerPointsHeading))
                    {
                        stringBuilder.Append("<tr><td align='left' style ='font-family:sans-serif;font-size:12px;line-height:14px;color:#ffffff;text-align:left;vertical-align:top;'>" + banner1.BannerPointsHeading + "</td></tr>");
                    }
                    stringBuilder.Append("<tr><td height ='10' style ='height:10px;'></td></tr>");

                    if (!string.IsNullOrEmpty(banner1.BannerPoints))
                    {
                        stringBuilder.Append("<tr><td align='left' valign ='top' style ='font-family:sans-serif;font-size:12px;line-height:18px;color:#ffffff;text-align:left;vertical-align:top;'>");
                        stringBuilder.Append("<ul style='padding-left:15px;margin:0;'>");
                        string[] values = banner1.BannerPoints.Split('|');
                        for (int i = 0; i < values.Length; i++)
                        {
                            stringBuilder.Append("<li style='list -style-type:disc;'>" + values[i] + "</li>");
                        }
                        stringBuilder.Append("</ul>");
                        stringBuilder.Append("</td></tr>");
                    }


                    if (!string.IsNullOrEmpty(banner1.RedirectUrlWithHeading) || !string.IsNullOrEmpty(banner1.BrandLogo))
                    {
                        stringBuilder.Append("<tr><td align='center' valign='top'>");
                        stringBuilder.Append("<table cellpadding ='0' cellspacing ='0' border ='0' align='center' valign ='top' height ='42' style='height:42px;'>");
                        stringBuilder.Append("<tr>");
                        if (!string.IsNullOrEmpty(banner1.RedirectUrlWithHeading))
                        {
                            stringBuilder.Append("<td width='376' valign='middle' align = 'left' style ='font-family:sans-serif;font-size:12px;line-height:14px;color:#ffffff;text-align:left;vertical-align:middle;width:376px;'>" + banner1.RedirectUrlWithHeading + "</td>");
                        }
                        if (!string.IsNullOrEmpty(banner1.BrandLogo))
                        {
                            stringBuilder.Append("<td width ='184' align='right' valign='middle' style ='vertical-align:middle;text-align:right'><img src='" + banner1.BrandLogo + "' style ='width:184px;height:42px;' alt='' title=''></td>");
                        }
                        stringBuilder.Append("</tr>");
                        stringBuilder.Append("</table>");
                        stringBuilder.Append("</td></tr>");
                    }

                    stringBuilder.Append("<tr><td height='20' style='height:20px;'></td></tr>");

                    stringBuilder.Append("</table>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("</table>");

                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td bgcolor='#f8f8f8'>&nbsp;</td>");
                    stringBuilder.Append("</tr>");
                    banner1HTML = stringBuilder.ToString();
                }
            }
            catch (Exception exception)
            {
              
                throw exception;
            }
            return banner1HTML;
        }

        public string OffersBanners2HTML(Banner2 banner2)
        {
            string banner2HTML = string.Empty;
            
            try
            {
                if (banner2.ShowBanner)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td bgcolor='#f8f8f8'>&nbsp;</td>");
                    stringBuilder.Append("<td bgcolor='#f8f8f8'>");
                    //stringBuilder.Append("</tr>");

                    //stringBuilder.Append("<tr>");
                    //stringBuilder.Append("<td valign='top' align='center'>");
                    if (!string.IsNullOrEmpty(banner2.BannerImageUrl))
                    {
                        stringBuilder.Append("<table width='600' cellpadding='0' cellspacing='0' border='0' bgcolor='#29161c' align='center' valign='top' style='width:600px;background:url('" + banner2.BannerImageUrl + "')no-repeat left top;background-color:#29161c;background-size:cover;'>");
                    }
                    else
                    {
                        stringBuilder.Append("<table width='600' cellpadding='0' cellspacing='0' border='0' bgcolor='#29161c' align='center' valign='top' style='width:600px;background-color:#29161c;background-size:cover;'>");
                    }
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='center' valign='top'>");
                    stringBuilder.Append("<table width='560' cellpadding='0' cellspacing='0' border='0' align='center' valign='top' style='width:560px;'>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td height='20' style='height:20px;'></td>");
                    stringBuilder.Append("</tr>");
                    if (!string.IsNullOrEmpty(banner2.BannerTitle))
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='left' style='font-family:sans-serif;font-size:20px;line-height:24px;color:#ffffff;text-align:left;vertical-align:top;'>" + banner2.BannerTitle + "</td>");
                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td height='10' style='height:10px;'></td>");
                    stringBuilder.Append("</tr>");
                    if (!string.IsNullOrEmpty(banner2.BannerInnerContent))
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='left' style='font-family:sans-serif;font-size:14px;line-height:16px;color:#ffffff;text-align:left;vertical-align:top;'>" + banner2.BannerInnerContent + "</td>");
                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td height='10' style='height:10px;'></td>");
                    stringBuilder.Append("</tr>");
                    if (!string.IsNullOrEmpty(banner2.BannerPointsHeading))
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='left' style='font-family:sans-serif;font-size:14px;line-height:16px;color:#ffffff;text-align:left;vertical-align:top;'>" + banner2.BannerPointsHeading + "</td>");
                        stringBuilder.Append("</tr>");
                    }

                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td height='10' style='height:10px;'></td>");
                    stringBuilder.Append("</tr>");

                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='center' valign='top'>");
                    stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' align='center'valign='top'>");

                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td width='470' align='left' valign='top' style='470px;'>");
                    stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' align='left' valign='top'>");
                    if (!string.IsNullOrEmpty(banner2.BannerPoints))
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='left' valign='top' style='font-family:sans-serif;font-size:12px;line-height:18px;color:#ffffff;text-align:left;vertical-align:top;'>");
                        stringBuilder.Append("<ul style='padding-left:15px;margin:0;'>");
                        string[] values = banner2.BannerPoints.Split('|');
                        for (int i = 0; i < values.Length; i++)
                        {
                            stringBuilder.Append("<li style='list-style-type:disc;'>" + values[i] + "</li>");
                        }
                        stringBuilder.Append("</ul>");
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("</tr>");
                    }

                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td height='10' style='height:10px;'></td>");
                    stringBuilder.Append("</tr>");


                    if (!string.IsNullOrEmpty(banner2.RedirectUrlWithHeading))
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td valign='middle' align='left' style='font-family:sans-serif;font-size:12px;line-height:14px;color:#ffffff;text-align:left;vertical-align:middle;'>" + banner2.RedirectUrlWithHeading + "</td>");
                        stringBuilder.Append("</tr>");
                    }

                    stringBuilder.Append("</table>");
                    stringBuilder.Append("</td>");


                    if (!string.IsNullOrEmpty(banner2.BrandLogo))
                    {
                        stringBuilder.Append("<td width='90' align='right' valign='bottom' style='vertical-align:bottom;text-align:right'><img src='" + banner2.BrandLogo + "' style='width:90px;height:66px;' alt='' title=''></td>");
                    }

                    stringBuilder.Append("</tr>");

                    stringBuilder.Append("</table>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");

                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td height='20' style='height:20px;'></td>");
                    stringBuilder.Append("</tr>");

                    stringBuilder.Append("</table>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("</table>");

                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td bgcolor='#f8f8f8'>&nbsp;</td>");
                    stringBuilder.Append("</tr>");
                    banner2HTML = stringBuilder.ToString();
                }
            }
            catch (Exception exception)
            {
                
                
            }
            return banner2HTML;
        }

        public string ReadJsonFile(string FileName, Boolean IsUTF8 = false)
        {
            string JsonStr = "";
            try
            {
                if (IsUTF8)
                {
                    JsonStr = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Json/" + FileName + ".Json"), Encoding.Default);
                }
                else
                {
                    JsonStr = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Json/" + FileName + ".Json"));
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return JsonStr;
        }

        public ConfirmationBanners OffersBanners(string cinemaId, string experience)
        {
            ConfirmationBanners confirmationBanners = new ConfirmationBanners();
            
            try
            {
                string confirmationBannerJson = ReadJsonFile("ConfirmationBanners", true);
                if (!string.IsNullOrEmpty(confirmationBannerJson))
                {
                    List<ConfirmationBanners> confirmationBannerList = JsonConvert.DeserializeObject<List<ConfirmationBanners>>(confirmationBannerJson);
                    confirmationBannerList = confirmationBannerList.Where(c => c.CinemaId == cinemaId).ToList();
                    confirmationBannerList = confirmationBannerList.Where(c => c.AllowedExperiences.Contains(experience)).ToList();
                    if (confirmationBannerList.Count > 0)
                    {
                        confirmationBanners = confirmationBannerList[0];
                    }
                }
            }
            catch (Exception exception)
            {
                
                throw exception;
            }
            return confirmationBanners;
        }

        public string TicketTypeText(List<TicketInfo> ticketInfo)
        {
            string txt = string.Empty;
            if (ticketInfo.Count > 0)
            {
                ticketInfo.ForEach(x =>
                {
                    txt += x.Description + " X " + x.Quantity + ", ";
                });
                txt = txt.Trim().TrimEnd(',');
            }
            return txt;
        }

        public string SendNetMail(string mailFrom, string mailTo, string copyTo, string htmlBody, string subject, string sFile)
        {
            string retunrmsg = "";
            try
            {
                // copyTo = "Ticket@reelcinemas.ae,ASrivastava.aro@emaar.ae";

                string MailServer = ConfigurationManager.AppSettings["MailServer"].ToString();
                string MailComponent = ConfigurationManager.AppSettings["MailComponent"].ToString();
                string SmtpMailId = ConfigurationManager.AppSettings["SmtpMailId"].ToString();
                string SmtpMailPwd = ConfigurationManager.AppSettings["SmtpMailPwd"].ToString();
                string SmtpMailPort = ConfigurationManager.AppSettings["SmtpMailPort"].ToString();
                mailFrom = SmtpMailId;

                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress(mailFrom, "Reel Cinemas");

                System.Net.Mail.MailAddress bcc;
                smtpClient.Host = MailServer;
                smtpClient.Port = int.Parse(SmtpMailPort);
             
                message.From = fromAddress;
                message.To.Add(mailTo);


                if (copyTo.Trim() != "" && copyTo.Trim() != null)
                {
                    string[] list_of_email = copyTo.Split(',');
                    if (list_of_email[0] != "")
                    {
                        bcc = new System.Net.Mail.MailAddress(list_of_email[0].ToString());
                        message.Bcc.Add(bcc);

                    }
                    for (int y = 1; y < list_of_email.Length; y++)
                    {
                        if (Convert.ToString(list_of_email[y]) != "")
                        {
                            bcc = new System.Net.Mail.MailAddress(list_of_email[y].ToString());
                            message.Bcc.Add(bcc);
                        }
                    }
                }
                if (sFile != "" && sFile != null)
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(sFile);
                    message.Attachments.Add(attachment);
                }
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = htmlBody;
                smtpClient.Send(message);
                message.Dispose();
                retunrmsg = "success";
            }
            catch (Exception exMail)
            {
                retunrmsg = exMail.Message;
            }

            return retunrmsg;
        }
    }
}