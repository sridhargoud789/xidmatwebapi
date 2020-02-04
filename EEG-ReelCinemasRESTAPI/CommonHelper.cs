using EEG_ReelCinemasRESTAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using ReelDvo;
using ReelDVO;
using ReelDAO;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using Passbook.Generator;
using Passbook.Generator.Fields;
using System.Text;

namespace EEG_ReelCinemasRESTAPI
{
    public class CommonHelper
    {
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
        public string CinemaLocationPageUrl(string cinemaId)
        {
            string url = string.Empty;

            try
            {
                string cinemasJson = ReadJsonFile("Cinemas");
                if (!string.IsNullOrEmpty(cinemasJson))
                {
                    var oDataCinemas = JsonConvert.DeserializeObject<ODataCinemas>(cinemasJson);
                    List<Cinemas> cinemasList = oDataCinemas.Value.Where(c => c.ID == cinemaId).ToList();
                    if (cinemasList.Count > 0)
                    {
                        url = cinemasList[0].WebpageUrl;
                    }
                }
            }
            catch (Exception exception)
            {


            }
            return url;
        }
        public BookingSummary GetBookingSummary(Int64 bookingInfoId, string SeatList)
        {
            BookingSummary resp = new BookingSummary();
            try
            {
                DataTable dt = new DataTable();
                dt = new BookingInfoDao().GetBookingSummary(bookingInfoId);

                if (dt.Rows.Count > 0)
                {
                    string strResp = JsonConvert.SerializeObject(dt.Rows[0]);
                    dynamic d = JsonConvert.DeserializeObject(strResp);
                    string tJson = JsonConvert.SerializeObject(d.Table[0]);
                    resp = JsonConvert.DeserializeObject<BookingSummary>(tJson);
                    string foodDesc = dt.Rows[0]["foodDesc"].ToString();
                    if (!string.IsNullOrEmpty(foodDesc))
                    {
                        resp.FoodInfoList = ConvertToFoodInfoList(foodDesc);
                    }
                    string ticketDescription = dt.Rows[0]["ticketDescription"].ToString();
                    if (!string.IsNullOrEmpty(ticketDescription))
                    {
                        resp.TicketInfoList = ConvertToTicketInfoList(ticketDescription);
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[0]["BookingId"].ToString()))
                    {
                        try
                        {
                            resp.QRCodeUrl = ConfigurationManager.AppSettings["APIBaseURL"].ToString() + "/QRCode/" + dt.Rows[0]["BookingId"].ToString() + ".png";
                        }
                        catch (Exception ex) { }
                        try
                        {

                            resp.AddWalletUrl = AddWalletUrl(resp.BookingId, resp.CinemaName, resp.MovieName, Convert.ToDateTime(resp.Showtime), resp.ScreenName, resp.Experience,
                            dt.Rows[0]["ticketDescription"].ToString(), resp.MovieId, resp.Rating, Convert.ToDecimal(resp.TotalAmount), SeatList);
                        }
                        catch (Exception ex) { }
                        try
                        {
                            resp.AddCalendarUrl = AddCalendarUrl(resp.BookingId, resp.CinemaName, resp.MovieName, Convert.ToDateTime(resp.Showtime), resp.ScreenName, resp.Experience, dt.Rows[0]["ticketDescription"].ToString());

                        }
                        catch (Exception ex) { }
                        try
                        {
                            resp.LocationPageUrl = CinemaLocationPageUrl(resp.CinemaId);

                        }
                        catch (Exception ex) { }

                    }



                }
            }
            catch (Exception ex)
            {
                resp = null;
            }
            return resp;
        }

        public List<FoodInfo> ConvertToFoodInfoList(string foodDescription)
        {
            List<FoodInfo> foodInfoList = new List<FoodInfo>();
            try
            {

                if (!string.IsNullOrEmpty(foodDescription))
                {
                    foodDescription = foodDescription.TrimEnd('#');
                    if (!string.IsNullOrEmpty(foodDescription))
                    {
                        string[] items = foodDescription.Split('#');
                        for (int item = 0; item < items.Length; item++)
                        {
                            FoodInfo foodInfo = new FoodInfo();
                            string[] itemDetails = items[item].Split('&');
                            bool foodInfoDescription = false;
                            for (int itemDetail = 0; itemDetail < itemDetails.Length; itemDetail++)
                            {
                                string[] detail = itemDetails[itemDetail].Split('|');
                                if (itemDetail == 0)
                                {
                                    foodInfo.Name = detail[1];
                                    foodInfo.UnitAmount = Convert.ToDecimal(detail[2]);
                                    foodInfo.Quantity = detail.Length > 3 ? Convert.ToInt32(detail[3]) : 1;
                                    foodInfo.SeatInfo = (detail.Length > 4 ? Convert.ToString(detail[4]) : string.Empty);
                                }
                                else
                                {
                                    foodInfo.Description = foodInfo.Description + detail[1] + ",";
                                    foodInfoDescription = true;
                                }
                            }
                            foodInfo.Description = foodInfoDescription ? "(" + foodInfo.Description.TrimEnd(',') + ")" : string.Empty;
                            foodInfoList.Add(foodInfo);
                        }
                    }
                }
            }
            catch (Exception exception)
            {

            }
            return foodInfoList;
        }

        public void CreateLog(string userSessionId, string type, string applicationName, string funcationName, string customData,
            string requestType, string Description, DeviceDetails dd)
        {

            try
            {
                new LogDao().InsertLog(userSessionId, type, applicationName, funcationName, customData,
                            requestType, Description, "MOBILE", dd);
            }
            catch (Exception ex)
            {

            }

        }


        private List<TicketInfo> ConvertToTicketInfoList(string ticketDescription)
        {
            List<TicketInfo> ticketInfoList = new List<TicketInfo>();
            try
            {
                string[] areas = ticketDescription.Split('#');
                for (int area = 0; area < areas.Length; area++)
                {
                    string[] values = areas[area].Split('|');
                    TicketInfo ticketInfo = new TicketInfo();
                    ticketInfo.Description = values[0];
                    ticketInfo.SeatInfo = values[1];
                    ticketInfo.UnitAmount = Convert.ToDecimal(values[2]);
                    ticketInfo.Quantity = Convert.ToInt32(values[3]);
                    ticketInfoList.Add(ticketInfo);
                }
            }
            catch (Exception exception)
            {

            }
            return ticketInfoList;
        }

        public string GenerateNumber()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 11; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }


        public string AddCalendarUrl(string bookingId, string cinemaName, string movieName, DateTime showtime, string screenName, string experience, string ticketDescription)
        {
            string addCalendarUrl = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(bookingId))
                {


                    var icalStringbuilder = new StringBuilder();
                    icalStringbuilder.AppendLine("BEGIN:VCALENDAR");
                    icalStringbuilder.AppendLine("PRODID:-//MyTestProject//EN");
                    icalStringbuilder.AppendLine("VERSION:2.0");

                    icalStringbuilder.AppendLine("BEGIN:VTIMEZONE");
                    icalStringbuilder.AppendLine("TZID:Asia/Dubai");
                    icalStringbuilder.AppendLine("BEGIN:STANDARD");
                    icalStringbuilder.AppendLine("DTSTART:19321213T204552");
                    icalStringbuilder.AppendLine("TZOFFSETTO:+0400");
                    icalStringbuilder.AppendLine("TZOFFSETFROM:+0000");
                    icalStringbuilder.AppendLine("TZNAME:GST");
                    icalStringbuilder.AppendLine("END:STANDARD");
                    icalStringbuilder.AppendLine("END:VTIMEZONE");

                    icalStringbuilder.AppendLine("BEGIN:VEVENT");
                    icalStringbuilder.AppendLine("SUMMARY;LANGUAGE=en-us:" + bookingId + " - " + movieName);
                    icalStringbuilder.AppendLine("CLASS:PUBLIC");
                    icalStringbuilder.AppendLine("DESCRIPTION:Screen:" + screenName + " Experience:" + (ConvertToTicketInfoList(ticketDescription)) + " Ticket(s):" + string.Join(",", ConvertToTicketInfoList(ticketDescription).Select(l => l.SeatInfo).ToList()));

                    //icalStringbuilder.AppendLine(string.Format("CREATED:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                    //icalStringbuilder.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", showtime));
                    //icalStringbuilder.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", showtime.AddMinutes(30)));

                    icalStringbuilder.AppendLine("DTSTART;TZID=Asia/Dubai:" + showtime.ToString("yyyyMMddTHHmm00"));
                    icalStringbuilder.AppendLine("DTEND;TZID=Asia/Dubai:" + showtime.AddMinutes(120).ToString("yyyyMMddTHHmm00"));

                    icalStringbuilder.AppendLine("SEQUENCE:0");
                    icalStringbuilder.AppendLine("UID:" + Guid.NewGuid());
                    icalStringbuilder.AppendLine("LOCATION:" + cinemaName);
                    icalStringbuilder.AppendLine("END:VEVENT");
                    icalStringbuilder.AppendLine("END:VCALENDAR");
                    var bytes = Encoding.UTF8.GetBytes(icalStringbuilder.ToString());
                    System.IO.File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/ICS/") + bookingId + ".ics", bytes);
                    addCalendarUrl = ConfigurationManager.AppSettings["APIBaseURL"].ToString() + "/ICS/" + bookingId + ".ics";
                }
            }
            catch (Exception exception)
            {

            }
            return addCalendarUrl;
        }
        public string AddWalletUrl(string bookingId, string cinemaName, string movieName, DateTime showtime, string screenName, string experience, string ticketDescription, string movieId, string rating, Decimal totalAmount, string seatInfo)
        {

            string url = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(bookingId))
                {

                    string PassTypeIdentifier = ConfigurationManager.AppSettings["PassbookPassTypeIdentifier"].ToString();
                    string CertThumbprint = ConfigurationManager.AppSettings["PassbookCertThumbprint"].ToString();
                    string TeamIdentifier = ConfigurationManager.AppSettings["PassbookTeamIdentifier"].ToString();

                    PassGenerator generator = new PassGenerator();
                    EventPassGeneratorRequest request = new EventPassGeneratorRequest();

                    request.PassTypeIdentifier = PassTypeIdentifier;
                    request.CertThumbprint = CertThumbprint;
                    request.TeamIdentifier = TeamIdentifier;
                    request.CertLocation = System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine;
                    request.SerialNumber = GenerateNumber();
                    request.Description = "Reel Cinemas";
                    request.OrganizationName = "Reel Cinemas";
                    request.BackgroundColor = "rgb(255,255,255)";
                    request.ForegroundColor = "rgb(120,120,120)";
                    request.LabelColor = "rgb(5,5,5)";
                    request.BookingID = bookingId;
                    request.SeatInfo = seatInfo;
                    request.AddBarcode(BarcodeType.PKBarcodeFormatQR, request.BookingID, "iso-8859-1");
                    string moviePoster = ConfigurationManager.AppSettings["weburl"].ToString() + "/Content/images/Movies/" + movieId + ".jpg";

                    if (!(System.IO.File.Exists(moviePoster)))
                    {
                        moviePoster = HttpContext.Current.Server.MapPath("~/pkpass/poster.jpg");
                    }
                    request.Images.Add(PassbookImage.Thumbnail, System.IO.File.ReadAllBytes(moviePoster));
                    request.Images.Add(PassbookImage.ThumbnailRetina, System.IO.File.ReadAllBytes(moviePoster));

                    // override icon and icon retina
                    request.Images.Add(PassbookImage.Icon, System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/pkpass/icon.png")));
                    request.Images.Add(PassbookImage.IconRetina, System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/pkpass/icon@2x.png")));

                    //R request.Images.Add(PassbookImage.iconthreeX, System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/images/icon@3x.png")));

                    request.Images.Add(PassbookImage.Logo, System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/pkpass/logo.png")));
                    request.Images.Add(PassbookImage.LogoRetina, System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/pkpass/logo@2x.png")));

                    request.Images.Add(PassbookImage.Background, System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/pkpass/BG.png")));
                    request.Images.Add(PassbookImage.BackgroundRetina, System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/pkpass/BG@2X.png")));

                    request.CinemaName = cinemaName;
                    request.MovieName = movieName;

                    request.TicketInfo = "SEATS : " + string.Join(",", ConvertToTicketInfoList(ticketDescription).Select(l => l.SeatInfo).ToList());
                    request.Screenname = screenName;
                    request.Rating = rating;

                    request.ShowDate = showtime.ToString("ddd dd MMM yyyy");
                    request.Showtime = showtime.ToString("hh:mm tt");
                    request.Total = "AED " + totalAmount.ToString("0.00");
                    request.Experience = experience;

                    //string latlong = GetLatLong(dt.Rows[0]["cinemaid"].ToString());
                    //request.latitude = Convert.ToDouble(latlong.Split(',')[0]);
                    //request.longitude = Convert.ToDouble(latlong.Split(',')[1]);

                    //request.Relavenantdate = showtime.AddMinutes(-30).ToString("yyyyMMddTHHmm00");
                    request.Relavenantdate = showtime.AddMinutes(-30).ToString("yyyy-MM-ddTHH:mm:ss+04:00");

                    System.IO.File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/PassBookFiles/") + bookingId + ".pkpass", generator.Generate(request));
                    url = ConfigurationManager.AppSettings["APIBaseURL"].ToString() + "/PassBookFiles/" + bookingId + ".pkpass";



                }
            }
            catch (Exception exception)
            {

            }
            return url;
        }

        public string CompleteBooking(int bookingInfoId, string VistaBookingId, string VistaBookingNumber, string VistaTransNumber, string SessionId, DataRow r)
        {
            new BookingInfoDao().UpdateBookingDetails(bookingInfoId, VistaBookingId, VistaBookingNumber, VistaTransNumber, true, "Success");

            string countryCode = r["countryCode"].ToString().Replace("+", "");
            string Phone = r["phoneNo"].ToString().Replace("+", "").TrimStart(new Char[] { '0' });
            Phone = countryCode + Phone;
            string SeatList = r["seatInfo"].ToString();

            BookingSummary bookingSummaryDetails = new CommonHelper().GetBookingSummary(bookingInfoId, SeatList);
            bookingSummaryDetails.PhoneNo = Phone;

            try
            {
                new NotificationHelper().BookingQRcode(VistaBookingId.ToString());
            }
            catch (Exception ex)
            {
                string strReq = JsonConvert.SerializeObject(new { sessionId = SessionId, vistaBookingId = VistaBookingId, seatList = SeatList, phone = Phone });
                new CommonHelper().CreateLog("", "ERROR", "CommonHelper", "BookingQRcode", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            try
            {
                string smsStatus = new NotificationHelper().SendSMSNotification(SessionId, VistaBookingId.ToString(), SeatList, Phone);
            }
            catch (Exception ex)
            {
                string strReq = JsonConvert.SerializeObject(new { sessionId = SessionId, vistaBookingId = VistaBookingId, seatList = SeatList, phone = Phone });
                new CommonHelper().CreateLog("", "ERROR", "CommonHelper", "SendSMSNotification", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }

            try
            {

                string emailStatus = new NotificationHelper().SendNetMail(string.Empty, bookingSummaryDetails.EmailId, ConfigurationManager.AppSettings["MailBCC"].ToString(),
                    new NotificationHelper().BookingEmailHTML(bookingSummaryDetails), "Reel Cinemas Booking Confirmation", null);
            }
            catch (Exception ex)
            {
                string strReq = JsonConvert.SerializeObject(new { sessionId = SessionId, vistaBookingId = VistaBookingId, seatList = SeatList, emailid = bookingSummaryDetails.EmailId });
                new CommonHelper().CreateLog("", "ERROR", "CommonHelper", "SendSMSNotification", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return "";
        }
    }
}