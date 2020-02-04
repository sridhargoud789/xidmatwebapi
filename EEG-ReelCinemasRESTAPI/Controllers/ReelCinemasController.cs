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
using EEG_ReelCinemasRESTAPI.UByEmaar;

namespace EEG_ReelCinemasRESTAPI.Controllers
{
    public class ReelCinemasController : ApiController
    {
        private DataTable ServiceSettings = null;
        private BaseAnalizer baseAnalizer = null;
        private Analizer analizer = null;
        MobileBookingDao oMobileBookingDao = null;
        private string VistaOptionalClientId = "";
        private string ReturnValue = string.Empty;

        #region Public Function(s)

        #region User Management Function(s)
        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/UserRegistration")]
        public async Task<object> UserRegistration([FromBody] UserRegistrationReq oReq)
        {
            try
            {


                var reqHeader = Request.Headers.GetValues("connectapitoken");
                UserRegistrationResp oResp = null;

                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), oReq.DeviceType, null, null, null, "UserRegistration", "UserRegistration", string.Empty, oReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), oReq.DeviceType, null, null, null, "UserRegistration", "restData.UserRegistration", string.Empty, oReq.DeviceDetails);
                }
                #region Influx API Call
                string RestdataUrl = ConfigurationManager.AppSettings["InfluxBaseURL"].ToString();
                url = RestdataUrl + "/UserAppAPI/UserRegistration";
                string response = WebRequestForMobile.WebServicesDoPost(url, JsonConvert.SerializeObject(oReq));
                var d = JsonConvert.DeserializeObject<CommonResp>(response);
                oResp = new UserRegistrationResp();
                if (d.statusCode == 1)
                {
                    oResp = JsonConvert.DeserializeObject<UserRegistrationResp>(response);
                    #endregion
                    if (oReq.UbyEmaarRegister)
                    {
                        try
                        {
                            if (oResp.statusMessage == "OK")
                            {
                                MemberBl oMemberBl = new MemberBl();
                                EnrollUByEmaarMember oEnrollUByEmaarMember = new EnrollUByEmaarMember();
                                oEnrollUByEmaarMember.DeviceType = oReq.DeviceType;
                                oEnrollUByEmaarMember.Email = new AuthenticationHelper().InfluxDecryptStringAES(oReq.EmailId);
                                oEnrollUByEmaarMember.MobileNo = oReq.CountryCode + oReq.PhoneNo;
                                oEnrollUByEmaarMember.MemberId = oReq.CountryCode + oReq.PhoneNo;

                                EnrollMemberReturn oEnrollMemberReturn = oMemberBl.EnrollMemberWithMobilePhoneNumber(oEnrollUByEmaarMember);

                                if (oEnrollMemberReturn.ReturnCode == 0)
                                {
                                    MemberContactBl oMemberContactBl = new MemberContactBl();
                                    SaveMemberEmailAddressReturn oSaveMemberEmailAddressReturn = oMemberContactBl.SaveMemberEmailAddress(oEnrollUByEmaarMember);
                                    if (oSaveMemberEmailAddressReturn.ReturnCode == 0)
                                    {
                                        MemberSecurityBl oMemberSecurityBl = new MemberSecurityBl();
                                        string password = new AuthenticationHelper().InfluxDecryptStringAES(oReq.Password);
                                        SetPasswordReturn oSetPasswordReturn = oMemberSecurityBl.SetPasswordUserReg(oEnrollUByEmaarMember, password);

                                        if (oSetPasswordReturn.ReturnCode == 0)
                                        {

                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            new CommonHelper().CreateLog("", "ERROR", "UserRegistrationController", "EnrollUByEmaarMember", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);

                        }
                    }
                }
                else
                {
                    oResp.statusCode = d.statusCode;
                    oResp.statusMessage = d.statusMessage;
                }

                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }

                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }
                return oResp;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "UserRegistrationController", "UserRegistration", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/UserLogin")]
        public async Task<object> UserLogin([FromBody] UserLoginReq oReq)
        {
            try
            {


                var reqHeader = Request.Headers.GetValues("connectapitoken");
                UserLoginResp oResp = null;

                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), "", null, null, null, "UserLogin", "UserLogin", string.Empty, oReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), "", null, null, null, "UserLogin", "restData.UserLogin", string.Empty, oReq.DeviceDetails);
                }

                #region Influx API Call
                string RestdataUrl = ConfigurationManager.AppSettings["InfluxBaseURL"].ToString();
                url = RestdataUrl + "/UserAppAPI/UserLogin";
                if (!string.IsNullOrEmpty(oReq.SocialMediaType))
                {
                    oReq.Password = "EDRpvImVqngM8AV5pXrL4g==";
                }
                string response = WebRequestForMobile.WebServicesDoPost(url, JsonConvert.SerializeObject(oReq));

                var d = JsonConvert.DeserializeObject<CommonResp>(response);
                oResp = new UserLoginResp();

                if (d.statusCode == 1)
                {
                    oResp = JsonConvert.DeserializeObject<UserLoginResp>(response);
                }
                else
                {
                    oResp.statusCode = d.statusCode;
                    oResp.statusMessage = d.statusMessage;
                }
                #endregion

                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }

                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }
                return oResp;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "UserLoginController", "UserLogin", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetUserDetails")]
        public async Task<object> GetUserDetails([FromBody] GetUserDetailsReq oReq)
        {
            try
            {


                var reqHeader = Request.Headers.GetValues("connectapitoken");
                GetUserDetailsResp oResp = null;

                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), "", null, null, null, "GetUserDetails", "GetUserDetails", string.Empty, oReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), "", null, null, null, "GetUserDetails", "restData.GetUserDetails", string.Empty, oReq.DeviceDetails);
                }

                #region Influx API Call
                string RestdataUrl = ConfigurationManager.AppSettings["InfluxBaseURL"].ToString();
                url = RestdataUrl + "/UserAppAPI/GetUserDetails";
                string response = WebRequestForMobile.WebServicesDoPost(url, JsonConvert.SerializeObject(oReq));

                oResp = JsonConvert.DeserializeObject<GetUserDetailsResp>(response);
                #endregion

                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }

                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }
                return oResp;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetUserDetailsController", "GetUserDetails", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/UpdateUser")]
        public async Task<object> UpdateUser([FromBody] UpdateUserReq oReq)
        {
            try
            {
            
                var reqHeader = Request.Headers.GetValues("connectapitoken");
                UserResp oResp = null;

                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), "", null, null, null, "UpdateUser", "UpdateUser", string.Empty, oReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), "", null, null, null, "UpdateUser", "restData.UpdateUser", string.Empty, oReq.DeviceDetails);
                }

                #region Influx API Call
                string RestdataUrl = ConfigurationManager.AppSettings["InfluxBaseURL"].ToString();
                url = RestdataUrl + "/UserAppAPI/UpdateUser";
                string response = WebRequestForMobile.WebServicesDoPost(url, JsonConvert.SerializeObject(oReq));

                oResp = JsonConvert.DeserializeObject<UserResp>(response);
                #endregion

                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }

                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }
                return oResp;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "UpdateUserController", "UpdateUser", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/ForgotPassword")]
        public async Task<object> ForgotPassword([FromBody] ForgotPasswordReq oReq)
        {
            try
            {


                var reqHeader = Request.Headers.GetValues("connectapitoken");
                UserResp oResp = null;

                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), "", null, null, null, "ForgotPassword", "ForgotPassword", string.Empty, oReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), "", null, null, null, "ForgotPassword", "restData.ForgotPassword", string.Empty, oReq.DeviceDetails);
                }

                #region Influx API Call
                string RestdataUrl = ConfigurationManager.AppSettings["InfluxBaseURL"].ToString();
                url = RestdataUrl + "/UserAppAPI/ForgotPassword";
                string response = WebRequestForMobile.WebServicesDoPost(url, JsonConvert.SerializeObject(oReq));

                oResp = JsonConvert.DeserializeObject<UserResp>(response);
                #endregion

                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }

                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }
                return oResp;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "ForgotPasswordController", "ForgotPassword", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                return null;
            }
        }
        #endregion

        #region Noon Payments Functions(s)
        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/InitiateNoonPayment")]
        public async Task<object> InitiateNoonPayment([FromBody]  InitiateCardPaymentReq oReq)
        {

            try
            {


                var reqHeader = Request.Headers.GetValues("connectapitoken");


                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                int iPaymentLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log

                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), oReq.DeviceType, oReq.UserSessionId, oReq.SessionId, oReq.CinemaId, "InitiateNoonPayment", "InitiateNoonPayment", string.Empty, oReq.DeviceDetails);

                }
                BookingSummary bookingSummaryDetails = new CommonHelper().GetBookingSummary(oReq.bookingInfoId, "");


                string NPURL = ConfigurationManager.AppSettings["NoonPaymentOrderURL"].ToString();
                bool isException = false;
                string strExceptionMsg = string.Empty;
                NoonPaymentResp NPResp = new NoonPaymentResp();
                string strResp = string.Empty;

                decimal dAmount = bookingSummaryDetails.TotalAmount;
                string strAmount = dAmount.ToString("0.00");
                string CustomerEmail = new AuthenticationHelper().AesDecrypt(oReq.CustomerEmail).ToString();
                string CustomerPhone = new AuthenticationHelper().AesDecrypt(oReq.CustomerPhone).ToString();
                string CustomerName = new AuthenticationHelper().AesDecrypt(oReq.CustomerName).ToString();
                DataTable dtGetPT = new DataTable();
                dtGetPT = new BookingInfoDao().GetPaymentTypes(oReq.CinemaId);
                string strNoonPaymentCategory = ConfigurationManager.AppSettings["NoonPaymentCategory"].ToString();
                if (dtGetPT.Rows.Count > 0)
                {
                    dtGetPT.DefaultView.RowFilter = "name='CreditCard'";
                    dtGetPT = dtGetPT.DefaultView.ToTable();
                    strNoonPaymentCategory = dtGetPT.Rows[0]["category"].ToString();
                }

                if (string.IsNullOrEmpty(oReq.tokenIdentifier))
                {
                    InitiateCardPayment oCCReq = new InitiateCardPayment();

                    try
                    {
                        oCCReq.apiOperation = "INITIATE";
                        oCCReq.order = new InitiateCardPaymentOrder()
                        {
                            name = "Payment for reel",
                            amount = strAmount,
                            currency = "AED",
                            channel = "mobile",
                            category = strNoonPaymentCategory,
                            description = "Payment for ticket purchase"
                        };
                        oCCReq.configuration = new InitiateCardPaymentConfiguration()
                        {

                            returnUrl = ConfigurationManager.AppSettings["PaymentResponseURL"].ToString(),
                            locale = "en",
                            tokenizeCc = true,
                            initiationValidity = DateTime.Now.AddDays(5).ToShortDateString(),
                            paymentAction = "AUTHORIZE"
                        };
                        oCCReq.paymentData = new InitiateCardPaymentdata()
                        {
                            type = new AuthenticationHelper().AesDecrypt(oReq.CardType),
                            data = new InitiateCardPaymentCardData()
                            {
                                numberPlain = new AuthenticationHelper().AesDecrypt(oReq.CardNumber),
                                cvv = new AuthenticationHelper().AesDecrypt(oReq.CardCVC),
                                expiryMonth = new AuthenticationHelper().AesDecrypt(oReq.CardExpiryMonth),
                                expiryYear = new AuthenticationHelper().AesDecrypt(oReq.CardExpiryYear)
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "InitiateNoonPaymentController", "InitiateCardPayment", JsonConvert.SerializeObject(oCCReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);

                    }
                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        analizer = new Analizer();
                        externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oCCReq), oReq.DeviceType, oReq.UserSessionId, oReq.SessionId, oReq.CinemaId, "InitiateNoonPayment", "restData.InitiateNoonPayment", string.Empty, oReq.DeviceDetails);
                    }
                    try
                    {
                        new BookingInfoDao().InsertPaymentLog(oReq.UserSessionId, oReq.OptionalClientId, Int64.Parse(oReq.SessionId), JsonConvert.SerializeObject(oCCReq), dAmount, oReq.tokenIdentifier, "Mobile", out iPaymentLogId,
                                                oReq.UserId, CustomerEmail, CustomerPhone, oReq.PhoneCountryCode, CustomerName, oReq.bookingInfoId, oReq.isNewsLetter, oReq.isIntrestedForLuckyDraw);
                    }
                    catch (Exception ex)
                    {
                        new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "InitiateNoonPaymentController", "InsertPaymentLog", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);

                    }

                    strResp = WebRequestForMobile.NoonPaymentWebRequest(NPURL, JsonConvert.SerializeObject(oCCReq), out isException, out strExceptionMsg);

                    if (oReq.IsSaveCard)
                    {
                        try
                        {
                            SaveNewCardReq oSNCReq = new SaveNewCardReq();
                            oSNCReq.cardNumber = oReq.CardNumber;
                            oSNCReq.expiryMonth = oReq.CardExpiryMonth;
                            oSNCReq.expiryYear = oReq.CardExpiryYear;
                            oSNCReq.brand = oReq.CardType;
                            oSNCReq.cardType = oReq.CardType;
                            oSNCReq.cardCVV = oReq.CardCVC;
                            oSNCReq.cardName = new AuthenticationHelper().AesDecrypt(oReq.CustomerName);
                            oSNCReq.emailId = new AuthenticationHelper().AesDecrypt(oReq.CustomerEmail);
                            oSNCReq.DeviceType = oReq.DeviceType;
                            var oSCC = pSaveCard(oSNCReq);
                        }
                        catch (Exception ex)
                        {
                            new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "InitiateNoonPaymentController", "SaveNewCard", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);

                        }
                    }

                }
                else
                {
                    InitiatePaymentByToken oTPReq = new InitiatePaymentByToken();
                    try
                    {
                        oTPReq.apiOperation = "INITIATE";
                        oTPReq.configuration = new PaymentByTokenConfiguration()
                        {
                            returnUrl = ConfigurationManager.AppSettings["PaymentResponseURL"].ToString(),
                            locale = "en",
                            tokenizeCc = true,
                            initiationValidity = DateTime.Now.AddDays(5).ToShortDateString(),
                            paymentAction = "AUTHORIZE"
                        };
                        oTPReq.order = new PaymentByTokenOrder()
                        {
                            name = "Payment for reel",
                            amount = strAmount,
                            currency = "AED",
                            channel = "mobile",
                            category = strNoonPaymentCategory,
                            description = "Payment for ticket purchase"
                        };
                        oTPReq.paymentData = new PaymentByTokenPaymentdata()
                        {
                            type = "CARD",
                            data = new PaymentByTokenData()
                            {
                                cvv = new AuthenticationHelper().AesDecrypt(oReq.CardCVC),
                                tokenIdentifier = oReq.tokenIdentifier
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "InitiateNoonPaymentController", "InitiatePaymentByToken", JsonConvert.SerializeObject(oTPReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);

                    }
                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        analizer = new Analizer();
                        externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oTPReq), oReq.DeviceType, oReq.UserSessionId, oReq.SessionId, oReq.CinemaId, "InitiateNoonPayment", "restData.InitiateNoonPayment", string.Empty, oReq.DeviceDetails);
                    }
                    try
                    {
                        new BookingInfoDao().InsertPaymentLog(oReq.UserSessionId, oReq.OptionalClientId, Int64.Parse(oReq.SessionId), JsonConvert.SerializeObject(oTPReq), dAmount, oReq.tokenIdentifier, "Mobile", out iPaymentLogId,
                                                                       oReq.UserId, CustomerEmail, CustomerPhone, oReq.PhoneCountryCode, CustomerName, oReq.bookingInfoId, oReq.isNewsLetter, oReq.isIntrestedForLuckyDraw);
                    }
                    catch (Exception ex)
                    {
                        new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "InitiateNoonPaymentController", "InsertPaymentLog", JsonConvert.SerializeObject(oTPReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);

                    }
                    strResp = WebRequestForMobile.NoonPaymentWebRequest(NPURL, JsonConvert.SerializeObject(oTPReq), out isException, out strExceptionMsg);
                }
                if (!isException)
                {
                    dynamic oNPResp = JsonConvert.DeserializeObject(strResp);
                    if (oNPResp.resultCode.ToString() == "0")
                    {
                        NPResp = JsonConvert.DeserializeObject<NoonPaymentResp>(strResp);

                        if (oReq.IsSaveCard && !string.IsNullOrEmpty(oReq.tokenIdentifier))
                        {
                            int i = new MobileBookingDao().InsertSavedCardInfo(oReq.UserId, NPResp.result.paymentDetails.tokenIdentifier, NPResp.result.paymentDetails.paymentInfo,
                                NPResp.result.paymentDetails.brand, NPResp.result.paymentDetails.cardType, Convert.ToInt16(NPResp.result.paymentDetails.expiryMonth),
                                Convert.ToInt16(NPResp.result.paymentDetails.expiryYear), NPResp.result.paymentDetails.cardType, CustomerEmail);
                        }
                    }
                    else
                    {
                        NPResp.resultCode = int.Parse(oNPResp.resultCode.ToString());
                        NPResp.message = oNPResp.message.ToString();
                    }
                }
                else
                {
                    NPResp.resultCode = 9999;
                    NPResp.message = strExceptionMsg;
                    new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "InitiateNoonPaymentController", "NoonPaymentWebRequest", JsonConvert.SerializeObject(oReq), "REQUEST", strExceptionMsg, oReq.DeviceDetails);

                }
                try { new BookingInfoDao().UpdatePaymentLog(iPaymentLogId, oReq.bookingInfoId, NPResp.result.order.id, JsonConvert.SerializeObject(NPResp), NPResp.resultCode.ToString(), NPResp.message, NPResp.result.paymentDetails.paymentInfo.ToString()); }
                catch (Exception ex)
                {
                    new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "InitiateNoonPaymentController", "UpdatePaymentLog", JsonConvert.SerializeObject(NPResp), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);

                }

                //int i = new BookingInfoDao().InsertNoonPaymentResponse(new InsertNoonPaymentResp()
                //{

                //});
                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(NPResp), NPResp.resultCode.ToString());
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(NPResp), NPResp.resultCode.ToString());
                }
                return NPResp;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "InitiateNoonPaymentController", "InitiateNoonPayment", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                return null;
            }
        }

        #endregion

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/MovieSessions")]
        public async Task<object> MovieSessions([FromBody] GetMoviesByExperienceReq oReq)
        {
            var reqHeader = Request.Headers.GetValues("connectapitoken");
            long originalRequestLogId = 0;
            long externalRequestLogId = 0;

            oMobileBookingDao = new MobileBookingDao();
            ServiceSettings = oMobileBookingDao.GetServiceSettings();
            try
            {


                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), null, null, null, null, "GetMoviesByExperience", "GetMoviesByExperience", string.Empty, oReq.DeviceDetails);
                }
                var sessionlist = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["SessionListJSONPath"]);

                List<SessionListRespData> oSL = new List<SessionListRespData>();
                oSL = JsonConvert.DeserializeObject<List<SessionListRespData>>(sessionlist);

                List<SessionListRespData> oSLResp = new List<SessionListRespData>();
                if (!String.IsNullOrEmpty(oReq.ExperienceName))
                {
                    foreach (var ms in oSL)
                    {
                        foreach (var ex in ms.Experiences)
                        {
                            string strEXName = ex.Type.Replace("-", "").ToLower();
                            if (strEXName.Contains(oReq.ExperienceName.ToLower()))
                            {
                                List<Mslst> msList = new List<Mslst>();
                                msList = (from x in ms.MSLst where x.EX.Replace("-", "").ToLower().Contains(oReq.ExperienceName.ToLower()) select x).ToList();
                                ms.MSLst = msList.ToArray();
                                oSLResp.Add(ms);
                            }
                        }
                    }
                }
                else
                {
                    oSLResp = oSL;
                }


                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oSLResp), oSLResp == null ? "0" : "OK");
                }

                return oSLResp;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "MovieSessionsController", "InitiateNoonPayment", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                ValidateOffErrorRes err = new ValidateOffErrorRes();
                err.statusCode = 0;
                err.statusMessage = ex.Message;
                return err;
            }
        }


        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetConfigurations")]
        public async Task<object> GetConfigurations([FromBody] GetConfigurationsReq oReq)
        {
            //var reqHeader = Request.Headers.GetValues("connectapitoken");

            try
            {


                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), oReq.DeviceType, null, null, null, "GetConfigurations", "GetConfigurations", string.Empty, oReq.DeviceDetails);
                }

                var json = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["ConfigurationJSONPath"].ToString());

                dynamic dJson = JsonConvert.DeserializeObject(json);
                string commonOBJ = dJson.ReelCinemasMobileConfiguration.Common.ToString();
                string deviceOSOBJ = dJson.ReelCinemasMobileConfiguration[oReq.DeviceOS].ToString();

                string strfinalOBJ = "{" + commonOBJ.TrimStart('{').TrimEnd('}') + "," + deviceOSOBJ.TrimStart('{').TrimEnd('}') + "}";


                var result = JsonConvert.DeserializeObject(strfinalOBJ);

                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (!string.IsNullOrEmpty(strfinalOBJ))
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(result), "1");
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(result), "");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetConfigurationsController", "GetConfigurations", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetSeatPlan")]
        public async Task<object> GetSeatPlan([FromBody] SeatPlanReq oSeatPlanReq)
        {
            try
            {
                var reqHeader = Request.Headers.GetValues("connectapitoken");


                string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
                string SalesChannel = ConfigurationManager.AppSettings["IOSSalesChannel"].ToString();
                if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
                {
                    OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
                    SalesChannel = ConfigurationManager.AppSettings["AndriodSalesChannel"].ToString();
                }

                VISTAResponse vistaResponse = null;
                NsSeatPlan.GetSeatPlanResp result = null;
                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oSeatPlanReq), oSeatPlanReq.DeviceType, null, oSeatPlanReq.SessionId.ToString(), oSeatPlanReq.CinemasId.ToString(), "GetSeatPlan", "GetSeatPlan", string.Empty, oSeatPlanReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    var extGetSeatPlanReq = new { cinemas = oSeatPlanReq.CinemasId, sessions = oSeatPlanReq.SessionId };
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(extGetSeatPlanReq), oSeatPlanReq.DeviceType, null, oSeatPlanReq.SessionId.ToString(), oSeatPlanReq.CinemasId.ToString(), "GetSeatPlan", "restData.GetSeatPlan", string.Empty, oSeatPlanReq.DeviceDetails);
                }

                #region Vista API Call
                string RestdataUrl = ConfigurationManager.AppSettings["RestdataUrl"].ToString();
                url = RestdataUrl + "/Cinemas/" + oSeatPlanReq.CinemasId + "/Sessions/" + oSeatPlanReq.SessionId + "/seat-plan";
                vistaResponse = await WebRequestForMobile.CreateWebRequest<NsSeatPlan.GetSeatPlanResp>(url, reqHeader.ToList()[0].ToString(), "GetSeatPlan", oSeatPlanReq.DeviceType, null);
                result = (NsSeatPlan.GetSeatPlanResp)vistaResponse.Result;
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
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetSeatPlanController", "GetSeatPlan", JsonConvert.SerializeObject(oSeatPlanReq), "REQUEST", JsonConvert.SerializeObject(ex), oSeatPlanReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetTickets")]
        public async Task<object> GetTickets([FromBody] GetTicketsReq oGetTicketsReq)
        {
            try
            {

                var reqHeader = Request.Headers.GetValues("connectapitoken");

                string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
                string SalesChannel = ConfigurationManager.AppSettings["IOSSalesChannel"].ToString();
                if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
                {
                    OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
                    SalesChannel = ConfigurationManager.AppSettings["AndriodSalesChannel"].ToString();
                }
                oGetTicketsReq.SalesChannelFilter = SalesChannel;

                VISTAResponse vistaResponse = null;
                NsTicketTypes.GetTicketTypesResp result = null;
                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetTicketsReq), oGetTicketsReq.DeviceType, null, oGetTicketsReq.SessionId.ToString(), oGetTicketsReq.CinemasId.ToString(), "GetTickets", "GetTickets", string.Empty, oGetTicketsReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    var extGetTicketsReq = new { cinemas = oGetTicketsReq.CinemasId, sessions = oGetTicketsReq.SessionId, salesChannelFilter = oGetTicketsReq.SalesChannelFilter };
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(extGetTicketsReq), oGetTicketsReq.DeviceType, null, oGetTicketsReq.SessionId.ToString(), oGetTicketsReq.CinemasId.ToString(), "GetTickets", "restData.GetTickets", string.Empty, oGetTicketsReq.DeviceDetails);
                }

                #region Vista API Call

                string RestdataUrl = ConfigurationManager.AppSettings["RestdataUrl"].ToString();
                url = RestdataUrl + "/Cinemas/" + oGetTicketsReq.CinemasId + "/Sessions/" + oGetTicketsReq.SessionId + "/tickets?salesChannelFilter=" + oGetTicketsReq.SalesChannelFilter;
                vistaResponse = await WebRequestForMobile.CreateWebRequest<NsTicketTypes.GetTicketTypesResp>(url, reqHeader.ToList()[0].ToString(), "GetTickets", oGetTicketsReq.DeviceType, null);
                result = (NsTicketTypes.GetTicketTypesResp)vistaResponse.Result;

                #endregion

                var Sessions_odata = GetSessions(reqHeader.ToList()[0].ToString());

                ODataSession objDataSessions = new ODataSession();
                DateTime ActualShowDateTime = DateTime.Now;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                var oSessionData = JsonConvert.DeserializeObject<ODataSession>(Sessions_odata.ToString(), settings);

                if (oSessionData.Value != null && oSessionData.Value.Count > 0)
                {
                    objDataSessions = oSessionData;
                }
                foreach (Session item in objDataSessions.Value)
                {
                    if (item.SessionId == oGetTicketsReq.SessionId)
                    {
                        ActualShowDateTime = item.Showtime;
                    }
                }

                List<DineInConfigList> configList = JsonConvert.DeserializeObject<List<DineInConfigList>>(new CommonHelper().ReadJsonFile("DineInCombo"));

                List<STTClass> StandardTicketTypesConfig = JsonConvert.DeserializeObject<List<STTClass>>(new CommonHelper().ReadJsonFile("StandardTicketTypes"));

                string tickettypes = string.Empty;
                foreach (var item in StandardTicketTypesConfig.ToList())
                {
                    if (item.CinemaId == oGetTicketsReq.CinemasId)
                    {
                        foreach (var tt in item.TicketTypes)
                        {
                            tickettypes += tt.TicketsType + "~";
                        }
                    }
                }

                tickettypes = tickettypes.TrimEnd('~');
                if (string.IsNullOrEmpty(oGetTicketsReq.OfferedTicketTypes))
                {
                    List<string> listTicketTypesToInclude = tickettypes.Split('~').ToList();

                    List<NsTicketTypes.Ticket> ticketTypesBySession = new List<NsTicketTypes.Ticket>();

                    if (result.ResponseCode == 0)
                    {
                        ticketTypesBySession = (from x in result.Tickets
                                                where listTicketTypesToInclude.Contains(x.TicketTypeCode) &&
                                                    (x.IsAvailableAsLoyaltyRecognitionOnly == false && x.IsAvailableForLoyaltyMembersOnly == false &&
                                                    x.IsChildOnlyTicket == false && x.IsComplimentaryTicket == false &&
                                                    x.IsDynamicallyPriced == false &&
                                                    x.IsRedemptionTicket == false && x.IsThirdPartyMemberTicket == false)
                                                select x).ToList();
                        foreach (var item in ticketTypesBySession)
                        {
                            #region Change Price and Description                            
                            foreach (var c in configList)
                            {
                                if (item.CinemaId == c.CinemaId && item.TicketTypeCode == c.TicketType)
                                {
                                    item.PriceInCents = c.FoodPriceInCents + c.TicketPriceInCents;
                                    item.Description = c.Description;
                                }

                            }
                            #endregion
                        }
                    }

                    var ticketTypesResponse = new { ResponseCode = result.Tickets == null ? 1 : result.ResponseCode, Tickets = ticketTypesBySession, ActualShowDateTime = ActualShowDateTime };
                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        if (vistaResponse.Response != null)
                        {
                            analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ticketTypesResponse), vistaResponse.Response == null ? "" : vistaResponse.Response.StatusCode.ToString());
                        }
                        else
                        {
                            analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ticketTypesResponse), "");
                        }
                    }
                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(ticketTypesResponse), "");
                    }
                    return ticketTypesResponse;
                }
                else
                {
                    List<string> listTicketsToInclude = oGetTicketsReq.OfferedTicketTypes.Split(',').ToList();

                    string Dine_IN_Ticket_Only_Code = ServiceSettings.Rows[0]["Dine_IN_Ticket_Only_Code"].ToString();
                    List<string> listTicketTypesToInclude = tickettypes.Split('~').ToList();

                    var offeredTicketsListByType = (from x in result.Tickets
                                                    where listTicketsToInclude.Contains(x.TicketTypeCode)
                                                    select x);
                    //listTicketTypesToInclude.Contains(x.TicketTypeCode)

                    foreach (var item in offeredTicketsListByType)
                    {
                        #region Change Price and Description
                        //TDM
                        foreach (var c in configList)
                        {
                            if (item.CinemaId == c.CinemaId && item.TicketTypeCode == c.TicketType)
                            {
                                item.PriceInCents = c.FoodPriceInCents + c.TicketPriceInCents;
                                item.Description = c.Description;
                            }

                        }
                        #endregion
                    }

                    var ticketTypesResponse = new { ResponseCode = offeredTicketsListByType == null ? 1 : result.ResponseCode, Tickets = offeredTicketsListByType, ActualShowDateTime = ActualShowDateTime };
                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        if (vistaResponse.Response != null)
                        {
                            analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ticketTypesResponse), vistaResponse.Response == null ? "" : vistaResponse.Response.StatusCode.ToString());
                        }
                        else
                        {
                            analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ticketTypesResponse), "");
                        }
                    }

                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(ticketTypesResponse), "");
                    }

                    return ticketTypesResponse;
                }


            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetTicketsController", "GetTickets", JsonConvert.SerializeObject(oGetTicketsReq), "REQUEST", JsonConvert.SerializeObject(ex), oGetTicketsReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetConcessionList")]
        public async Task<object> GetConcessionList([FromBody] GetConcessionListReq oGetConcessionListReq)
        {
            try
            {

                var reqHeader = Request.Headers.GetValues("connectapitoken");
                string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
                string SalesChannel = ConfigurationManager.AppSettings["IOSSalesChannel"].ToString();
                if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
                {
                    OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
                    SalesChannel = ConfigurationManager.AppSettings["AndriodSalesChannel"].ToString();
                }
                oGetConcessionListReq.ClientId = OptionalClientId;


                VISTAResponse vistaResponse = null;
                NsConcessionList.GetConcessionListResp result = null;
                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetConcessionListReq), oGetConcessionListReq.DeviceType, null, oGetConcessionListReq.SessionId.ToString(), oGetConcessionListReq.CinemaId.ToString(), "GetConcessionList", "GetConcessionList", string.Empty, oGetConcessionListReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    var extGetConcessionListReq = new { cinemas = oGetConcessionListReq.CinemaId, clientId = oGetConcessionListReq.ClientId };
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(extGetConcessionListReq), oGetConcessionListReq.DeviceType, null, oGetConcessionListReq.SessionId.ToString(), oGetConcessionListReq.CinemaId.ToString(), "GetConcessionList", "restData.concession-items", string.Empty, oGetConcessionListReq.DeviceDetails);
                }

                #region Vista API Call
                string RestdataUrl = ConfigurationManager.AppSettings["RestdataUrl"].ToString();
                url = RestdataUrl + "/concession-items-grouped-by-tabs?cinemaId=" + oGetConcessionListReq.CinemaId + "&clientId=" + oGetConcessionListReq.ClientId;
                vistaResponse = await WebRequestForMobile.CreateWebRequest<NsConcessionList.GetConcessionListResp>(url, reqHeader.ToList()[0].ToString(), "GetConcessionList", oGetConcessionListReq.DeviceType, null);
                result = (NsConcessionList.GetConcessionListResp)vistaResponse.Result;
                #endregion

                string concessionItemsFromDb = "";

                #region Filter Concessions List
                if ((oGetConcessionListReq.CinemaId == "0001" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["TheDubaiMall_Dine_in_Kids_Combo"].ToString()) || (oGetConcessionListReq.CinemaId == "0003" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["JARCMall_Dine_in_Kids_Combo"].ToString()) || (oGetConcessionListReq.CinemaId == "0004" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["LePointe_Dine_in_Kids_Combo"].ToString()))
                {
                    concessionItemsFromDb = ServiceSettings.Rows[0]["ConcessionItemsToInclude_" + oGetConcessionListReq.CinemaId + "_Dine_in_Kids_Combo"].ToString();
                }
                else if ((oGetConcessionListReq.CinemaId == "0001" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["TheDubaiMall_Dine_in_Meal_Choice_Combo"].ToString()) || (oGetConcessionListReq.CinemaId == "0003" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["JARCMall_Dine_in_Meal_Choice_Combo"].ToString()) || (oGetConcessionListReq.CinemaId == "0004" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["LePointe_Dine_in_Meal_Choice_Combo"].ToString()))
                {
                    concessionItemsFromDb = ServiceSettings.Rows[0]["ConcessionItemsToInclude_" + oGetConcessionListReq.CinemaId + "_Dine_in_Meal_Choice_Combo"].ToString();
                }
                else if ((oGetConcessionListReq.CinemaId == "0001" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["TheDubaiMall_Dine_in_Meal_Combo"].ToString()) || (oGetConcessionListReq.CinemaId == "0003" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["JARCMall_Dine_in_Meal_Combo"].ToString()) || (oGetConcessionListReq.CinemaId == "0004" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["LePointe_Dine_in_Meal_Combo"].ToString()))
                {
                    concessionItemsFromDb = ServiceSettings.Rows[0]["ConcessionItemsToInclude_" + oGetConcessionListReq.CinemaId + "_Dine_in_Meal_Combo"].ToString();
                }
                else if ((oGetConcessionListReq.CinemaId == "0001" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["TheDubaiMall_Dine_in_Full_Meal_Combo"].ToString()) || (oGetConcessionListReq.CinemaId == "0003" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["JARCMall_Dine_in_Full_Meal_Combo"].ToString()) || (oGetConcessionListReq.CinemaId == "0004" && oGetConcessionListReq.TicketTypeCode == ServiceSettings.Rows[0]["LePointe_Dine_in_Full_Meal_Combo"].ToString()))
                {
                    concessionItemsFromDb = ServiceSettings.Rows[0]["ConcessionItemsToInclude_" + oGetConcessionListReq.CinemaId + "_Dine_in_Full_Meal_Combo"].ToString();
                }
                else
                {
                    concessionItemsFromDb = ServiceSettings.Rows[0]["ConcessionItemsToInclude_" + oGetConcessionListReq.CinemaId + "_Dine_in_Full_Meal_Combo"].ToString();
                }

                List<string> listConcessionItemsToInclude = concessionItemsFromDb.Split('|').ToList();
                var concessionListByType = (from x in result.ConcessionTabs where listConcessionItemsToInclude.Contains(x.Name) select x);

                #endregion
                var resp = concessionListByType.ToList();
                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (vistaResponse.Response != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(concessionListByType), vistaResponse.Response.StatusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(concessionListByType), "");
                    }
                }

                foreach (var v in resp)
                {
                    if (v.Name == "CMBStarer")
                    {
                        v.Name = "Starter";
                    }
                    else if (v.Name == "CMBMains")
                    {
                        v.Name = "Main";
                    }
                    else if (v.Name == "DRINKSFULL")
                    {
                        v.Name = "Drink";
                    }
                    else if (v.Name == "CMBDessert")
                    {
                        v.Name = "Dessert";
                    }
                }
                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (vistaResponse.Response != null)
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(resp), vistaResponse.Response.StatusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(resp), "");
                    }
                }
                return resp;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetConcessionListController", "GetConcessionList", JsonConvert.SerializeObject(oGetConcessionListReq), "REQUEST", JsonConvert.SerializeObject(ex), oGetConcessionListReq.DeviceDetails);
                return null;
            }
        }


        public async Task<object> AddTicketDiscount(AddTicketReq oAddTicketReq, IEnumerable<string> reqHeader)
        {
            try
            {


                #region Vista API Call Offer

                long originalRequestLogId = 0;
                long externalRequestLogId = 0;
                NsAddTicket.AddTicketResp result = null;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oAddTicketReq.TicketTypes), oAddTicketReq.DeviceType, oAddTicketReq.UserSessionId, oAddTicketReq.SessionId.ToString(), oAddTicketReq.CinemaId.ToString(), "AddTicketDiscount", "AddTicketDiscount", string.Empty, oAddTicketReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oAddTicketReq.TicketTypes), oAddTicketReq.DeviceType, oAddTicketReq.UserSessionId, oAddTicketReq.SessionId.ToString(), oAddTicketReq.CinemaId.ToString(), "AddTicketDiscount", "restTicketing.Ticket_Discount", string.Empty, oAddTicketReq.DeviceDetails);
                }


                VISTAResponse vistaResponse = null;
                string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
                string url = RestTicketingUrl + "/Order/discount";

                List<TicketDiscounts> lstticketDiscount = new List<TicketDiscounts>();

                int k = 0;
                for (int i = 0; i < oAddTicketReq.TicketTypes.Count; i++)
                {
                    for (int j = 0; j < oAddTicketReq.TicketTypes[i].Qty; j++)
                    {
                        if (!string.IsNullOrEmpty(oAddTicketReq.TicketTypes[i].DiscountCode))
                        {
                            lstticketDiscount.Add(new TicketDiscounts() { DiscountCode = oAddTicketReq.TicketTypes[i].DiscountCode, ItemId = k + 1 });
                            k = k + 1;
                        }
                    }
                }

                var ii = new
                {
                    OptionalClientId = oAddTicketReq.OptionalClientId,
                    UserSessionId = oAddTicketReq.UserSessionId,
                    CinemaId = oAddTicketReq.CinemaId,
                    TicketDiscounts = lstticketDiscount,
                    ReturnOrder = true,
                    UserSelectedSeatingSupported = true,
                    SkipAutoAllocation = true
                };

                if (lstticketDiscount.Count > 0)
                {
                    vistaResponse = await WebRequestForMobile.CreateWebPostRequest<NsAddTicket.AddTicketResp>(url, reqHeader.ToList()[0].ToString(), ii, "AddTicketDiscount", oAddTicketReq.DeviceType, oAddTicketReq.UserSessionId);
                    result = (NsAddTicket.AddTicketResp)vistaResponse.Result;

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
                }
                else
                {
                    result = null;
                    new CommonHelper().CreateLog("", "ERROR", "AddTicketDiscount", "AddTicketResp", JsonConvert.SerializeObject(ii), "REQUEST", "No Data.", oAddTicketReq.DeviceDetails);
                }

                return result;
                #endregion
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "AddTicketDiscount", "AddTicketDiscount", JsonConvert.SerializeObject(oAddTicketReq), "REQUEST", JsonConvert.SerializeObject(ex), oAddTicketReq.DeviceDetails);
                return null;
            }
        }


        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetSavedCards")]
        public async Task<object> GetSavedCards([FromBody] GetSavedCardReq oReq)
        {
            try
            {
                GetSavedCardResp resp = new GetSavedCardResp();
                DataTable dt = new DataTable();
                dt = new MobileBookingDao().GetSavedCard(oReq.userId, oReq.cardId);
                if (dt.Rows.Count > 0)
                {
                    resp.statusCode = 1;
                    resp.statusMessage = "Records available";
                    string str = JsonConvert.SerializeObject(dt);
                    resp.Sourcedata = JsonConvert.DeserializeObject<GetSavedCardSourceData[]>(str);
                }
                else
                {
                    resp.statusCode = 0;
                    resp.statusMessage = "No Records available";
                }
                return resp;

            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetSavedCardsController", "GetSavedCards", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                GetSavedCardResp err = new GetSavedCardResp();
                err.statusCode = 0;
                err.statusMessage = ex.Message;
                return err;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/DeleteSavedCard")]
        public async Task<object> DeleteSavedCard([FromBody] DeleteSavedCardReq oReq)
        {
            try
            {
                DeleteSavedCardResp resp = new DeleteSavedCardResp();

                int i = new MobileBookingDao().DeleteSavedCard(oReq.cardId);
                if (i > 0)
                {
                    resp.statusCode = 1;
                    resp.statusMessage = "Deleted Successfully";
                }
                else
                {
                    resp.statusCode = 0;
                    resp.statusMessage = "No Record Found";
                }
                return resp;

            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "DeleteSavedCardController", "DeleteSavedCard", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                DeleteSavedCardResp err = new DeleteSavedCardResp();
                err.statusCode = 0;
                err.statusMessage = ex.Message;
                return err;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/SaveNewCard")]
        public async Task<object> SaveNewCard([FromBody] SaveNewCardReq oReq)
        {
            return pSaveCard(oReq);
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetOffers")]
        public async Task<object> GetOffers([FromBody] GetOffersReq oGetOffersReq)
        {
            try
            {
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;


                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetOffersReq), null, null, oGetOffersReq.SessionId.ToString(), oGetOffersReq.CinemaId.ToString(), "GetOffers", "GetOffers", string.Empty, oGetOffersReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetOffersReq), null, null, oGetOffersReq.SessionId.ToString(), oGetOffersReq.CinemaId.ToString(), "oGetOffersReq", "restTicketing.GetOffers", string.Empty, oGetOffersReq.DeviceDetails);
                }


                GetOffersResp oResp = new GetOffersResp();

                string RestdataUrl = ConfigurationManager.AppSettings["InfluxBaseURL"].ToString();
                string url = RestdataUrl + "/OfferAPPAPI/GetOffers";
                string response = WebRequestForMobile.WebServicesDoPost(url, JsonConvert.SerializeObject(oGetOffersReq));

                oResp = JsonConvert.DeserializeObject<GetOffersResp>(response);

                var temp = (from x in oResp.Sourcedata where x.id != 6 && x.id != 8 && x.id != 3 select x);
                oResp.Sourcedata = temp.ToArray();

                foreach (var item in oResp.Sourcedata)
                {
                    string termsAndCondition = item.termsAndCondition.Replace("#", "<li style='font-weight:bold;'>").Replace("|", "</li><li>").Replace("<li>\t\r\n", "<br/>").Replace("\r\n", "<br/><br/>");
                    item.termsAndCondition = "<ul style='list-style: none;'>" + termsAndCondition + "</li></ul>";
                }
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp == null ? "" : oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }

                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oResp != null)
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), oResp.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResp), "");
                    }
                }
                return oResp;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetOffersController", "GetOffers", JsonConvert.SerializeObject(oGetOffersReq), "REQUEST", JsonConvert.SerializeObject(ex), oGetOffersReq.DeviceDetails);
                ValidateOffErrorRes err = new ValidateOffErrorRes();
                err.statusCode = 0;
                err.statusMessage = ex.Message;
                return err;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/ValidateOffers")]
        public async Task<object> ValidateOffers([FromBody] ValidateOffersReq oValidateOffersReq)
        {
            try
            {
                var reqHeader = Request.Headers.GetValues("connectapitoken");


                string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
                string SalesChannel = ConfigurationManager.AppSettings["IOSSalesChannel"].ToString();
                if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
                {
                    OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
                    SalesChannel = ConfigurationManager.AppSettings["AndriodSalesChannel"].ToString();
                }

                string errMSG = string.Empty;
                bool IsError = false;


                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oValidateOffersReq), oValidateOffersReq.DeviceType, "", oValidateOffersReq.SessionId.ToString(), oValidateOffersReq.CinemaId.ToString(), "ValidateOffers", "ValidateOffers", string.Empty, oValidateOffersReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oValidateOffersReq), oValidateOffersReq.DeviceType, "", oValidateOffersReq.SessionId.ToString(), oValidateOffersReq.CinemaId.ToString(), "ValidateOffers", "restTicketing.ValidateOffers", string.Empty, oValidateOffersReq.DeviceDetails);
                }


                ValidateOffersResponse vResp = new ValidateOffersResponse();
                string response = ValidateOffer_Influx(oValidateOffersReq, out errMSG, out IsError);


                if (IsError)
                {
                    //Ext Service Response Log
                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", response, "");
                    }
                    ValidateOffErrorRes err = new ValidateOffErrorRes();
                    err.statusCode = 0;
                    err.statusMessage = errMSG;
                    return err;
                }
                else
                {
                    dynamic d = JsonConvert.DeserializeObject(response);
                    //Ext Service Response Log
                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", response, d.statusCode.ToString());
                    }
                    if (d.statusCode.ToString() == "0")
                    {

                        ValidateOffErrorRes err = new ValidateOffErrorRes();
                        err.statusCode = int.Parse(d.statusCode.ToString());
                        err.statusMessage = d.statusMessage.ToString();
                        return err;
                    }
                    else
                    {
                        vResp = JsonConvert.DeserializeObject<ValidateOffersResponse>(response);
                    }

                    if (vResp.Sourcedata.validateOffersResponse.Pattern == "BOGO")
                    {


                        GetTicketsReq oGetTicketsReq = new GetTicketsReq();
                        oGetTicketsReq.CinemasId = oValidateOffersReq.CinemaId;
                        oGetTicketsReq.DeviceType = oValidateOffersReq.DeviceType;
                        oGetTicketsReq.SalesChannelFilter = SalesChannel;
                        oGetTicketsReq.SessionId = oValidateOffersReq.SessionId;

                        List<NsTicketTypes.Ticket> lstTickets = await GetTicketTypes(oGetTicketsReq);


                        List<string> FreeTicketTypes = new List<string>();
                        List<string> TicketTypes = new List<string>();
                        string bankName = oValidateOffersReq.offerHeader.ToLower().Replace("bank", "").Trim();

                        var offeredTicketsListByType = (from x in lstTickets
                                                        where x.Description.ToString().ToLower().Contains(bankName)
                                                        select x);

                        foreach (var item in lstTickets)
                        {
                            if (item.Description.ToLower().Contains(bankName))
                            {
                                if (item.PriceInCents == 0)
                                {
                                    FreeTicketTypes.Add(item.TicketTypeCode);
                                }
                                else
                                {
                                    TicketTypes.Add(item.TicketTypeCode);
                                }

                            }
                        }
                        vResp.Sourcedata.validateOffersResponse.TicketTypes = TicketTypes.ToArray();
                        vResp.Sourcedata.validateOffersResponse.FreeTicketTypes = FreeTicketTypes.ToArray();
                    }

                    //Wrapper Service Response Log
                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        if (vResp != null)
                        {
                            analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(vResp), vResp.statusCode.ToString());
                        }
                        else
                        {
                            analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(vResp), "");
                        }
                    }
                    return vResp;
                }
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "ValidateOffersController", "ValidateOffers", JsonConvert.SerializeObject(oValidateOffersReq), "REQUEST", JsonConvert.SerializeObject(ex), oValidateOffersReq.DeviceDetails);
                ValidateOffErrorRes err = new ValidateOffErrorRes();
                err.statusCode = 0;
                err.statusMessage = ex.Message;
                return err;
            }
        }


        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/AddTicket")]
        public async Task<object> AddTicket([FromBody] AddTicketReq oAddTicketReq)
        {
            try
            {
                var reqHeader = Request.Headers.GetValues("connectapitoken");
                string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
                string SalesChannel = ConfigurationManager.AppSettings["IOSSalesChannel"].ToString();
                if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
                {
                    OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
                    SalesChannel = ConfigurationManager.AppSettings["AndriodSalesChannel"].ToString();
                }
                oAddTicketReq.OptionalClientId = OptionalClientId;

                VISTAResponse vistaResponse = null;
                NsAddTicket.AddTicketResp result = null;
                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                #region Change Price
                GetTicketsReq oGetTicketsReq = new GetTicketsReq();
                oGetTicketsReq.CinemasId = oAddTicketReq.CinemaId;
                oGetTicketsReq.DeviceType = oAddTicketReq.DeviceType;
                oGetTicketsReq.SalesChannelFilter = SalesChannel;
                oGetTicketsReq.SessionId = oAddTicketReq.SessionId;

                List<NsTicketTypes.Ticket> lstTickets = await GetTicketTypes(oGetTicketsReq);

                foreach (var item in oAddTicketReq.TicketTypes)
                {
                    if (lstTickets != null && lstTickets.Count > 0)
                    {
                        item.PriceInCents = lstTickets.Where(s => s.TicketTypeCode == item.TicketTypeCode).First().PriceInCents.ToString();
                    }
                }
                #endregion

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oAddTicketReq), oAddTicketReq.DeviceType, oAddTicketReq.UserSessionId, oAddTicketReq.SessionId.ToString(), oAddTicketReq.CinemaId.ToString(), "AddTicket", "AddTicket", string.Empty, oAddTicketReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oAddTicketReq), oAddTicketReq.DeviceType, oAddTicketReq.UserSessionId, oAddTicketReq.SessionId.ToString(), oAddTicketReq.CinemaId.ToString(), "AddTicket", "restTicketing.Order_tickets", string.Empty, oAddTicketReq.DeviceDetails);
                }

                #region Vista API Call
                string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
                url = RestTicketingUrl + "/Order/tickets";
                vistaResponse = await WebRequestForMobile.CreateWebPostRequest<NsAddTicket.AddTicketResp>(url, reqHeader.ToList()[0].ToString(), oAddTicketReq, "AddTicket", oAddTicketReq.DeviceType, oAddTicketReq.UserSessionId);
                result = (NsAddTicket.AddTicketResp)vistaResponse.Result;
                #endregion

                //Apply Discount

                object orderAfterDiscount = await AddTicketDiscount(oAddTicketReq, reqHeader);

                if (orderAfterDiscount != null)
                {
                    result = (NsAddTicket.AddTicketResp)orderAfterDiscount;
                }

                //Call Get Order
                GetOrderReq oGetOrderReq = new GetOrderReq();
                oGetOrderReq.CinemaId = oAddTicketReq.CinemaId;
                oGetOrderReq.OptionalClientId = oAddTicketReq.OptionalClientId;
                oGetOrderReq.ProcessOrderValue = true;
                oGetOrderReq.UserSessionId = oAddTicketReq.UserSessionId;

                //NsGetOrder.GetOrderResp oGetOrderResp = await GetOrderInfo(oGetOrderReq);


                var sessionlist = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["SessionListJSONPath"]);


                var oSL = JsonConvert.DeserializeObject<List<SessionListRespData>>(sessionlist);

                string strSeatNos = string.Empty;
                string strTicketTypeCode = string.Empty;



                foreach (var item in result.Order.Sessions[0].Tickets)
                {
                    strTicketTypeCode += item.TicketTypeCode + "|";
                    strSeatNos += item.SeatData + ",";
                }

                decimal ticketAmount = Math.Round(decimal.Divide(Convert.ToDecimal(result.Order.TotalValueCents), 100), 2);

                decimal totalAmt = Math.Round(decimal.Divide(Convert.ToDecimal(result.Order.TotalValueCents), 100), 2);

                decimal vatAmount = ((totalAmt / Convert.ToDecimal(1.05)) * Convert.ToDecimal(ConfigurationManager.AppSettings["BookingVAT"]));

                InsertBookingInfo oIBF = new InsertBookingInfo();
                foreach (var m in oSL)
                {
                    foreach (var s in m.MSLst)
                    {
                        if (s.SID == oAddTicketReq.SessionId)
                        {
                            oIBF.userId = 0;
                            oIBF.userSessionId = oAddTicketReq.UserSessionId;
                            oIBF.movieId = m.MID;
                            oIBF.movieName = m.MN;
                            oIBF.genre = m.GR;
                            oIBF.language = m.ML;
                            oIBF.cinemaId = s.CID;
                            oIBF.cinemaName = s.CN;
                            oIBF.screenName = s.SC;
                            oIBF.screenNo = s.SC;
                            oIBF.experience = s.EX;
                            oIBF.rating = m.RT;
                            oIBF.sessionId = oAddTicketReq.SessionId;
                            oIBF.noOfSeats = result.Order.TotalOrderCount;
                            oIBF.showdate = Convert.ToDateTime(s.SD);
                            oIBF.showtime = Convert.ToDateTime(s.SD);
                            oIBF.ticketAmount = ticketAmount;
                            oIBF.totalAmount = (Convert.ToDecimal(result.Order.TotalValueCents) / 100);
                            oIBF.seatInfo = strSeatNos.TrimEnd(',');
                            oIBF.ticketTypeCode = strTicketTypeCode.TrimEnd('|');
                            oIBF.ticketDescription = "";
                            oIBF.bookingdate = DateTime.Now;
                            oIBF.offerId = oAddTicketReq.OfferId;
                            oIBF.VatAmount = vatAmount;
                            oIBF.foodDesc = "";
                            oIBF.foodAmount = 0;
                            oIBF.memberIdOrEmailId = "";
                            oIBF.savings = 0;
                            oIBF.offerDescription = "";
                        }
                    }
                }

                int bookingInfoId = new BookingInfoDao().InsertBookingInfo(oIBF, oAddTicketReq.DeviceDetails);
                result.bookingInfoId = bookingInfoId;


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
            catch (Exception ex)
            {
                new CommonHelper().CreateLog(oAddTicketReq.UserSessionId, "ERROR", "AddTicket", "AddTicket", JsonConvert.SerializeObject(oAddTicketReq), "REQUEST", JsonConvert.SerializeObject(ex), oAddTicketReq.DeviceDetails);

                ValidateOffErrorRes err = new ValidateOffErrorRes();
                err.statusCode = 0;
                err.statusMessage = ex.Message;
                return err;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/SetSeat")]
        public async Task<object> SetSeat([FromBody] SetSeatReq oSetSeatReq)
        {
            try
            {


                var reqHeader = Request.Headers.GetValues("connectapitoken");
                string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
                if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
                {
                    OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
                }
                oSetSeatReq.OptionalClientId = OptionalClientId;
                VISTAResponse vistaResponse = null;
                NsSetSeat.SetSeatResp result = null;
                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oSetSeatReq), oSetSeatReq.DeviceType, oSetSeatReq.UserSessionId, oSetSeatReq.SessionId.ToString(), oSetSeatReq.CinemaId.ToString(), "SetSeat", "SetSeat", string.Empty, oSetSeatReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oSetSeatReq), oSetSeatReq.DeviceType, oSetSeatReq.UserSessionId, oSetSeatReq.SessionId.ToString(), oSetSeatReq.CinemaId.ToString(), "SetSeat", "restTicketing.Order_Seats", string.Empty, oSetSeatReq.DeviceDetails);
                }

                #region Vista API Call
                string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
                url = RestTicketingUrl + "/Order/Seats";
                vistaResponse = await WebRequestForMobile.CreateWebPostRequest<NsSetSeat.SetSeatResp>(url, reqHeader.ToList()[0].ToString(), oSetSeatReq, "SetSeat", oSetSeatReq.DeviceType, oSetSeatReq.UserSessionId);
                result = (NsSetSeat.SetSeatResp)vistaResponse.Result;
                #endregion
                string strTicketDescription = string.Empty;
                if (result.Result == 0)
                {

                    List<string> DistinctAreaCodes = new List<string>(); ;
                    foreach (var t in result.Order.Sessions[0].Tickets)
                    {
                        DistinctAreaCodes.Add(t.TicketTypeCode);
                    }

                    var da = DistinctAreaCodes.Select(x => x).Distinct();
                    foreach (var a in da.ToList())
                    {
                        string Desc = string.Empty;
                        string strSeatNo = string.Empty;
                        Int64 iAmount = 0;
                        int quantity = 0;
                        foreach (var t in result.Order.Sessions[0].Tickets)
                        {
                            if (t.TicketTypeCode == a.ToString())
                            {
                                Desc = t.Description;
                                strSeatNo += t.SeatData + ",";
                                iAmount += t.DealPriceCents;
                                quantity++;
                            }
                        }
                        decimal dAmount = Convert.ToDecimal(iAmount) / 100;
                        strTicketDescription += Desc + "|" + strSeatNo.TrimEnd(',') + "|" + dAmount.ToString() + "|" + quantity.ToString() + "#";
                    }

                    new BookingInfoDao().UpdateTicketDescription(oSetSeatReq.bookingInfoId, strTicketDescription.TrimEnd('#'), 0, "");
                }

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

                if (result.Result == 8)
                {
                    result.ErrorDescription = "The selected seats are not avialable. Please retry by selecting different seats.";
                }
                return result;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog(oSetSeatReq.UserSessionId, "ERROR", "SetSeat", "SetSeat", JsonConvert.SerializeObject(oSetSeatReq), "REQUEST", JsonConvert.SerializeObject(ex), oSetSeatReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/EmaarEmpSendOtp")]
        public async Task<object> EmaarEmpSendOtp([FromBody]EmaarEmpSendOtpReq req)
        {



            EmaarEmpSendOTPRes response = new EmaarEmpSendOTPRes();
            var reqHeader = Request.Headers.GetValues("connectapitoken");
            string url = "";
            long originalRequestLogId = 0;
            long externalRequestLogId = 0;
            oMobileBookingDao = new MobileBookingDao();
            ServiceSettings = oMobileBookingDao.GetServiceSettings();
            try
            {
                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(req), req.DeviceType, "", req.SessionId.ToString(), req.CinemaId.ToString(), "EmaarEmpSendOtp", "EmaarEmpSendOtp", string.Empty, req.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(req), req.DeviceType, "", req.SessionId.ToString(), req.CinemaId.ToString(), "EmaarEmpSendOtp", "restTicketing.EmaarEmpSendOtp", string.Empty, req.DeviceDetails);
                }


                //var httpWebRequest = (HttpWebRequest)WebRequest.Create(ServiceSettings.Rows[0]["Employee_Offer_SendOTP_URL"].ToString());
                //httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers["ClientProfileId"] = ServiceSettings.Rows[0]["Employee_Offer_ClientProfileId"].ToString();
                //httpWebRequest.Method = "POST";

                //if (ConfigurationManager.AppSettings["isProxyEnabled"].ToString() == "True")
                //{
                //    var proxy = new WebProxy(ConfigurationManager.AppSettings["ProxyURL"].ToString());
                //    proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ProxyUserName"].ToString(), ConfigurationManager.AppSettings["ProxyPassword"].ToString());
                //    httpWebRequest.Proxy = proxy;
                //}


                //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                //{
                //    string json = "{\"Email\":\"" + req.Email + "\"}";

                //    streamWriter.Write(json);
                //    streamWriter.Flush();
                //    streamWriter.Close();
                //}
                //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //var responseString = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
                string json = "{\"Email\":\"" + req.Email + "\"}";
                string URL = ServiceSettings.Rows[0]["Employee_Offer_SendOTP_URL"].ToString();
                var responseString = WebRequestForMobile.EmaarOTPWebRequest(json, URL);

                dynamic json1 = JsonConvert.DeserializeObject<object>(responseString);

                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (responseString != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(json1), json1.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(json1), "");
                    }
                }

                if (json1.Status == "Success" && json1.StatusCode == "0")
                {
                    string strRD = json1.ResposeData.ToString();
                    dynamic rd = JsonConvert.DeserializeObject(strRD);
                    //OTPDataRes otpk = JsonConvert.DeserializeObject<OTPDataRes>(JsonConvert.DeserializeObject(json1.ResposeData));

                    if (json1.ResposeData != "")
                    {
                        response.Status = "True";
                        response.statusCode = 1;
                        response.OTPKey = rd.OTPKey;
                        response.Message = json1.Status;
                    }
                    else
                    {
                        response.Status = "False";
                        response.statusCode = 0;
                        response.OTPKey = "";
                        response.Message = json1.Status;
                    }
                    //return "Success";
                }

                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(response), response.statusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog(req.Email, "ERROR", "EmaarEmpSendOtpController", "EmaarEmpSendOtp", JsonConvert.SerializeObject(req), "REQUEST", JsonConvert.SerializeObject(ex), req.DeviceDetails);

                response.statusCode = 0;
                response.Message = ex.Message;
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(response), response.statusCode.ToString());
                }
            }

            return response;
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/AddWallet")]
        public object AddWallet([FromBody] AddWalletReq oReq)
        {
            AddWalletRes response = new AddWalletRes();
            string url = string.Empty;
            var reqHeader = Request.Headers.GetValues("connectapitoken");

            long originalRequestLogId = 0;
            oMobileBookingDao = new MobileBookingDao();
            ServiceSettings = oMobileBookingDao.GetServiceSettings();

            try
            {
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), oReq.DeviceType, oReq.UserSessionId, oReq.SessionId.ToString(), oReq.CinemaId.ToString(), "AddWallet", "AddWallet", string.Empty, oReq.DeviceDetails);
                }

                if (!string.IsNullOrEmpty(oReq.bookingId))
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
                    request.BookingID = oReq.bookingId;
                    request.AddBarcode(BarcodeType.PKBarcodeFormatQR, request.BookingID, "iso-8859-1");
                    string moviePoster = ConfigurationManager.AppSettings["WebsiteServerPath"].ToString() + "/Content/images/Movies/" + oReq.movieId + ".jpg";

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

                    request.CinemaName = oReq.cinemaName;
                    request.MovieName = oReq.movieName;

                    request.TicketInfo = "SEATS : ";// + string.Join(",", ConvertToTicketInfoList(oReq.ticketDescription).Select(l => l.SeatInfo).ToList());
                    request.Screenname = oReq.screenName;
                    request.Rating = oReq.rating;

                    request.ShowDate = oReq.showtime.ToString("ddd dd MMM yyyy");
                    request.Showtime = oReq.showtime.ToString("hh:mm tt");
                    request.Total = "AED " + oReq.totalAmount.ToString("0.00");
                    request.Experience = oReq.experience;
                    request.Relavenantdate = oReq.showtime.AddMinutes(-30).ToString("yyyy-MM-ddTHH:mm:ss+04:00");
                    byte[] bFile = generator.Generate(request);

                    response.FileName = oReq.bookingId + ".pkpass";


                    try
                    {
                        File.WriteAllBytes(ConfigurationManager.AppSettings["pkpassPath"].ToString() + "/" + response.FileName, bFile);
                    }
                    catch (Exception ex)
                    {
                        new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "AddWalletController", "WriteAllBytes", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                    }
                    string fileURL = ConfigurationManager.AppSettings["APIBaseURL"].ToString() + "/pkpass/" + response.FileName;
                    response.FileURL = new AuthenticationHelper().AesEncrypt(fileURL).ToString();
                    response.FileBase64String = "";// Convert.ToBase64String(bFile);
                    response.FileName = oReq.bookingId + ".pkpass";

                    response.statusCode = 1;
                    response.statusMessage = "Generated successfully.";

                }
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog(oReq.UserSessionId, "ERROR", "AddWalletController", "AddWallet", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);

                response.statusCode = 0;
                response.statusMessage = ex.Message;
                response.FileBase64String = "";
                response.FileName = "";
            }

            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(response), response.statusCode.ToString());
            }
            return response;
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/AddConcessions")]
        public async Task<object> AddConcessions([FromBody] AddConcessionsReq oAddConcessionsReq)
        {
            try
            {
                var reqHeader = Request.Headers.GetValues("connectapitoken");
                string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
                if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
                {
                    OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
                }

                //Call Get Order
                GetOrderReq oGetOrderReq = new GetOrderReq();
                oGetOrderReq.CinemaId = oAddConcessionsReq.CinemaId;
                oGetOrderReq.OptionalClientId = "";
                oGetOrderReq.ProcessOrderValue = true;
                oGetOrderReq.UserSessionId = oAddConcessionsReq.UserSessionId;

                NsGetOrder.GetOrderResp oGetOrderResp = await GetOrderInfo(oGetOrderReq);

                if (oGetOrderResp.Result == 0 && oGetOrderResp.Order.Concessions != null)
                {
                    // Remove Concession(s)
                    ConcessionRemoval concessionRemoveType = null;
                    ConcessionRemoval[] concessionRemoveArray = null;

                    concessionRemoveArray = new ConcessionRemoval[oGetOrderResp.Order.Concessions.Count()];

                    int itemCount = 0;
                    foreach (var concessionItem in oGetOrderResp.Order.Concessions)
                    {
                        concessionRemoveType = new ConcessionRemoval();
                        concessionRemoveType.Id = null;
                        concessionRemoveType.ItemId = concessionItem.ItemId;
                        concessionRemoveType.Quantity = 1;
                        concessionRemoveArray[itemCount] = concessionRemoveType;
                        itemCount++;
                    }





                    RemoveConcessionsReq removeConcessionsReq = new RemoveConcessionsReq();
                    removeConcessionsReq.UserSessionId = oAddConcessionsReq.UserSessionId;
                    removeConcessionsReq.ConcessionRemovals = concessionRemoveArray.ToList();
                    removeConcessionsReq.CinemaId = oAddConcessionsReq.CinemaId;
                    removeConcessionsReq.ReturnOrder = true;
                    removeConcessionsReq.RemoveAllConcessions = true;
                    removeConcessionsReq.OptionalClientId = OptionalClientId;
                    removeConcessionsReq.DeviceDetails = oAddConcessionsReq.DeviceDetails;
                    object oExtRemoveConcessions = await ExtRemoveConcessions(removeConcessionsReq);
                }
                //Call Vista Add Concession

                object oAddConcess = await ExtAddConcessions(oAddConcessionsReq);

                try
                {
                    if (oAddConcess != null)
                    {
                        NsAddConcession.AddConcessionResp oresp = new NsAddConcession.AddConcessionResp();
                        oresp = (NsAddConcession.AddConcessionResp)oAddConcess;

                        string strTicketDescription = string.Empty;

                        List<string> DistinctTicketTypeCodes = new List<string>(); ;

                        foreach (var t in oresp.Order.Sessions[0].Tickets)
                        {
                            DistinctTicketTypeCodes.Add(t.TicketTypeCode);
                        }
                        var da = DistinctTicketTypeCodes.Select(x => x).Distinct();
                        decimal dTotalAmount = Convert.ToDecimal(oresp.Order.TotalValueCents) / 100;

                        foreach (var a in da.ToList())
                        {
                            string Desc = string.Empty;
                            string strSeatNo = string.Empty;
                            Int64 iAmount = 0;
                            int quantity = 0;
                            foreach (var t in oresp.Order.Sessions[0].Tickets)
                            {
                                if (t.TicketTypeCode == a.ToString())
                                {
                                    Desc = t.Description;
                                    strSeatNo += t.SeatData + ",";
                                    iAmount += t.DealPriceCents;
                                    quantity++;
                                }
                            }
                            decimal dAmount = Convert.ToDecimal(iAmount) / 100;
                            strTicketDescription += Desc + "|" + strSeatNo.TrimEnd(',') + "|" + dAmount.ToString() + "|" + quantity.ToString() + "#";
                        }
                        if (oAddConcessionsReq.bookingInfoId != 0)
                        {
                            new BookingInfoDao().UpdateTicketDescription(oAddConcessionsReq.bookingInfoId, strTicketDescription.TrimEnd('#'), dTotalAmount, oAddConcessionsReq.fooddesc);
                        }

                    }
                }
                catch (Exception ex)
                {

                }
                return oAddConcess;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog(oAddConcessionsReq.UserSessionId, "ERROR", "AddConcessionsController", "AddConcessions", JsonConvert.SerializeObject(oAddConcessionsReq), "REQUEST", JsonConvert.SerializeObject(ex), oAddConcessionsReq.DeviceDetails);

                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/CompleteOrderByCard")]
        public async Task<object> CompleteOrderByCard([FromBody] CompleteOrderByCardReq oCompleteOrderByCardReq)
        {
            try
            {



                GetOrderReq oGetOrderReq = new GetOrderReq();
                oGetOrderReq.CinemaId = oCompleteOrderByCardReq.CinemaId;
                oGetOrderReq.OptionalClientId = "";
                oGetOrderReq.ProcessOrderValue = true;
                oGetOrderReq.UserSessionId = oCompleteOrderByCardReq.UserSessionId;

                NsGetOrder.GetOrderResp oGetOrderResp = await GetOrderInfo(oGetOrderReq);
                var reqHeader = Request.Headers.GetValues("connectapitoken");
                VISTAResponse vistaResponse = null;
                object result = null;
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
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oCompleteOrderByCardReq), oCompleteOrderByCardReq.DeviceType, oCompleteOrderByCardReq.UserSessionId, oCompleteOrderByCardReq.SessionId, oCompleteOrderByCardReq.CinemaId, "CompleteOrderByCard", "CompleteOrderByCard", string.Empty, oCompleteOrderByCardReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oCompleteOrderByCardReq), oCompleteOrderByCardReq.DeviceType, oCompleteOrderByCardReq.UserSessionId, oCompleteOrderByCardReq.SessionId, oCompleteOrderByCardReq.CinemaId, "CompleteOrderByCard", "restTicketing.order_payment", string.Empty, oCompleteOrderByCardReq.DeviceDetails);
                }




                #region Vista API Call
                string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
                url = RestTicketingUrl + "/order/payment";
                vistaResponse = await WebRequestForMobile.CreateWebPostRequest<object>(url, reqHeader.ToList()[0].ToString(), oCompleteOrderByCardReq, "CompleteOrderByCard", oCompleteOrderByCardReq.DeviceType, oCompleteOrderByCardReq.UserSessionId);
                result = vistaResponse.Result;
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

                if (vistaResponse.Response.StatusCode.ToString() == "OK")
                {
                    UpdateNewsLetterFlag(oCompleteOrderByCardReq.CustomerEmail, oCompleteOrderByCardReq.isNewsletter, oCompleteOrderByCardReq.isIntrestedForLuckyDraw);
                    new MobileBookingDao().InsertSavedCardInfo(oCompleteOrderByCardReq.UserId, oCompleteOrderByCardReq.PaymentInfo.tokenIdentifier, oCompleteOrderByCardReq.PaymentInfo.CardNumber, oCompleteOrderByCardReq.PaymentInfo.CardType,
                                                                oCompleteOrderByCardReq.PaymentInfo.CardType, int.Parse(oCompleteOrderByCardReq.PaymentInfo.CardExpiryMonth),
                                                                int.Parse(oCompleteOrderByCardReq.PaymentInfo.CardExpiryYear), "", "");
                }

                return result;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog(oCompleteOrderByCardReq.UserSessionId, "ERROR", "CompleteOrderByCardController", "CompleteOrderByCard", JsonConvert.SerializeObject(oCompleteOrderByCardReq), "REQUEST", JsonConvert.SerializeObject(ex), oCompleteOrderByCardReq.DeviceDetails);
                return null;
            }
        }




        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/CompleteOrderByUBECard")]
        public async Task<object> CompleteOrderByUBECard([FromBody] CompleteOrderByUBELoyaltyReq oCompleteOrderByUBELoyaltyReq)
        {

            var reqHeader = Request.Headers.GetValues("connectapitoken");
            string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
            if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
            {
                OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
            }
            VISTAResponse vistaResponse = null;
            object result = null;
            string url = "";
            string ReturnValue = string.Empty;
            ServiceSettings = new DataTable();
            oMobileBookingDao = new MobileBookingDao();
            ServiceSettings = oMobileBookingDao.GetServiceSettings();

            ResultReturnDvo resultObj = null;

            ReelDAO.MobileBookingDao mobileDao = null;
            TransactionDetailsDvo oTransactionDetailsDvo = oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo;
            dynamic completeOrderPaymentByUBECardResp = null;

            long logId = 0;
            long originalRequestLogId = 0;
            try
            {
                Analizer analizer = new Analizer();
                BaseAnalizer baseAnalizer = new BaseAnalizer();
                resultObj = new ResultReturnDvo();
                mobileDao = new ReelDAO.MobileBookingDao();

                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    string currentFunctionName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    originalRequestLogId = 0;
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oTransactionDetailsDvo), oTransactionDetailsDvo.CreatedBy
                        , oTransactionDetailsDvo.UserSessionId, oTransactionDetailsDvo.SessionId, oTransactionDetailsDvo.CinemaId, currentFunctionName
                        , currentFunctionName, string.Empty, oCompleteOrderByUBELoyaltyReq.DeviceDetails);
                }

                //if (Authentication.Username == userName && DecryptAuth(Authentication.Password, transactionDetails.UserSessionId, transactionDetails.CreatedBy) == passWord)
                //{
                //securityToken = ServiceSettings.Rows[0]["SecurityTokenForSmartButtonServices"].ToString();
                //rewardTypeExternalReference = ServiceSettings.Rows[0]["LocationExternalReference"].ToString();
                //locationExternalReference = ServiceSettings.Rows[0]["LocationExternalReference"].ToString();
                double paymentAmount = 0;
                bool isValidPaymentAmount = double.TryParse(oTransactionDetailsDvo.TotalPrice, out paymentAmount);
                if (isValidPaymentAmount)
                {
                    OfferBl oOfferBl = new OfferBl();
                    MemberEligibleChoiceRewardItem oThreshold = oOfferBl.MemberEligibleChoiceRewards(oTransactionDetailsDvo);

                    if (oCompleteOrderByUBELoyaltyReq.LoyaltyPoints >= oThreshold.RewardTypePointCost)
                    {
                        // external payment starting
                        var reqExtpymnt = new
                        {
                            UserSessionId = oTransactionDetailsDvo.UserSessionId,
                            OptionalClientId = OptionalClientId
                        };
                        if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                        {
                            logId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(reqExtpymnt), oTransactionDetailsDvo.CreatedBy
                                , oTransactionDetailsDvo.UserSessionId, oTransactionDetailsDvo.SessionId, oTransactionDetailsDvo.CinemaId, System.Reflection.MethodBase.GetCurrentMethod().Name
                                , "ticketService.ExternalPaymentStarting", string.Empty, oCompleteOrderByUBELoyaltyReq.DeviceDetails);
                        }

                        string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();

                        int _PaymentValueCents = Convert.ToInt16(oCompleteOrderByUBELoyaltyReq.DollarAmountToRedeem * 100);
                        dynamic payment = new { CardNumber = oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.MemberId, CardType = "LOYALTY", PaymentTenderCategory = "LOYALTY", PaymentValueCents = _PaymentValueCents, PaymentSystemId = "-" };

                        dynamic emaarVistaRequest = new
                        {
                            UserSessionId = oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.UserSessionId,
                            PaymentInfo = payment,
                            PerformPayment = false,
                            CustomerEmail = oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.Email,
                            CustomerPhone = oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.MemberId,
                            CustomerName = oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.Name,
                            OptionalClientId = OptionalClientId,
                            BookingNotes = (oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.BookingNotes == true ? "Y" : "N")
                        };



                        url = RestTicketingUrl + "/order/payment";

                        vistaResponse = await WebRequestForMobile.CreateWebPostRequest<object>(url, reqHeader.ToList()[0].ToString(), JsonConvert.SerializeObject(emaarVistaRequest), "CompleteOrderByUBECard", null, null);
                        result = (object)vistaResponse.Result;

                        //respExternalPayStart = ticketService.ExternalPaymentStarting(reqExtpymnt);

                        if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                        {
                            analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(vistaResponse), vistaResponse.Result == null ? "" : vistaResponse.Result.ToString());
                        }

                        //Check the response for ExternalPaymentStarting
                        if (vistaResponse.Response.StatusCode.ToString() == baseAnalizer.OK)
                        {
                            RewardBl oRewardBl = new RewardBl();
                            respIssueVariableReward oRespIssueVariableReward = oRewardBl.IssueVariableReward(oCompleteOrderByUBELoyaltyReq, oTransactionDetailsDvo);

                            if (oRespIssueVariableReward.ReturnCode == 0)
                            {
                                // log into bookingmobile table
                                //resultObj = mobileDao.InsertBookingDetails(transactionDetails);
                                DateTime datetime;
                                bool valid = false;
                                valid = DateTime.TryParse(oTransactionDetailsDvo.MovieDateTime, out datetime);
                                if (valid)
                                {
                                    oTransactionDetailsDvo.MovieDateTime = datetime.ToString("dd-MMM-yyyy HH:mm:ss");
                                }

                                string customerIPAddress = GetUserIpAddress();
                                oTransactionDetailsDvo.CustomerIPAddress = customerIPAddress;


                                // complete order
                                PaymentInfo paymentInfo = new PaymentInfo();
                                paymentInfo.CardNumber = oTransactionDetailsDvo.MemberId;
                                paymentInfo.CardType = "UBERS";
                                paymentInfo.PaymentTenderCategory = "LOYALTY";
                                paymentInfo.PaymentValueCents = Convert.ToInt32(paymentAmount * 100);
                                paymentInfo.PaymentSystemId = ServiceSettings.Rows[0]["PaymentSystemId"].ToString();

                                var completeOrderRequest = new
                                {
                                    CustomerEmail = oTransactionDetailsDvo.Email,
                                    CustomerName = oTransactionDetailsDvo.Name,
                                    CustomerPhone = oTransactionDetailsDvo.Phone,
                                    UserSessionId = oTransactionDetailsDvo.UserSessionId,
                                    PerformPayment = false,
                                    OptionalClientId = OptionalClientId,
                                    PaymentInfo = paymentInfo,
                                    BookingNotes = oTransactionDetailsDvo.BookingNotes ? "Y" : "N"
                                };


                                //completeOrderRequest.BookingNotes = transactionDetails.BookingNotes;
                                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                {
                                    logId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(completeOrderRequest), oTransactionDetailsDvo.CreatedBy
                                        , oTransactionDetailsDvo.UserSessionId, oTransactionDetailsDvo.SessionId, oTransactionDetailsDvo.CinemaId, System.Reflection.MethodBase.GetCurrentMethod().Name
                                        , "ticketService.CompleteOrder", oTransactionDetailsDvo.ReferenceNo, oCompleteOrderByUBELoyaltyReq.DeviceDetails);
                                }

                                //CompleteOrderResponse completeOrderResponse = ticketService.CompleteOrder(completeOrderRequest);

                                RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();

                                url = RestTicketingUrl + "/order/payment";

                                vistaResponse = await WebRequestForMobile.CreateWebPostRequest<object>(url, reqHeader.ToList()[0].ToString(), completeOrderRequest, "CompleteOrderByGiftCard", oTransactionDetailsDvo.DeviceType, oTransactionDetailsDvo.UserSessionId);
                                result = vistaResponse.Result;

                                completeOrderPaymentByUBECardResp = JsonConvert.DeserializeObject<CompleteOrderPaymentByUBECardResp>(result.ToString());
                                completeOrderPaymentByUBECardResp.MobileBookingId = oTransactionDetailsDvo.ReferenceNo;



                                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                {
                                    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(vistaResponse), vistaResponse.Response == null ? "" : vistaResponse.Response.StatusCode.ToString());
                                }

                                if (vistaResponse.Response.StatusCode.ToString() == baseAnalizer.OK)
                                {
                                    //bool IsRedeemed = RedeemPromoCodeResponse(transactionDetails, completeOrderResponse.VistaBookingId);
                                    // payment successful
                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        logId = analizer.InsertServiceLog(0, "request",
                                            ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() + "," + oCompleteOrderByUBELoyaltyReq.LocationExternalReference + "," + oRespIssueVariableReward.MemberIssuedRewardId.ToString()
                                            , oTransactionDetailsDvo.CreatedBy
                                            , oTransactionDetailsDvo.UserSessionId, oTransactionDetailsDvo.SessionId, oTransactionDetailsDvo.CinemaId, System.Reflection.MethodBase.GetCurrentMethod().Name
                                            , "Offer.RedeemMemberRewards", String.Empty, oCompleteOrderByUBELoyaltyReq.DeviceDetails);
                                    }

                                    // redeem points
                                    oOfferBl = new OfferBl();
                                    RedeemMemberRewardsReturn oRedeemMemberRewardsReturn = oOfferBl.RedeemMemberRewards(oCompleteOrderByUBELoyaltyReq.SecurityToken, oCompleteOrderByUBELoyaltyReq.LocationExternalReference, new int[] { oRespIssueVariableReward.MemberIssuedRewardId }, oTransactionDetailsDvo, ServiceSettings);

                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oRedeemMemberRewardsReturn), oRedeemMemberRewardsReturn.ReturnCode.ToString());
                                    }


                                    if (vistaResponse.Response.StatusCode.ToString() == baseAnalizer.OK)
                                    {
                                        new BookingInfoDao().UpdateBookingDetails(oCompleteOrderByUBELoyaltyReq.bookingInfoId, completeOrderPaymentByUBECardResp.VistaBookingId.ToString(), completeOrderPaymentByUBECardResp.VistaBookingNumber.ToString(), completeOrderPaymentByUBECardResp.VistaTransNumber.ToString(), true, "Success");
                                        BookingSummary bookingSummaryDetails = new CommonHelper().GetBookingSummary(oCompleteOrderByUBELoyaltyReq.bookingInfoId, "");
                                        try
                                        {
                                            string smsStatus = new NotificationHelper().SendSMSNotification(oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.SessionId, completeOrderPaymentByUBECardResp.MobileBookingId, "", bookingSummaryDetails.PhoneNo);
                                        }
                                        catch (Exception)
                                        {


                                        }

                                        try
                                        {

                                            string emailStatus = new NotificationHelper().SendNetMail(string.Empty, bookingSummaryDetails.EmailId, ConfigurationManager.AppSettings["MailBCC"].ToString(),
                                                new NotificationHelper().BookingEmailHTML(bookingSummaryDetails), "Reel Cinemas Booking Confirmation", null);
                                        }
                                        catch (Exception)
                                        {

                                        }

                                        UpdateNewsLetterFlag(oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.Email, oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.isNewsletter, oCompleteOrderByUBELoyaltyReq.TransactionDetailsDvo.isIntrestedForLuckyDraw);
                                    }
                                }
                                else
                                {
                                    // payment unsuccessfull
                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        logId = analizer.InsertServiceLog(0, "request",
                                            ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() + "," + oCompleteOrderByUBELoyaltyReq.LocationExternalReference + "," + oRespIssueVariableReward.MemberIssuedRewardId.ToString()
                                            , oTransactionDetailsDvo.CreatedBy
                                            , oTransactionDetailsDvo.UserSessionId, oTransactionDetailsDvo.SessionId, oTransactionDetailsDvo.CinemaId, System.Reflection.MethodBase.GetCurrentMethod().Name
                                            , "Offer.CancelMemberRewards", String.Empty, oCompleteOrderByUBELoyaltyReq.DeviceDetails);
                                    }

                                    oOfferBl = new OfferBl();
                                    CancelMemberRewardsReturn oCancelMemberRewardsReturn = oOfferBl.CancelMemberRewards(oCompleteOrderByUBELoyaltyReq.SecurityToken, new int[] { oRespIssueVariableReward.MemberIssuedRewardId }, oTransactionDetailsDvo, ServiceSettings);

                                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                                    {
                                        analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oCancelMemberRewardsReturn), oCancelMemberRewardsReturn.ReturnCode.ToString());
                                    }

                                    analizer.WriteEventAndExceptions("Transaction Failed in CompleteOrderLoyaltyCard- CompleteOrder method failed for transaction number " + oTransactionDetailsDvo.ReferenceNo, "", "", baseAnalizer.EXCEPTION);
                                }
                            }
                        }
                        else
                        {
                            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                            {
                                //logId = analizer.InsertServiceLog(0, "request",
                                //    ServiceSettings.Rows[0]["SecurityTokenForSmartButtonWS"].ToString() + "," + oCompleteOrderByUBELoyaltyReq.LocationExternalReference + "," + oRespIssueVariableReward.MemberIssuedRewardId.ToString()
                                //    , oTransactionDetailsDvo.CreatedBy
                                //    , oTransactionDetailsDvo.UserSessionId, oTransactionDetailsDvo.SessionId, oTransactionDetailsDvo.CinemaId, System.Reflection.MethodBase.GetCurrentMethod().Name
                                //    , "Offer.CancelMemberRewards", String.Empty);
                            }

                            oOfferBl = new OfferBl();
                            //CancelMemberRewardsReturn oCancelMemberRewardsReturn = oOfferBl.CancelMemberRewards(oCompleteOrderByUBELoyaltyReq.SecurityToken, new int[] { oRespIssueVariableReward.MemberIssuedRewardId }, oTransactionDetailsDvo, ServiceSettings);

                            //if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                            //{
                            //    analizer.UpdateServiceLog(logId, "response", JsonConvert.SerializeObject(oCancelMemberRewardsReturn), oCancelMemberRewardsReturn.ReturnCode.ToString());
                            //}
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "CompleteOrderByUBECardController", "CompleteOrderByUBECard", JsonConvert.SerializeObject(oCompleteOrderByUBELoyaltyReq), "REQUEST", JsonConvert.SerializeObject(ex), oCompleteOrderByUBELoyaltyReq.DeviceDetails);

                resultObj.Success = false;
                resultObj.Iserror = true;
                resultObj.ErrorMsg = ServiceSettings.Rows[0]["CustomErrorMessage"].ToString();
                resultObj.PaymentUrl = ServiceSettings.Rows[0]["PaymentResult"].ToString() + "?Status=Error&ErrorMsg=" + ServiceSettings.Rows[0]["CustomErrorMessage"].ToString();

                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer.UpdateServiceLog(originalRequestLogId, "response", ReturnValue, resultObj.Success.ToString());
                }

                return ReturnValue;
            }
            finally
            {
                baseAnalizer = null;
            }
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                //analizer.UpdateServiceLog(originalRequestLogId, "response", ReturnValue, resultObj.Success.ToString());
            }
            return completeOrderPaymentByUBECardResp;
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/CheckGiftCardBalance")]
        public async Task<object> CheckGiftCardBalance([FromBody] CheckGiftCardBalanceReq oReq)
        {
            var reqHeader = Request.Headers.GetValues("connectapitoken");
            ResultReturnDvo resultObj = null;
            VISTAResponse vistaResponse = null;
            GiftCardBalanceResp result = null;
            string url = "";
            long originalRequestLogId = 0;
            long externalRequestLogId = 0;

            ReelDAO.MobileBookingDao oMobileBookingDao = new MobileBookingDao();

            ServiceSettings = oMobileBookingDao.GetServiceSettings();

            try
            {
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), null, null, null, null, "CheckGiftCardBalance", "restData.CheckGiftCardBalance", string.Empty, oReq.DeviceDetails);
                }

                #region Gift Card Balance Request

                string RestdataUrl = ConfigurationManager.AppSettings["RestdataUrl"].ToString();
                url = RestdataUrl + "/gift-cards/balance/" + oReq.GiftCardNumber;
                vistaResponse = await WebRequestForMobile.CreateWebRequest<GiftCardBalanceResp>(url, reqHeader.ToList()[0].ToString(), "CheckGiftCardBalance", "", null);
                result = (GiftCardBalanceResp)vistaResponse.Result;

                #endregion

                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (vistaResponse.Response != null)
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(vistaResponse), vistaResponse.Response.StatusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(vistaResponse), "");
                    }
                }
                if (result.ResponseCode != 0)
                {
                    var Result = new { Result = 1, ErrorMessage = "Please provide a valid gift card" };
                    return Result;
                }
                return result;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "CheckGiftCardBalanceController", "CheckGiftCardBalance", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);

            }
            return result;
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/CompleteOrderByGiftCard")]
        public async Task<object> CompleteOrderByGiftCard([FromBody] CompleteOrderByGiftCardReq oCompleteOrderByGiftCardReq)
        {
            var reqHeader = Request.Headers.GetValues("connectapitoken");
            string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
            if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
            {
                OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
            }
            ResultReturnDvo resultObj = null;
            VISTAResponse vistaResponse = null;
            dynamic result = null;
            string url = "";
            long originalRequestLogId = 0;
            long externalRequestLogId = 0;

            ReelDAO.MobileBookingDao oMobileBookingDao = new MobileBookingDao();

            ServiceSettings = oMobileBookingDao.GetServiceSettings();

            try
            {
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oCompleteOrderByGiftCardReq), null, null, null, null, "CompleteOrderByGiftCard", "CompleteOrderByGiftCard", string.Empty, oCompleteOrderByGiftCardReq.DeviceDetails);
                }

                TransactionDetailsDvoForGift transactionDetailsDvo = oCompleteOrderByGiftCardReq.TransactionDetailsDvoForGift;

                setVistaOptionalClientId(transactionDetailsDvo.CreatedBy);
                baseAnalizer = new BaseAnalizer();
                oMobileBookingDao = new ReelDAO.MobileBookingDao();
                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oCompleteOrderByGiftCardReq), null, null, null, null, "CompleteOrderByGiftCard", "restTicketing.CompleteOrderByGiftCard", string.Empty, oCompleteOrderByGiftCardReq.DeviceDetails);
                }
                #region Gift Card Balance Request

                string RestdataUrl = ConfigurationManager.AppSettings["RestdataUrl"].ToString();
                url = RestdataUrl + "/gift-cards/balance/" + oCompleteOrderByGiftCardReq.GiftCardNumber;
                vistaResponse = await WebRequestForMobile.CreateWebRequest<GiftCardBalanceResp>(url, reqHeader.ToList()[0].ToString(), "CompleteOrderByGiftCard", "", null);
                result = (GiftCardBalanceResp)vistaResponse.Result;

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
                if (vistaResponse.Response.StatusCode.ToString() == baseAnalizer.OK && result.ResponseCode == 0)
                {
                    try
                    {
                        double cardBalance = 0;
                        bool isValidBalance = double.TryParse(result.BalanceInCents.ToString(), out cardBalance);

                        double paymentAmount = 0;
                        bool isValidPaymentAmount = double.TryParse(transactionDetailsDvo.TotalPrice, out paymentAmount);

                        if (isValidBalance)
                        {
                            cardBalance = cardBalance / 100;
                        }
                        //Gift card balance should be greater than ticket amount
                        if (isValidBalance && isValidPaymentAmount && cardBalance > 0 && cardBalance > paymentAmount)
                        {
                            DateTime datetime;
                            bool valid = false;
                            valid = DateTime.TryParse(transactionDetailsDvo.MovieDateTime, out datetime);
                            if (valid)
                            {
                                transactionDetailsDvo.MovieDateTime = datetime.ToString("dd-MMM-yyyy HH:mm:ss");
                            }
                            string customerIPAddress = GetUserIpAddress();
                            transactionDetailsDvo.CustomerIPAddress = customerIPAddress;


                            #region Complete Order
                            EEG_ReelCinemasRESTAPI.Models.CompleteOrderByGiftCard.PaymentInfo paymentInfo = new Models.CompleteOrderByGiftCard.PaymentInfo();

                            paymentInfo.CardNumber = oCompleteOrderByGiftCardReq.GiftCardNumber;
                            paymentInfo.CardType = "SVS";
                            paymentInfo.PaymentTenderCategory = "SVS";
                            paymentInfo.PaymentValueCents = Convert.ToInt32(paymentAmount * 100);
                            paymentInfo.PaymentSystemId = "-";
                            paymentInfo.BillFullOutstandingAmount = true;
                            paymentInfo.UseAsBookingRef = false;
                            paymentInfo.BillingValueCents = Convert.ToInt32(paymentAmount * 100);
                            paymentInfo.SaveCardToWallet = false;

                            var includedAttachments = new { IncludeApplePassBook = false, IncludeICal = false };

                            var completeOrderRequest = new
                            {
                                UserSessionId = transactionDetailsDvo.UserSessionId,
                                PaymentInfo = paymentInfo,
                                PerformPayment = true,
                                CustomerEmail = transactionDetailsDvo.Email,
                                CustomerPhone = transactionDetailsDvo.Phone,
                                CustomerName = transactionDetailsDvo.Name,
                                UnpaidBooking = false,
                                OptionalReturnMemberBalances = true,
                                BookingMode = 0,
                                PassTypesRequestedForOrder = includedAttachments,
                                UseAlternateLanguage = false,
                                OptionalClientId = OptionalClientId,
                                BookingNotes = transactionDetailsDvo.BookingNotes ? "Y" : "N"
                            };

                            #region Vista API Call
                            string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
                            url = RestTicketingUrl + "/order/payment";
                            vistaResponse = await WebRequestForMobile.CreateWebPostRequest<NsGiftCard.CompleteOrderPaymentByGiftCardResp>(url, reqHeader.ToList()[0].ToString(), completeOrderRequest, "CompleteOrderByGiftCard", transactionDetailsDvo.CreatedBy, transactionDetailsDvo.UserSessionId);
                            result = (NsGiftCard.CompleteOrderPaymentByGiftCardResp)vistaResponse.Result;
                            result.MobileBookingId = resultObj.ResultSet;
                            #endregion

                            if (vistaResponse.Response.StatusCode.ToString() == baseAnalizer.OK)
                            {
                                new BookingInfoDao().UpdateBookingDetails(oCompleteOrderByGiftCardReq.bookingInfoId, result.VistaBookingId.ToString(), result.VistaBookingNumber.ToString(), result.VistaTransNumber.ToString(), true, "Success");
                                BookingSummary bookingSummaryDetails = new CommonHelper().GetBookingSummary(oCompleteOrderByGiftCardReq.bookingInfoId, "");
                                try
                                {
                                    string smsStatus = new NotificationHelper().SendSMSNotification(oCompleteOrderByGiftCardReq.TransactionDetailsDvoForGift.SessionId, result.MobileBookingId, "", bookingSummaryDetails.PhoneNo);
                                }
                                catch (Exception)
                                {


                                }

                                try
                                {

                                    string emailStatus = new NotificationHelper().SendNetMail(string.Empty, bookingSummaryDetails.EmailId, ConfigurationManager.AppSettings["MailBCC"].ToString(),
                                        new NotificationHelper().BookingEmailHTML(bookingSummaryDetails), "Reel Cinemas Booking Confirmation", null);
                                }
                                catch (Exception)
                                {

                                }
                                UpdateNewsLetterFlag(oCompleteOrderByGiftCardReq.TransactionDetailsDvoForGift.Email, oCompleteOrderByGiftCardReq.TransactionDetailsDvoForGift.isNewsletter, oCompleteOrderByGiftCardReq.TransactionDetailsDvoForGift.isIntrestedForLuckyDraw);
                            }
                            #endregion

                        }
                        else
                        {
                            var Result = new { Result = 1, ErrorMessage = "Gift card does not have sufficent balance" };
                          
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                else
                {
                    var Result = new { Result = 1, ErrorMessage = "Please provide a valid gift card" };
                    
                }
                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (vistaResponse.Response != null)
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(vistaResponse), vistaResponse.Response.StatusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(vistaResponse), "");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "CompleteOrderByGiftCardController", "CompleteOrderByGiftCard", JsonConvert.SerializeObject(oCompleteOrderByGiftCardReq), "REQUEST", JsonConvert.SerializeObject(ex), oCompleteOrderByGiftCardReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetBookingInfo")]
        public async Task<object> GetBookingInfo([FromBody] GetBookingInfoReq oGetBookingInfoReq)
        {
            try
            {


                var reqHeader = Request.Headers.GetValues("connectapitoken");
                VISTAResponse vistaResponse = new VISTAResponse();
                object result = null;
                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    baseAnalizer = new BaseAnalizer();
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetBookingInfoReq), null, null, null, oGetBookingInfoReq.CinemaId, "GetBookingInfo", "GetBookingInfo", string.Empty, oGetBookingInfoReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    baseAnalizer = new BaseAnalizer();
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetBookingInfoReq), null, null, null, oGetBookingInfoReq.CinemaId, "GetBookingInfo", "restTicketing.booking", string.Empty, oGetBookingInfoReq.DeviceDetails);
                }

                #region Vista API Call
                string RESTBookingUrl = ConfigurationManager.AppSettings["RESTBookingUrl"].ToString();
                url = RESTBookingUrl + "/booking";
                vistaResponse = await WebRequestForMobile.CreateWebPostRequest<object>(url, reqHeader.ToList()[0].ToString(), oGetBookingInfoReq, "GetBookingInfo", null, null);
                result = vistaResponse.Result;
                #endregion

                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (vistaResponse != null && vistaResponse.Response != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(result), vistaResponse.Response.StatusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(result), "");
                    }
                }

                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (vistaResponse != null && vistaResponse.Response != null)
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
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetBookingInfoController", "GetBookingInfo", JsonConvert.SerializeObject(oGetBookingInfoReq), "REQUEST", JsonConvert.SerializeObject(ex), oGetBookingInfoReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetUserBookingDetails")]
        public async Task<object> GetUserBookingDetails([FromBody] GetUserBookingDetailsReq oGetBookingInfoReq)
        {
            var reqHeader = Request.Headers.GetValues("connectapitoken");
            GetUserBookingDetailsRes influxRes = new GetUserBookingDetailsRes();
            object result = null;
            string url = "";
            long originalRequestLogId = 0;
            long externalRequestLogId = 0;

            oMobileBookingDao = new MobileBookingDao();
            ServiceSettings = oMobileBookingDao.GetServiceSettings();
            try
            {


                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    baseAnalizer = new BaseAnalizer();
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetBookingInfoReq), oGetBookingInfoReq.UserId, null, null, null, "GetUserBookingDetails", "GetUserBookingDetails", string.Empty, oGetBookingInfoReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    baseAnalizer = new BaseAnalizer();
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetBookingInfoReq), oGetBookingInfoReq.UserId, null, null, null, "GetUserBookingDetails", "restTicketing.GetUserBookingDetails", string.Empty, oGetBookingInfoReq.DeviceDetails);
                }

                #region Influx API Call
                string RestdataUrl = ConfigurationManager.AppSettings["InfluxBaseURL"].ToString();
                url = RestdataUrl + "/UserAppAPI/GetUserBookingDetails";
                string response = WebRequestForMobile.WebServicesDoPost(url, JsonConvert.SerializeObject(oGetBookingInfoReq));
                #endregion
                dynamic d = JsonConvert.DeserializeObject(response);
                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (!string.IsNullOrEmpty(response))
                    {

                        analizer.UpdateServiceLog(externalRequestLogId, "response", response, d.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", response, "");
                    }
                }



                if (d.statusCode.ToString() == "1")
                {
                    influxRes = JsonConvert.DeserializeObject<GetUserBookingDetailsRes>(response);
                    if (influxRes.Sourcedata.Length == 0)
                    {
                        influxRes.statusCode = 0;
                        influxRes.statusMessage = "No bookings found.";
                    }
                    else
                    {
                        var sessionlist = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["SessionListJSONPath"]);


                        var oSL = JsonConvert.DeserializeObject<List<SessionListRespData>>(sessionlist);
                        foreach (var item in influxRes.Sourcedata)
                        {

                            string MN = item.MovieName;
                            foreach (var s in oSL)
                            {
                                if (MN == s.MN)
                                {
                                    item.DU = s.DU;
                                }
                            }

                        }

                    }
                }
                else
                {
                    influxRes.statusCode = d.statusCode.ToString();
                    influxRes.statusMessage = d.statusMessage.ToString();
                }
                //Wrapper Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (!string.IsNullOrEmpty(response))
                    {

                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(result), d.statusCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(result), "");
                    }
                }
            }
            catch (Exception ex)
            {
                influxRes.statusCode = 0;
                influxRes.statusMessage = ex.Message;
                new CommonHelper().CreateLog("", "ERROR", "GetUserBookingDetailsController", "GetUserBookingDetails", JsonConvert.SerializeObject(oGetBookingInfoReq), "REQUEST", JsonConvert.SerializeObject(ex), oGetBookingInfoReq.DeviceDetails);

            }
            return influxRes;
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/GetOrder")]
        public async Task<object> GetOrder([FromBody] GetOrderReq oGetOrderReq)
        {
            try
            {
                return await GetOrderInfo(oGetOrderReq);
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetOrderController", "GetOrder", JsonConvert.SerializeObject(oGetOrderReq), "REQUEST", JsonConvert.SerializeObject(ex), oGetOrderReq.DeviceDetails);
                return null;
            }

        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/CancelOrder")]
        public async Task<object> CancelOrder([FromBody] CancelOrderReq oCancelOrderReq)
        {
            try
            {


                var reqHeader = Request.Headers.GetValues("connectapitoken");
                VISTAResponse vistaResponse = null;
                object result = null;
                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oCancelOrderReq), null, null, null, oCancelOrderReq.CinemaId, "CancelOrder", "CancelOrder", string.Empty, oCancelOrderReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oCancelOrderReq), null, null, null, oCancelOrderReq.CinemaId, "CancelOrder", "restTicketing.order/cancel", string.Empty, oCancelOrderReq.DeviceDetails);
                }

                #region Vista API Call
                string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
                url = RestTicketingUrl + "/order/cancel";
                vistaResponse = await WebRequestForMobile.CreateWebPostRequest<object>(url, reqHeader.ToList()[0].ToString(), oCancelOrderReq, "CancelOrder", null, null);
                result = vistaResponse.Result;
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
            catch (Exception ex)
            {
                new CommonHelper().CreateLog(oCancelOrderReq.UserSessionId, "ERROR", "CancelOrderController", "CancelOrder", JsonConvert.SerializeObject(oCancelOrderReq), "REQUEST", JsonConvert.SerializeObject(ex), oCancelOrderReq.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/EarnUByEmaarpoints")]
        public object EarnUByEmaarpoints(EarnUByEmaarpoints oEarnUByEmaarpoints)
        {
            ResultReturnDvo resultObj = null;
            DataTable dtBookingDetails = null;
            ReelDAO.MobileBookingDao mobileDao = null;
            analizer = new Analizer();
            baseAnalizer = new BaseAnalizer();
            resultObj = new ResultReturnDvo();

            oMobileBookingDao = new MobileBookingDao();
            ServiceSettings = oMobileBookingDao.GetServiceSettings();

            try
            {
                mobileDao = new MobileBookingDao();
                string transactionId = oEarnUByEmaarpoints.MemberId.Split('_')[1];
                // dtBookingDetails = mobileDao.GetMobileBookingDetailsByTransactionId(int.Parse(transactionId));

                if (dtBookingDetails.Rows.Count > 0)
                {
                    if (dtBookingDetails.Rows[0]["OfferType"].ToString() == "8")// 8 is for ENBD offer
                    {
                        resultObj.Success = false;
                        resultObj.Iserror = true;
                        resultObj.ErrorMsg = "Earning points failed for ENBD customer";

                        ReturnValue = JsonConvert.SerializeObject(resultObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                        return ReturnValue;
                    }
                }


                //if (oTransactionDetailsDvo.UBEAvailedPoints == null || oTransactionDetailsDvo.UBEAvailedPoints == "")
                //{
                //Get Member Available Points

                TransactionBl oTransactionBl = new TransactionBl();
                IssueTransactionNoBasketReturn oIssueTransactionNoBasketReturn = oTransactionBl.GetIssueTransactionNoBasket(oEarnUByEmaarpoints, dtBookingDetails);

                if (oIssueTransactionNoBasketReturn.ReturnCode == -1)
                {
                    resultObj.Success = false;
                    resultObj.Iserror = true;
                    resultObj.CustomObject = oIssueTransactionNoBasketReturn.MemberPointsAvailable;
                    resultObj.Message = "Invalid Booking Id";
                }
                if (oIssueTransactionNoBasketReturn.ReturnCode == 0)
                {
                    resultObj.Success = true;
                    resultObj.Iserror = false;
                    resultObj.CustomObject = oIssueTransactionNoBasketReturn.MemberPointsAvailable;

                }
                else
                {
                    resultObj.Success = false;
                    resultObj.Iserror = true;
                    resultObj.CustomObject = oIssueTransactionNoBasketReturn.MemberPointsAvailable;
                    switch (oIssueTransactionNoBasketReturn.ReturnCode)
                    {
                        case 5000:
                            resultObj.Message = "No Member Found";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "EarnUByEmaarpointsController", "EarnUByEmaarpoints", JsonConvert.SerializeObject(oEarnUByEmaarpoints), "REQUEST", JsonConvert.SerializeObject(ex), oEarnUByEmaarpoints.DeviceDetails);

            }

            var customResultObj = new
            {
                Success = resultObj.Success,
                Iserror = resultObj.Iserror,
                ErrorMsg = resultObj.ErrorMsg,
                CustomObject = resultObj.CustomObject == null ? null : resultObj.CustomObject,
                Message = resultObj.Message
            };
            return customResultObj;
        }

        [HttpPost]
        [AuthenticateRequest]
        [Route("api/ReelCinemas/EnrollUByEmaarMember")]
        public object EnrollUByEmaarMember(EnrollUByEmaarMember oEnrollUByEmaarMember)
        {
            try
            {
                ResultReturnDvo resultObj = null;
                BookingSummary bResp = new BookingSummary();
                analizer = new Analizer();
                baseAnalizer = new BaseAnalizer();
                resultObj = new ResultReturnDvo();
                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();
                string uBEMemberId = string.Empty;
                DataTable dt = new DataTable();

                try
                {
                    dt = new BookingInfoDao().GetBookingSummary(oEnrollUByEmaarMember.bookingInfoId);
                    if (dt.Rows.Count > 0)
                    {
                        string strResp = JsonConvert.SerializeObject(dt.Rows[0]);
                        dynamic d = JsonConvert.DeserializeObject(strResp);
                        string tJson = JsonConvert.SerializeObject(d.Table[0]);
                        bResp = JsonConvert.DeserializeObject<BookingSummary>(tJson);
                    }
                    var memberDetailsResponse = new UbyEmaarBAL().GetMemberDetails(oEnrollUByEmaarMember.MobileNo, bResp.CinemaId);
                    if (memberDetailsResponse.FirstName == null)
                    {
                        QuickRegisterRequest request = new QuickRegisterRequest();
                        request.BirthDay = 1;
                        request.BirthMonth = 1;
                        request.BirthYear = 1990;
                        request.ClientProfileId = "";
                        request.Email = oEnrollUByEmaarMember.Email;
                        request.ExtensionData = null;
                        request.FirstName = bResp.FirstName;
                        request.LastName = " ";
                        request.Mobile = oEnrollUByEmaarMember.MobileNo;
                        request.Password = "123456";
                        var oResp = new UbyEmaarBAL().RegisterMember(request, bResp.CinemaId);

                    }

                    decimal dAmt = bResp.TotalAmount;
                    int iAmt = Convert.ToInt16(Math.Floor(dAmt));

                    var oEarnPointsResp = new UbyEmaarBAL().Earnpoints(oEnrollUByEmaarMember.MobileNo, iAmt, oEnrollUByEmaarMember.MobileBookingId, bResp.CinemaId);

                    if (oEarnPointsResp.LMSStatusCode == 0 && oEarnPointsResp.Status == "SUCCESS")
                    {
                        resultObj.Success = true;
                        resultObj.Iserror = false;
                        resultObj.CustomObject = oEarnPointsResp.PointsEarned;
                        resultObj.ErrorMsg = "Points earned successfully.";
                    }
                    else
                    {
                        resultObj.Success = false;
                        resultObj.Iserror = true;
                        resultObj.ErrorMsg = ServiceSettings.Rows[0]["UBE-RegistrationErrorMessage"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    new CommonHelper().CreateLog("", "ERROR", "EarnUByEmaarpointsController", "EnrollMemberWithMobilePhoneNumber", JsonConvert.SerializeObject(oEnrollUByEmaarMember), "REQUEST", JsonConvert.SerializeObject(ex), oEnrollUByEmaarMember.DeviceDetails);

                    resultObj.Success = false;
                    resultObj.Iserror = true;
                    resultObj.ErrorMsg = ServiceSettings.Rows[0]["UBE-RegistrationErrorMessage"].ToString();
                    analizer.WriteEventAndExceptions("Exception in ApplyUByEmaarOfferDiscount-Exception " + ex.Message + "Stacktrace-" + ex.StackTrace, oEnrollUByEmaarMember.DeviceType, oEnrollUByEmaarMember.MobileNo, baseAnalizer.EXCEPTION);
                }


                var enrollUByEmaarMember = new { Success = resultObj.Success, Iserror = resultObj.Iserror, ErrorMsg = resultObj.ErrorMsg, CustomObject = resultObj.CustomObject, Message = resultObj.Message };
                return enrollUByEmaarMember;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "EnrollUByEmaarMemberController", "EnrollUByEmaarMember", JsonConvert.SerializeObject(oEnrollUByEmaarMember), "REQUEST", JsonConvert.SerializeObject(ex), oEnrollUByEmaarMember.DeviceDetails);
                return null;
            }
        }

        [HttpPost]
        //[AuthenticateRequest]
        [Route("api/ReelCinemas/ValidateUBEMember")]
        public object ValidateUBEMember([FromBody] ValidateUBEMemberReq oValidateUBEMemberReq)
        {
            respExternalLogin oRespExternalLogin = null;
            long originalRequestLogId = 0;
            long externalRequestLogId = 0;

            oMobileBookingDao = new MobileBookingDao();
            ServiceSettings = oMobileBookingDao.GetServiceSettings();

            //Wrapper Service Request Log
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                analizer = new Analizer();
                originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oValidateUBEMemberReq), null, null, null, null, "ValidateUBEMember", "ValidateUBEMember", string.Empty, oValidateUBEMemberReq.DeviceDetails);
            }

            //Ext Service Request Log
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                analizer = new Analizer();
                externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oValidateUBEMemberReq), null, null, null, null, "ValidateUBEMember", "restTicketing.GetPortalExternalLogin", string.Empty, oValidateUBEMemberReq.DeviceDetails);
            }
            ResponseDvo oResponseDvo = new ResponseDvo();

            string decryptedPassword = string.Empty;
            decryptedPassword = new AuthenticationHelper().AesDecrypt(oValidateUBEMemberReq.UBEMemberPassword);
            oValidateUBEMemberReq.UBEMemberPassword = decryptedPassword;
            try
            {
                baseAnalizer = new BaseAnalizer();
                analizer = new Analizer();
                //Portal Externl Login Info
                PortalBl oPortalBl = new PortalBl();
                oRespExternalLogin = oPortalBl.GetPortalExternalLogin(oValidateUBEMemberReq, ServiceSettings);
                //Ext Service Response Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    if (oRespExternalLogin.ReturnCode != null)
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oRespExternalLogin), oRespExternalLogin.ReturnCode == null ? "" : oRespExternalLogin.ReturnCode.ToString());
                    }
                    else
                    {
                        analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oRespExternalLogin), "");
                    }
                }

                // var oLoginResp = new UbyEmaarBAL().Login(oValidateUBEMemberReq.UBEMemberId, oValidateUBEMemberReq.UBEMemberPassword, "0001", 2);
                double TicketPrice = 0;
                double MemberDirhamsAvailable;
                double MemberPointsRequired = 0.0;
                double MemberDirhamsRequired = 0.0;

                if (oRespExternalLogin != null && oRespExternalLogin.ReturnCode == 0)
                {
                    //Fetch Member Point Balance
                    MemberActivityBl oMemberActivityBl = new MemberActivityBl();
                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        analizer = new Analizer();
                        externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oValidateUBEMemberReq), null, null, null, null, "ValidateUBEMember", "restTicketing.GetFetchMemberPointBalancesReturn", string.Empty, oValidateUBEMemberReq.DeviceDetails);
                    }
                    FetchMemberPointBalancesReturn oFetchMemberPointBalancesReturn = oMemberActivityBl.GetFetchMemberPointBalancesReturn(oValidateUBEMemberReq, ServiceSettings);

                    //  var getUBEPoints = new UbyEmaarBAL().GetPoints(oValidateUBEMemberReq.UBEMemberId, "0001");

                    if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                    {
                        if (oFetchMemberPointBalancesReturn.ReturnCode != null)
                        {
                            analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oFetchMemberPointBalancesReturn), oFetchMemberPointBalancesReturn.ReturnCode == null ? "" : oFetchMemberPointBalancesReturn.ReturnCode.ToString());
                        }
                        else
                        {
                            analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oFetchMemberPointBalancesReturn), "");
                        }
                    }

                    if (oFetchMemberPointBalancesReturn.ReturnCode == 0)
                    {
                        TicketPrice = (double)(oValidateUBEMemberReq.TicketPriceInCents) / 100;
                        MemberDirhamsAvailable = Math.Round((oFetchMemberPointBalancesReturn.MemberPointsAvailable / 10), 2, MidpointRounding.ToEven);
                        if (MemberDirhamsAvailable < TicketPrice)
                        {
                            MemberPointsRequired = (TicketPrice * 10) - oFetchMemberPointBalancesReturn.MemberPointsAvailable;
                            MemberDirhamsRequired = TicketPrice - MemberDirhamsAvailable;
                        }
                        else
                        {
                            MemberPointsRequired = (oValidateUBEMemberReq.TicketPriceInCents * 2);
                            MemberDirhamsRequired = TicketPrice;
                        }

                        oResponseDvo.Result = 0;
                        oResponseDvo.Iserror = false;
                        var fetchMemberPointBalancesReturn = new
                        {
                            MemberPointsAvailable = oFetchMemberPointBalancesReturn.MemberPointsAvailable,
                            MemberDirhamsAvailable = oFetchMemberPointBalancesReturn.MemberPointsAvailable / 10,
                            MemberPointsRequired = MemberPointsRequired,
                            MemberDirhamsRequired = MemberDirhamsRequired,
                        };

                        oResponseDvo.CustomObject = fetchMemberPointBalancesReturn;
                    }
                    else
                    {
                        oResponseDvo.Result = 1;
                        oResponseDvo.Iserror = false;
                        var fetchMemberPointBalancesReturn = new { MemberPointsAvailable = 0 };
                        oResponseDvo.CustomObject = null;
                        switch (oFetchMemberPointBalancesReturn.ReturnCode)
                        {
                            case 1:
                                oResponseDvo.Message = "Undefined Error";
                                break;
                            case 2:
                                oResponseDvo.Message = "Required Field Missing";
                                break;
                            case 100:
                                oResponseDvo.Message = "Invalid Security Token";
                                break;
                            case 101:
                                oResponseDvo.Message = "Insufficient Permission";
                                break;
                            case 102:
                                oResponseDvo.Message = "Security Token Suspended";
                                break;
                            case 5000:
                                oResponseDvo.Message = "No Member Found";
                                break;
                            default:
                                oResponseDvo.Message = "";
                                break;
                        }
                    }
                }
                else
                {
                    oResponseDvo.Result = 1;
                    oResponseDvo.Iserror = false;
                    oResponseDvo.CustomObject = null;
                    oResponseDvo.Message = "Invalid member id / or password";
                }
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "ValidateUBEMemberController", "ValidateUBEMember", JsonConvert.SerializeObject(oValidateUBEMemberReq), "REQUEST", JsonConvert.SerializeObject(ex), oValidateUBEMemberReq.DeviceDetails);

            }
            if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
            {
                analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(oResponseDvo), "");
            }
            return oResponseDvo;
        }

        [HttpPost]
        //[AuthenticateRequest]
        [Route("api/ReelCinemas/GetUBEPointsByTicketPrice")]
        public object GetUBEPointsByTicketPrice([FromBody] GetUBEPointsByTicketPrice oGetUBEPointsByTicketPrice)
        {
            try
            {
                decimal iAmntCents = Convert.ToDecimal(oGetUBEPointsByTicketPrice.TicketPriceInCents) / 100;
                int iAmt = Convert.ToInt16(Math.Floor(iAmntCents));

                var totalAvailPoints = new { MemberPointsAvailable = (iAmt / 2).ToString() };

                return totalAvailPoints;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetUBEPointsByTicketPriceController", "GetUBEPointsByTicketPrice", JsonConvert.SerializeObject(oGetUBEPointsByTicketPrice), "REQUEST", JsonConvert.SerializeObject(ex), oGetUBEPointsByTicketPrice.DeviceDetails);
                return null;
            }
        }

        public void setVistaOptionalClientId(String key)
        {
            if (key.ToLower().Contains("android"))
            {
                //VistaOptionalClientId = "211.111.111.100";
                VistaOptionalClientId = Convert.ToString(ServiceSettings.Rows[0]["VistaOptionalClientIdForAndroid"]);
            }
            else if (key.ToLower().Contains("iphone"))
            {
                //VistaOptionalClientId = "211.111.111.200";
                VistaOptionalClientId = Convert.ToString(ServiceSettings.Rows[0]["VistaOptionalClientIdForIos"]);
            }

        }
        #endregion

        #region Private Function(s)

        private SaveNewCardResp pSaveCard(SaveNewCardReq oReq)
        {
            try
            {
                #region Create Token from Noon Payments
                string NPURL = ConfigurationManager.AppSettings["NoonPaymentCreateTokenURL"].ToString();
                bool isException = false;
                string strExceptionMsg = string.Empty;
                SaveNewCardNPReq SCNPReq = new SaveNewCardNPReq();

                SaveNewCardNPResp oSCNPCResp = new SaveNewCardNPResp();
                SCNPReq.paymentData = new SaveNewCardNPPaymentdata()
                {
                    type = new AuthenticationHelper().AesDecrypt(oReq.cardType),
                    data = new SaveNewCardNPData()
                    {
                        numberPlain = new AuthenticationHelper().AesDecrypt(oReq.cardNumber),
                        cvv = new AuthenticationHelper().AesDecrypt(oReq.cardCVV),
                        expiryMonth = new AuthenticationHelper().AesDecrypt(oReq.expiryMonth),
                        expiryYear = new AuthenticationHelper().AesDecrypt(oReq.expiryYear)
                    }
                };
                SCNPReq.transaction = new SaveNewCardNPTransaction()
                {
                    amount = "1.00",
                    currency = "AED"
                };

                long externalRequestLogId = 0;
                long originalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oReq), oReq.DeviceType, null, null, null, "SaveNewCard", "SaveNewCard", string.Empty, oReq.DeviceDetails);
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(SCNPReq), oReq.DeviceType, null, null, null, "SaveNewCard", "SaveNewCard.CreateToken", string.Empty, oReq.DeviceDetails);
                }

                string strResp = WebRequestForMobile.NoonPaymentWebRequest(NPURL, JsonConvert.SerializeObject(SCNPReq), out isException, out strExceptionMsg);
                int i = 0;
                SaveNewCardResp resp = new SaveNewCardResp();
                if (!isException)
                {
                    dynamic d = JsonConvert.DeserializeObject(strResp);
                    if (d.resultCode.ToString() == "0")
                    {
                        oSCNPCResp = JsonConvert.DeserializeObject<SaveNewCardNPResp>(strResp);

                        var tokenDetails = oSCNPCResp.result.paymentDetails;

                        string strEmailId = (oReq.emailId == null ? "" : oReq.emailId);
                        i = new MobileBookingDao().InsertSavedCardInfo(oReq.UserId, tokenDetails.tokenIdentifier, tokenDetails.paymentInfo, tokenDetails.brand, tokenDetails.cardType, Convert.ToInt16(tokenDetails.expiryMonth), Convert.ToInt16(tokenDetails.expiryYear), oReq.cardName, strEmailId);

                    }
                    else
                    {
                        oSCNPCResp.resultCode = int.Parse(d.resultCode.ToString());
                        oSCNPCResp.message = d.message.ToString();
                    }
                }
                else
                {
                    new CommonHelper().CreateLog("", "ERROR", "SaveCardController", "NoonPaymentWebRequest", JsonConvert.SerializeObject(oReq), "REQUEST", strExceptionMsg, oReq.DeviceDetails);

                    oSCNPCResp.resultCode = 9999;
                    oSCNPCResp.message = strExceptionMsg;
                }

                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(oSCNPCResp), oSCNPCResp.resultCode.ToString());
                }
                #endregion


                if (i > 0)
                {
                    resp.statusCode = 1;
                    resp.statusMessage = "Added Successfully.";
                }
                else
                {
                    resp.statusCode = 0;
                    resp.statusMessage = "Failed to add.";
                }


                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer.UpdateServiceLog(originalRequestLogId, "response", JsonConvert.SerializeObject(resp), resp.statusCode.ToString());
                }
                return resp;

            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "SaveCardController", "SaveCard", JsonConvert.SerializeObject(oReq), "REQUEST", JsonConvert.SerializeObject(ex), oReq.DeviceDetails);
                SaveNewCardResp err = new SaveNewCardResp();
                err.statusCode = 0;
                err.statusMessage = ex.Message;
                return err;
            }
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
                throw exception;
            }
            return ticketInfoList;
        }


        private string UpdateNewsLetterFlag(string EmailId, bool isNewsletter, bool isIntrestedForLuckyDraw)
        {
            try
            {
                int i = new MobileBookingDao().UpdateNewsLetterFlag(EmailId, isNewsletter, isIntrestedForLuckyDraw);
                return i > 0 ? "Success" : "Error";
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "UpdateNewsLetterFlag", "UpdateNewsLetterFlag", "EmailId", "REQUEST", JsonConvert.SerializeObject(ex), null);
                return null;
            }

        }



        private string GetUserIpAddress()
        {
            try
            {


                string stringIpAddress = string.Empty;

                stringIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (stringIpAddress == null) //may be the HTTP_X_FORWARDED_FOR is null
                {
                    stringIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];//we can use REMOTE_ADDR
                }
                return stringIpAddress;
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetUserIpAddress", "GetUserIpAddress", "", "REQUEST", JsonConvert.SerializeObject(ex), null);
                return null;
            }
        }

        private async Task<NsGetOrder.GetOrderResp> GetOrderInfo(GetOrderReq oGetOrderReq)
        {
            try
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
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetOrderReq), null, null, null, oGetOrderReq.CinemaId, "GetOrder", "GetOrder", string.Empty, oGetOrderReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oGetOrderReq), null, null, null, oGetOrderReq.CinemaId, "GetOrder", "restTicketing.order", string.Empty, oGetOrderReq.DeviceDetails);
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
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetOrderInfo", "GetOrderInfo", JsonConvert.SerializeObject(oGetOrderReq), "REQUEST", JsonConvert.SerializeObject(ex), oGetOrderReq.DeviceDetails);
                return null;
            }
        }

        private async Task<List<NsTicketTypes.Ticket>> GetTicketTypes(GetTicketsReq oGetTicketsReq)
        {
            try
            {
                var reqHeader = Request.Headers.GetValues("connectapitoken");
                VISTAResponse vistaResponse = null;
                NsTicketTypes.GetTicketTypesResp result = null;
                string url = "";

                #region Vista API Call
                string RestdataUrl = ConfigurationManager.AppSettings["RestdataUrl"].ToString();
                url = RestdataUrl + "/Cinemas/" + oGetTicketsReq.CinemasId + "/Sessions/" + oGetTicketsReq.SessionId + "/tickets?salesChannelFilter=" + oGetTicketsReq.SalesChannelFilter;
                vistaResponse = await WebRequestForMobile.CreateWebRequest<NsTicketTypes.GetTicketTypesResp>(url, reqHeader.ToList()[0].ToString(), "GetTickets", oGetTicketsReq.DeviceType, null);
                result = (NsTicketTypes.GetTicketTypesResp)vistaResponse.Result;

                if (result.Tickets == null)
                {
                    return null;
                }
                else
                {
                    return result.Tickets.ToList();
                }
                #endregion
            }
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "GetTicketTypes", "GetTicketTypes", JsonConvert.SerializeObject(oGetTicketsReq), "REQUEST", JsonConvert.SerializeObject(ex), oGetTicketsReq.DeviceDetails);
                return null;
            }
        }

        private string ValidateOffer_Influx(ValidateOffersReq oValidateOffersReq, out string errMSG, out bool IsError)
        {
            errMSG = "";
            IsError = false;
            try
            {
                if (!String.IsNullOrEmpty(oValidateOffersReq.Password))
                {
                    string decryptedPassword = string.Empty;
                    try
                    {
                        decryptedPassword = new AuthenticationHelper().AesDecrypt(oValidateOffersReq.Password);
                        oValidateOffersReq.Password = decryptedPassword;
                    }
                    catch (Exception)
                    {
                        errMSG = "Invalid password encryption";
                        IsError = true;
                        return "error";
                    }
                }
                ValidateOffersResponse res = new ValidateOffersResponse();

                #region Influx API Call
                string RestdataUrl = ConfigurationManager.AppSettings["InfluxBaseURL"].ToString();
                string url = RestdataUrl + "/OfferAPPAPI/ValidateOffers";
                string response = WebRequestForMobile.WebServicesDoPost(url, JsonConvert.SerializeObject(oValidateOffersReq));


                return response;
                #endregion

            }
            catch (Exception ex)
            {
                errMSG = ex.Message;
                IsError = true;
                new CommonHelper().CreateLog("", "ERROR", "ValidateOffer_Influx", "ValidateOffer_Influx", JsonConvert.SerializeObject(oValidateOffersReq), "REQUEST", JsonConvert.SerializeObject(ex), oValidateOffersReq.DeviceDetails);
                return null;
            }
        }

        private async Task<NsAddConcession.AddConcessionResp> ExtAddConcessions(AddConcessionsReq oAddConcessionsReq)
        {
            try
            {
                var reqHeader = Request.Headers.GetValues("connectapitoken");
                string OptionalClientId = ConfigurationManager.AppSettings["IOSOptClientID"].ToString();
                if (reqHeader.ToList()[0].ToString() == ConfigurationManager.AppSettings["AndriodAPIToken"].ToString())
                {
                    OptionalClientId = ConfigurationManager.AppSettings["AndriodOptClientID"].ToString();
                }
                VISTAResponse vistaResponse = null;
                NsAddConcession.AddConcessionResp result = null;
                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;
                EEG_ReelCinemasRESTAPI.Models.Concession[] concessionArray = null;
                string[] seatConcessionArray = null;
                EEG_ReelCinemasRESTAPI.Models.Concession concessionType = null;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oAddConcessionsReq), oAddConcessionsReq.DeviceType, oAddConcessionsReq.UserSessionId, null, oAddConcessionsReq.CinemaId.ToString(), "AddConcessions", "AddConcessions", string.Empty, oAddConcessionsReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oAddConcessionsReq), oAddConcessionsReq.DeviceType, oAddConcessionsReq.UserSessionId, null, oAddConcessionsReq.CinemaId.ToString(), "AddConcessions", "restTicketing.order_Addconcessions", string.Empty, oAddConcessionsReq.DeviceDetails);
                }

                string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
                url = RestTicketingUrl + "/order/concessions";


                #region Generate Request

                foreach (var r in oAddConcessionsReq.ConcessionDetail)
                {
                    var oInSeatOrderDeliveryInfo = new
                    {
                        Comment = ServiceSettings.Rows[0]["DeliveryWindowComment"].ToString(), //"Online booking mobile app";
                        DeliveryWindowDescription = ServiceSettings.Rows[0]["DeliveryWindowDescription"].ToString(),//"1st sitting";
                        DeliveryWindowValue = ServiceSettings.Rows[0]["DeliveryWindowValue"].ToString() //"-20";
                    };

                    Deliveryinfo oItemDeliveryInfo = new Deliveryinfo();

                    Seat[] oDeliverySeatArray = new Seat[1];
                    string seatRowId = r.SeatKey.Substring(0, 1);
                    string seatNumber = r.SeatKey.Substring(1);
                    Seat oDeliverySeat = new Seat();
                    oDeliverySeat.RowId = seatRowId;
                    oDeliverySeat.SeatNumber = seatNumber;
                    oDeliverySeatArray[0] = oDeliverySeat;

                    oItemDeliveryInfo.Seats = oDeliverySeatArray;
                    oItemDeliveryInfo.Comment = r.SeatKey;

                    seatConcessionArray = r.ConsessionValue.Split(',');
                    concessionArray = new Models.Concession[seatConcessionArray.Count()];
                    int j = 0;
                    foreach (var consessionItem in seatConcessionArray)
                    {
                        concessionType = new Models.Concession();
                        concessionType.ItemId = consessionItem.ToString();
                        concessionType.Quantity = 1;
                        concessionType.PromoCode = r.PromoCode;
                        concessionArray[j] = concessionType;
                        j++;
                    }

                    var oCustomAddConcessionReq = new
                    {
                        CinemaId = oAddConcessionsReq.CinemaId,
                        ProcessOrderValue = true,
                        Concessions = concessionArray,
                        UserSessionId = oAddConcessionsReq.UserSessionId,
                        OptionalClientId = OptionalClientId,
                        ReturnOrder = true,
                        InSeatOrderDeliveryInfo = oInSeatOrderDeliveryInfo,
                        SessionId = oAddConcessionsReq.SessionId,
                        DeviceType = oAddConcessionsReq.DeviceType,
                        DeliveryInfo = oItemDeliveryInfo
                    };
                    vistaResponse = await WebRequestForMobile.CreateWebPostRequest<NsAddConcession.AddConcessionResp>(url, reqHeader.ToList()[0].ToString(), oCustomAddConcessionReq, "AddConcessions", oAddConcessionsReq.DeviceType, oAddConcessionsReq.UserSessionId);
                    result = (NsAddConcession.AddConcessionResp)vistaResponse.Result;
                }
                #endregion

                #region Vista API Call


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
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "ExtAddConcessions", "ExtAddConcessions", JsonConvert.SerializeObject(oAddConcessionsReq), "REQUEST", JsonConvert.SerializeObject(ex), oAddConcessionsReq.DeviceDetails);
                return null;
            }
        }

        private async Task<object> ExtRemoveConcessions([FromBody] RemoveConcessionsReq oRemoveConcessionsReq)
        {
            try
            {


                var reqHeader = Request.Headers.GetValues("connectapitoken");
                VISTAResponse vistaResponse = null;
                object result = null;
                string url = "";
                long originalRequestLogId = 0;
                long externalRequestLogId = 0;

                oMobileBookingDao = new MobileBookingDao();
                ServiceSettings = oMobileBookingDao.GetServiceSettings();

                //Wrapper Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    originalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oRemoveConcessionsReq), null, oRemoveConcessionsReq.UserSessionId, null, oRemoveConcessionsReq.CinemaId.ToString(), "RemoveConcessions", "RemoveConcessions", string.Empty, oRemoveConcessionsReq.DeviceDetails);
                }

                //Ext Service Request Log
                if (ServiceSettings.Rows[0]["EnableServiceLog"].ToString() == "Y")
                {
                    analizer = new Analizer();
                    externalRequestLogId = analizer.InsertServiceLog(0, "request", JsonConvert.SerializeObject(oRemoveConcessionsReq), null, null, null, oRemoveConcessionsReq.CinemaId.ToString(), "RemoveConcessions", "restTicketing.order_Removeconcessions", string.Empty, oRemoveConcessionsReq.DeviceDetails);
                }

                #region Vista API Call
                string RestTicketingUrl = ConfigurationManager.AppSettings["RestTicketingUrl"].ToString();
                url = RestTicketingUrl + "/order/concessions";
                vistaResponse = await WebRequestForMobile.CreateWebPostRequest<object>(url, reqHeader.ToList()[0].ToString(), oRemoveConcessionsReq, "RemoveConcessions", null, oRemoveConcessionsReq.UserSessionId);
                result = vistaResponse.Result;
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
            catch (Exception ex)
            {
                new CommonHelper().CreateLog("", "ERROR", "ExtRemoveConcessions", "ExtRemoveConcessions", JsonConvert.SerializeObject(oRemoveConcessionsReq), "REQUEST", JsonConvert.SerializeObject(ex), oRemoveConcessionsReq.DeviceDetails);
                return null;
            }
        }

        public static object GetSessions(string connectapitoken)
        {
            object result = null;


            string OdataUrl = ConfigurationManager.AppSettings["OdataUrl"].ToString();

            string url = OdataUrl + "/Sessions?$format=json";

            result = WebRequestForMobile.CreateWebRequest(url, connectapitoken);

            return result;
        }
        #endregion
    }
}