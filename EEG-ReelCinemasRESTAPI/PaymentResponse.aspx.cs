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
using Passbook.Generator;
using Passbook.Generator.Fields;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using RestSharp;

namespace EEG_ReelCinemasRESTAPI
{
    public partial class PaymentResponse : System.Web.UI.Page
    {
        private DataTable ServiceSettings = null;
        private BaseAnalizer baseAnalizer = null;
        private Analizer analizer = null;
        MobileBookingDao oMobileBookingDao = null;
        private string VistaOptionalClientId = "";
        private string ReturnValue = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnAction_Click(object sender, EventArgs e)
        {
            try
            {

                string PaymentConfirmationURL = ConfigurationManager.AppSettings["PaymentConfirmationURL"].ToString();
                if (Request.Params.Count > 0)
                {
                    var req = Request.QueryString;
                    if (req["result"] != null)
                    {
                        string result = req["result"].ToString();
                        string paymentInfo = req["paymentInfo"].ToString();
                        string orderReference = req["orderReference"].ToString();
                        string orderId = req["orderId"].ToString();

                        CompleteOrderByCardReq oReq = new CompleteOrderByCardReq();
                        DataTable dt = new DataTable();
                        dt = new BookingInfoDao().GetBookingDetailsByOrderId(Int64.Parse(orderId));
                        decimal dTotalAmount = 0;
                        string OptionalClientId = "";

                        if (dt.Rows.Count > 0)
                        {
                            var r = dt.Rows[0];
                            OptionalClientId = r["OptionalClientId"].ToString();

                            oReq.UserSessionId = r["userSessionId"].ToString();
                            oReq.PerformPayment = true;
                            oReq.CustomerEmail = r["emailId"].ToString();
                            oReq.CustomerPhone = r["phoneNo"].ToString();
                            oReq.CustomerName = r["lastName"].ToString() == "" ? r["firstName"].ToString() : r["lastName"].ToString() + " " + r["firstName"].ToString();
                            oReq.OptionalClientId = OptionalClientId;
                            oReq.BookingNotes = "";
                            oReq.SessionId = r["SessionId"].ToString();
                            oReq.CinemaId = r["cinemaId"].ToString();

                            string strResponse = r["Request"].ToString();
                            Int64 bookingInfoId = Convert.ToInt64(r["bookingInfoId"].ToString());
                            string NPURL = ConfigurationManager.AppSettings["NoonPaymentOrderURL"].ToString();
                            bool isException = false;
                            string strExceptionMsg = string.Empty;
                            long externalRequestLogId = 0;
                            oMobileBookingDao = new MobileBookingDao();
                            ServiceSettings = oMobileBookingDao.GetServiceSettings();

                            if (result == "SUCCESS")
                            {
                                if (r["tokenIdentifier"].ToString() == "")
                                {
                                    InitiateCardPayment oCCReq = new InitiateCardPayment();
                                    oCCReq = JsonConvert.DeserializeObject<InitiateCardPayment>(strResponse);
                                    oReq.PaymentInfo = new Models.CompleteOrderByCard.PaymentInfo()
                                    {
                                        CardNumber = oCCReq.paymentData.data.numberPlain,
                                        CardType = oCCReq.paymentData.type,
                                        CardExpiryMonth = oCCReq.paymentData.data.expiryMonth,
                                        CardExpiryYear = oCCReq.paymentData.data.expiryYear,
                                        PaymentValueCents = Convert.ToInt32(Convert.ToDecimal(oCCReq.order.amount) * 100),
                                        PaymentSystemId = "",
                                        CardCVC = oCCReq.paymentData.data.cvv,
                                        tokenIdentifier = ""
                                    };
                                    dTotalAmount = Convert.ToDecimal(oCCReq.order.amount);
                                }
                                else
                                {
                                    InitiatePaymentByToken oTPReq = new InitiatePaymentByToken();
                                    oTPReq = JsonConvert.DeserializeObject<InitiatePaymentByToken>(strResponse);
                                    oReq.PaymentInfo = new Models.CompleteOrderByCard.PaymentInfo()
                                    {
                                        CardNumber = string.Empty,
                                        CardType = oTPReq.paymentData.type,
                                        CardExpiryMonth = string.Empty,
                                        CardExpiryYear = string.Empty,
                                        PaymentValueCents = Convert.ToInt32(Convert.ToDecimal(oTPReq.order.amount) * 100),
                                        PaymentSystemId = "",
                                        CardCVC = oTPReq.paymentData.data.cvv,
                                        tokenIdentifier = oTPReq.paymentData.data.tokenIdentifier
                                    };
                                    dTotalAmount = Convert.ToDecimal(oTPReq.order.amount);
                                }

                                #region Capture Amount

                                NoonPaymentCaptureReq NPCReq = new NoonPaymentCaptureReq();

                                NoonPaymentCaptureResp oNPCResp = new NoonPaymentCaptureResp();


                                try
                                {

                                    NPCReq.apiOperation = "CAPTURE";
                                    NPCReq.order = new NPCReqOrder()
                                    {
                                        id = Convert.ToInt64(orderId)
                                    };
                                    NPCReq.transaction = new NPCReqTransaction()
                                    {
                                        amount = dTotalAmount.ToString(),
                                        currency = "AED"
                                    };



                                    //Wrapper Service Request Log
                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        analizer = new Analizer();
                                        externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(NPCReq), oReq.DeviceType, oReq.UserSessionId, oReq.SessionId, oReq.CinemaId, "CaptureNoonPayment", "restTicketing.CaptureNoonPayment", string.Empty);
                                    }

                                    string strCaptureResp = WebRequestForMobile.NoonPaymentWebRequest(NPURL, JsonConvert.SerializeObject(NPCReq), out isException, out strExceptionMsg);

                                    if (!isException)
                                    {
                                        dynamic dNPCResp = JsonConvert.DeserializeObject(strCaptureResp);
                                        if (dNPCResp.resultCode.ToString() == "0")
                                        {
                                            oNPCResp = JsonConvert.DeserializeObject<NoonPaymentCaptureResp>(strCaptureResp);
                                        }
                                        else
                                        {
                                            oNPCResp.resultCode = int.Parse(dNPCResp.resultCode.ToString());
                                            oNPCResp.message = dNPCResp.message.ToString();
                                        }
                                    }
                                    else
                                    {
                                        new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "NoonPaymentWebRequest", JsonConvert.SerializeObject(NPCReq), "REQUEST", strExceptionMsg, null);
                                        oNPCResp.resultCode = 9999;
                                        oNPCResp.message = strExceptionMsg;
                                    }

                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oNPCResp), oNPCResp.resultCode.ToString());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "CaptureNoonPayment", JsonConvert.SerializeObject(oNPCResp), "REQUEST", JsonConvert.SerializeObject(ex), null);
                                }
                                #endregion
                                #region Get Noon Payment Order Details
                                GetNoonPaymentOrderDetails oNPODResp = new GetNoonPaymentOrderDetails();
                                try
                                {
                                    NPURL = ConfigurationManager.AppSettings["NoonPaymentOrderURL"].ToString() + orderId;
                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        analizer = new Analizer();
                                        externalRequestLogId = analizer.InsertServiceLog(0, "request", NPURL, oReq.DeviceType, oReq.UserSessionId, oReq.SessionId, oReq.CinemaId, "GetNoonPaymentOrderDetails", "restTicketing.GetNoonPaymentOrderDetails", string.Empty);
                                    }
                                    string strODResp = WebRequestForMobile.WebRequestGet(NPURL, out isException, out strExceptionMsg);

                                    if (!isException)
                                    {
                                        dynamic dNPGetODResp = JsonConvert.DeserializeObject(strODResp);

                                        if (dNPGetODResp.resultCode.ToString() == "0")
                                        {
                                            oNPODResp = JsonConvert.DeserializeObject<GetNoonPaymentOrderDetails>(strODResp);
                                        }
                                        else
                                        {
                                            oNPODResp.resultCode = int.Parse(dNPGetODResp.resultCode.ToString());
                                            oNPODResp.message = dNPGetODResp.message.ToString();
                                        }
                                    }
                                    else
                                    {
                                        new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "GetNoonPaymentOrderDetails", JsonConvert.SerializeObject(oNPODResp), "REQUEST", strExceptionMsg, null);
                                        oNPODResp.resultCode = 9999;
                                        oNPODResp.message = strExceptionMsg;
                                    }

                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oNPODResp), oNPODResp.resultCode.ToString());
                                    }
                                    string authCode = string.Empty;
                                    string transactionStatus = string.Empty;
                                    float iTransacAmount = 0;
                                    foreach (var t in oNPODResp.result.transactions)
                                    {
                                        if (t.type == "AUTHORIZATION")
                                        {
                                            authCode = t.authorizationCode;
                                            transactionStatus = t.status;
                                            iTransacAmount = t.amount;
                                        }
                                    }

                                    new BookingInfoDao().InsertNoonPaymentResponse(new InsertNoonPaymentResp()
                                    {
                                        bookingInfoId = bookingInfoId,
                                        resultCode = oNPODResp.resultCode,
                                        message = oNPODResp.message,
                                        requestReference = oNPODResp.requestReference,
                                        authorizationCode = authCode,
                                        transactionStatus = transactionStatus,
                                        reasonCode = "100",
                                        amount = iTransacAmount,
                                        orderId = Convert.ToInt64(orderId),
                                        orderStatus = oNPODResp.result.order.status,
                                        tokenIdentifier = oNPODResp.result.paymentDetails.tokenIdentifier,
                                        paymentMechanism = oNPODResp.result.paymentDetails.paymentMechanism,
                                        paymentInfo = oNPODResp.result.paymentDetails.paymentInfo,
                                        brand = oNPODResp.result.paymentDetails.brand,
                                        cardType = oNPODResp.result.paymentDetails.cardType,
                                        expiryMonth = Convert.ToInt16(oNPODResp.result.paymentDetails.expiryMonth),
                                        expiryYear = Convert.ToInt16(oNPODResp.result.paymentDetails.expiryYear),
                                        App_Source = "MOBILE"
                                    });


                                }
                                catch (Exception ex)
                                {
                                    new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "GetNoonPaymentOrderDetails", JsonConvert.SerializeObject(oNPODResp), "REQUEST", JsonConvert.SerializeObject(ex), null);
                                }
                                #endregion

                                if (oNPCResp.resultCode == 0)
                                {
                                    oReq.PaymentInfo.CardNumber = r["EncryptedCardNo"].ToString();
                                    oReq.PerformPayment = false;
                                    oReq.BookingNotes = "N";
                                    oReq.PaymentInfo.PaymentSystemId = "-";
                                    string resp = CompleteOrderByCCPayment(oReq);

                                    if (resp != "")
                                    {

                                        dynamic d = JsonConvert.DeserializeObject(resp);
                                        if (d.Result.ToString() == "13" || d.Result.ToString() == "0" || d.Result.ToString() == "25")
                                        {

                                            string strAPIResult = "13";
                                            new BookingInfoDao().UpdateBookingDetails(bookingInfoId, d.VistaBookingId.ToString(), d.VistaBookingNumber.ToString(), d.VistaTransNumber.ToString(), true, "Success");

                                            string countryCode = r["countryCode"].ToString().Replace("+", "");
                                            string Phone = r["phoneNo"].ToString().Replace("+", "").TrimStart(new Char[] { '0' });
                                            Phone = countryCode + Phone;
                                            string SeatList = r["seatInfo"].ToString();

                                            BookingSummary bookingSummaryDetails = new CommonHelper().GetBookingSummary(bookingInfoId, SeatList);
                                            bookingSummaryDetails.PhoneNo = Phone;

                                            try
                                            {
                                                new NotificationHelper().BookingQRcode(d.VistaBookingId.ToString());
                                            }
                                            catch (Exception ex)
                                            {
                                                string strReq = JsonConvert.SerializeObject(new { sessionId = oReq.SessionId, vistaBookingId = d.VistaBookingId, seatList = SeatList, phone = Phone });
                                                new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "BookingQRcode", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
                                            }
                                            try
                                            {
                                                string smsStatus = new NotificationHelper().SendSMSNotification(oReq.SessionId, d.VistaBookingId.ToString(), SeatList, Phone);
                                            }
                                            catch (Exception ex)
                                            {
                                                string strReq = JsonConvert.SerializeObject(new { sessionId = oReq.SessionId, vistaBookingId = d.VistaBookingId, seatList = SeatList, phone = Phone });
                                                new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "SendSMSNotification", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
                                            }

                                            try
                                            {

                                                string emailStatus = new NotificationHelper().SendNetMail(string.Empty, bookingSummaryDetails.EmailId, ConfigurationManager.AppSettings["MailBCC"].ToString(),
                                                    new NotificationHelper().BookingEmailHTML(bookingSummaryDetails), "Reel Cinemas Booking Confirmation", null);
                                            }
                                            catch (Exception ex)
                                            {
                                                string strReq = JsonConvert.SerializeObject(new { sessionId = oReq.SessionId, vistaBookingId = d.VistaBookingId, seatList = SeatList, emailid = bookingSummaryDetails.EmailId });
                                                new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "SendSMSNotification", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
                                            }

                                            string msg = "Success";
                                            string strResult = "" + strAPIResult + "|" + msg + "|" + d.VistaBookingId.ToString() + "|" + d.VistaBookingNumber.ToString() + "|" + d.VistaTransNumber.ToString() + "";
                                            string strEncryptedResult = new AuthenticationHelper().AesEncrypt(strResult);
                                            if (oNPCResp.resultCode.ToString() == "0")
                                            {
                                                Response.Redirect("" + PaymentConfirmationURL + "?Result=" + strEncryptedResult + "", true);
                                            }
                                        }
                                        else
                                        {
                                            string msg = "Failed";
                                            string strResult = "" + d.Result.ToString() + "|" + msg;
                                            string strEncryptedResult = new AuthenticationHelper().AesEncrypt(strResult);
                                            if (oNPCResp.resultCode.ToString() == "0")
                                            {
                                                Response.Redirect("" + PaymentConfirmationURL + "?Result=" + strEncryptedResult + "", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string msg = "Failed";
                                        string strResult = "1|" + msg;
                                        string strEncryptedResult = new AuthenticationHelper().AesEncrypt(strResult);
                                        Response.Redirect("" + PaymentConfirmationURL + "?Result=" + strEncryptedResult + "", true);
                                    }
                                }
                                else
                                {
                                    new BookingInfoDao().UpdateBookingDetails(bookingInfoId, "", "", "", false, oNPCResp.message);
                                    string strResult = "" + oNPCResp.resultCode.ToString() + "|" + oNPCResp.message;
                                    string strEncryptedResult = new AuthenticationHelper().AesEncrypt(strResult);
                                    Response.Redirect("" + PaymentConfirmationURL + "?Result=" + strEncryptedResult + "", true);
                                }
                            }
                            else
                            {
                                #region Get Noon Payment Order Details
                                GetNoonPaymentOrderDetails oNPODResp = new GetNoonPaymentOrderDetails();
                                try
                                {
                                    NPURL = ConfigurationManager.AppSettings["NoonPaymentOrderURL"].ToString() + orderId;
                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        analizer = new Analizer();
                                        externalRequestLogId = analizer.InsertServiceLog(0, "request", NPURL, oReq.DeviceType, oReq.UserSessionId, oReq.SessionId, oReq.CinemaId, "GetNoonPaymentOrderDetails", "restTicketing.GetNoonPaymentOrderDetails", string.Empty);
                                    }
                                    string strODResp = WebRequestForMobile.WebRequestGet(NPURL, out isException, out strExceptionMsg);

                                    if (!isException)
                                    {
                                        dynamic dNPGetODResp = JsonConvert.DeserializeObject(strODResp);

                                        if (dNPGetODResp.resultCode.ToString() == "0")
                                        {
                                            oNPODResp = JsonConvert.DeserializeObject<GetNoonPaymentOrderDetails>(strODResp);
                                        }
                                        else
                                        {
                                            oNPODResp.resultCode = int.Parse(dNPGetODResp.resultCode.ToString());
                                            oNPODResp.message = dNPGetODResp.message.ToString();
                                        }
                                    }
                                    else
                                    {
                                        new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "GetNoonPaymentOrderDetails", JsonConvert.SerializeObject(oNPODResp), "REQUEST", strExceptionMsg, null);
                                        oNPODResp.resultCode = 9999;
                                        oNPODResp.message = strExceptionMsg;
                                    }

                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oNPODResp), oNPODResp.resultCode.ToString());
                                    }
                                    string authCode = string.Empty;
                                    string transactionStatus = oNPODResp.result.order.errorMessage;
                                    float iTransacAmount = oNPODResp.result.order.amount;


                                    new BookingInfoDao().InsertNoonPaymentResponse(new InsertNoonPaymentResp()
                                    {
                                        bookingInfoId = bookingInfoId,
                                        resultCode = oNPODResp.resultCode,
                                        message = oNPODResp.message,
                                        requestReference = oNPODResp.requestReference,
                                        authorizationCode = authCode,
                                        transactionStatus = transactionStatus,
                                        reasonCode = "100",
                                        amount = iTransacAmount,
                                        orderId = Convert.ToInt64(orderId),
                                        orderStatus = oNPODResp.result.order.status,
                                        tokenIdentifier = oNPODResp.result.paymentDetails.tokenIdentifier,
                                        paymentMechanism = oNPODResp.result.paymentDetails.paymentMechanism,
                                        paymentInfo = oNPODResp.result.paymentDetails.paymentInfo,
                                        brand = oNPODResp.result.paymentDetails.brand,
                                        cardType = oNPODResp.result.paymentDetails.cardType,
                                        expiryMonth = Convert.ToInt16(oNPODResp.result.paymentDetails.expiryMonth),
                                        expiryYear = Convert.ToInt16(oNPODResp.result.paymentDetails.expiryYear),
                                        App_Source = "MOBILE"
                                    });

                                    new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "UNSUCCESS", JsonConvert.SerializeObject(Request.QueryString), "REQUEST", "", null);
                                    string msg = "Failed";
                                    string strResult = "1|" + transactionStatus;
                                    string strEncryptedResult = new AuthenticationHelper().AesEncrypt(strResult);
                                    Response.Redirect("" + PaymentConfirmationURL + "?Result=" + strEncryptedResult + "", true);
                                }
                                catch (Exception ex)
                                {
                                    new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "GetNoonPaymentOrderDetails", JsonConvert.SerializeObject(oNPODResp), "REQUEST", JsonConvert.SerializeObject(ex), null);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "UNSUCCESS", JsonConvert.SerializeObject(Request.QueryString), "REQUEST", "", null);
                            string msg = "Failed";
                            string strResult = "1|" + msg;
                            string strEncryptedResult = new AuthenticationHelper().AesEncrypt(strResult);
                            Response.Redirect("" + PaymentConfirmationURL + "?Result=" + strEncryptedResult + "", true);
                        }
                    }
                    else
                    {
                        new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "UNSUCCESS", JsonConvert.SerializeObject(Request.QueryString), "REQUEST", "", null);
                        string msg = "Failed";
                        string strResult = "1|" + msg;
                        string strEncryptedResult = new AuthenticationHelper().AesEncrypt(strResult);
                        Response.Redirect("" + PaymentConfirmationURL + "?Result=" + strEncryptedResult + "", true);
                    }
                }
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "PaymentResponse", "Page_Load", JsonConvert.SerializeObject(Request.QueryString), "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
        }
        private async Task<NsGetOrder.GetOrderResp> GetOrderInfo(GetOrderReq oGetOrderReq)
        {
            var reqHeader = Request.Headers.GetValues("connectapitoken");
            VISTAResponse vistaResponse = null;
            NsGetOrder.GetOrderResp result = null;
            string url = "";
            long originalRequestLogId = 0;
            long externalRequestLogId = 0;

            oMobileBookingDao = new MobileBookingDao();
            ServiceSettings = oMobileBookingDao.GetServiceSettings();

            //Wrapper Service Request Log
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                analizer = new Analizer();
                originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetOrderReq), null, null, null, oGetOrderReq.CinemaId, "GetOrder", "GetOrder", string.Empty);
            }

            //Ext Service Request Log
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                analizer = new Analizer();
                externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetOrderReq), null, null, null, oGetOrderReq.CinemaId, "GetOrder", "restTicketing.order", string.Empty);
            }

            #region Vista API Call
            string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
            url = RestTicketingUrl + "/order";
            vistaResponse = await WebRequestForMobile.CreateWebPostRequest<NsGetOrder.GetOrderResp>(url, reqHeader.ToList()[0].ToString(), oGetOrderReq, "GetOrder", null, null);
            result = (NsGetOrder.GetOrderResp)vistaResponse.Result;
            #endregion

            //Ext Service Response Log
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                if (vistaResponse.Response != null)
                {
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(result), vistaResponse.Response == null ? "" : vistaResponse.Response.StatusCode.ToString());
                }
                else
                {
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(result), "");
                }
            }

            //Wrapper Service Response Log
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                if (vistaResponse.Response != null)
                {
                    analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(result), vistaResponse.Response.StatusCode.ToString());
                }
                else
                {
                    analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(result), "");
                }
            }
            return result;
        }
        private string CompleteOrderByCCPayment(CompleteOrderByCardReq oCompleteOrderByCardReq)
        {
            try
            {


                string strOutPut = string.Empty;

                var reqHeader = Request.Headers.GetValues("connectapitoken");

                string apiToken = ConfigurationManager.AppSettings["AndriodAPIToken"].ToString();

                if (oCompleteOrderByCardReq.OptionalClientId == ConfigurationManager.AppSettings["IOSOptClientID"].ToString())
                {
                    apiToken = ConfigurationManager.AppSettings["IOSAPIToken"].ToString();
                }

                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = 0;
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oCompleteOrderByCardReq), oCompleteOrderByCardReq.DeviceType, oCompleteOrderByCardReq.UserSessionId, oCompleteOrderByCardReq.SessionId, oCompleteOrderByCardReq.CinemaId, "CompleteOrderByCCPayment", "CompleteOrderByCCPayment", string.Empty);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oCompleteOrderByCardReq), oCompleteOrderByCardReq.DeviceType, oCompleteOrderByCardReq.UserSessionId, oCompleteOrderByCardReq.SessionId, oCompleteOrderByCardReq.CinemaId, "CompleteOrderByCCPayment", "restTicketing.CompleteOrderByCCPayment", string.Empty);
                }


                #region Vista API Call
                string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
                url = RestTicketingUrl + "/order/payment";

                strOutPut = WebRequestForMobile.CreateWPR(url, apiToken, JsonConvert.SerializeObject(oCompleteOrderByCardReq), "CompleteOrderByCard", oCompleteOrderByCardReq.DeviceType, oCompleteOrderByCardReq.UserSessionId);

                #endregion

                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (!string.IsNullOrEmpty(strOutPut))
                    {
                        dynamic d = JsonConvert.DeserializeObject(strOutPut);
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(strOutPut), d.Result.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", strOutPut, "");
                    }
                }

                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (!string.IsNullOrEmpty(strOutPut))
                    {
                        dynamic d = JsonConvert.DeserializeObject(strOutPut);
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(strOutPut), d.Result.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", strOutPut, "");
                    }
                }
                return strOutPut;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog(oCompleteOrderByCardReq.UserSessionId, "ERROR", "PaymentResponse", "CompleteOrderByCCPayment", JsonConvert.SerializeObject(oCompleteOrderByCardReq), "REQUEST", JsonConvert.SerializeObject(ex), null);
                return "";
            }
        }


    }
}