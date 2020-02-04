using EEG_ReelCinemasRESTAPI.Models;
using EEG_ReelCinemasRESTAPI.UByEmaar;
using Newtonsoft.Json;
using ReelDAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI
{
    public class UbyEmaarBAL
    {
        //Get available Point
        public GetPointsResponse GetPoints(string memberId, string cinemaId)
        {
            //declaration

            GetPointsResponse response = new GetPointsResponse();
            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { memberId = memberId, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.GetPoints", string.Empty);
            try
            {
                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();
                    GetPointsRequest request = new GetPointsRequest();
                    request.ClientProfileId = config.ClientProfileId;
                    request.MemberId = memberId;
                    //Call GetPoints endpoint 
                    response = client.GetPoints(request);
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");
                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog(memberId, "ERROR", "UbyEmaarBAL", "GetPoints", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }
        
        //Login U BY Emaar 
        public LoginResponse Login(string memberId, string password, string cinemaId, int dataFrom)
        {
            //declaration

            LoginResponse response = new LoginResponse();

            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { memberId = memberId, password = password, dataFrom = dataFrom, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.Login", string.Empty);

            try
            {

                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();

                    LoginRequest request = new LoginRequest();
                    request.ClientProfileId = config.ClientProfileId;
                    request.MemberId = memberId;
                    request.Password = password;
                    //Call Login endpoint 

                    response = client.ExternalLogin(request);

                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");

                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog(memberId, "ERROR", "UbyEmaarBAL", "Login", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }

        //Login U BY Emaar using OTP
        public SendOTPResponse LoginByOTP(string memberId, string cinemaId)
        {
            //declaration

            SendOTPResponse response = new SendOTPResponse();

            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { memberId = memberId, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.LoginByOTP", string.Empty);
            try
            {
                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();
                    SendOTPRequest request = new SendOTPRequest();
                    request.ClientProfileId = config.ClientProfileId;
                    request.MemberId = memberId;
                    //Call Login endpoint 
                    response = client.SendOTP(request);
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");
                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog(memberId, "ERROR", "UbyEmaarBAL", "LoginByOTP", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }
        //Validate OTP
        public ValidateOTPResponse ValidateOTP(string memberId, string otp, string resultKey, string cinemaId, int dataFrom)
        {
            //declaration
            ValidateOTPResponse response = new ValidateOTPResponse();

            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { memberId = memberId, otp = otp, resultKey = resultKey, dataFrom = dataFrom, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.ValidateOTP", string.Empty);

            try
            {
                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();
                    ValidateOTPRequest request = new ValidateOTPRequest();
                    request.ClientProfileId = config.ClientProfileId;
                    request.MemberId = memberId;
                    request.OTPKey = otp;
                    request.OTPValue = resultKey;
                    //Call Login endpoint 
                    response = client.ValidateOTP(request);
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");
                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog(memberId, "ERROR", "UbyEmaarBAL", "ValidateOTP", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }

        //Earn Points
        public EarnPointsResponse Earnpoints(string memberId, double TransactionPoints, string TransactionRef, string cinemaId)
        {
            //declaration
            EarnPointsResponse response = new EarnPointsResponse();

            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { memberId = memberId, TransactionPoints = TransactionPoints, TransactionRef = TransactionRef, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.Earnpoints", string.Empty);
            try
            {
                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();

                    EarnPointsRequest request = new EarnPointsRequest();
                    request.ClientProfileId = config.ClientProfileId;
                    request.MemberId = memberId;
                    request.TransactionAmount = TransactionPoints;
                    request.TransactionRef = TransactionRef;
                    //Call Login endpoint 
                    response = client.EarnPoints(request);
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");
                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog(memberId, "ERROR", "UbyEmaarBAL", "LoginByOTP", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }

        //Block U By Emaar Points
        public BlockPointsResponse BlockLoyatlyPoints(string memberId, double Amount, string cinemaId)
        {
            BlockPointsResponse response = new BlockPointsResponse();

            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { memberId = memberId, Amount = Amount, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.BlockLoyatlyPoints", string.Empty);
            try
            {
                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();

                    BlockPointsRequest request = new BlockPointsRequest();
                    request.ClientProfileId = config.ClientProfileId;
                    request.MemberId = memberId;
                    request.TransactionAmount = Amount;

                    //Call BlockPoint endpoint 
                    response = client.BlockPoints(request);
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");
                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog(memberId, "ERROR", "UbyEmaarBAL", "BlockLoyatlyPoints", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }
        //Redeem U By Emaar Points
        public RedeemPointsResponse RedeemPoints(string memberId, int LMSIssuedRewardId, string TransactionRef, string cinemaId)
        {
            RedeemPointsResponse response = new RedeemPointsResponse();

            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { memberId = memberId, LMSIssuedRewardId = LMSIssuedRewardId, TransactionRef = TransactionRef, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.RedeemPoints", string.Empty);
            try
            {
                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();

                    RedeemPointsRequest request = new RedeemPointsRequest();
                    request.ClientProfileId = config.ClientProfileId;
                    request.MemberId = memberId;
                    request.LMSIssuedRewardId = LMSIssuedRewardId;
                    request.TransactionRef = TransactionRef;
                    //Call Redeem endpoint 
                    response = client.RedeemPoints(request);
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");
                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog(memberId, "ERROR", "UbyEmaarBAL", "RedeemPoints", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }


        //Refund U By Emaar Points
        public PointAdjustmentResponse RefundPoints(string memberId, double transactionPoints, string transactionRef, string cinemaId)
        {
            //declaration
            PointAdjustmentResponse response = new PointAdjustmentResponse();

            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { memberId = memberId, transactionPoints = transactionPoints, transactionRef = transactionRef, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.RefundPoints", string.Empty);
            try
            {
                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();

                    PointAdjustmentRequest request = new PointAdjustmentRequest();
                    request.ClientProfileId = config.ClientProfileId;
                    request.MemberId = memberId;
                    request.TransactionPoints = transactionPoints;
                    request.TransactionRef = transactionRef;
                    //Call Redeem endpoint 
                    response = client.AdjustPoints(request);
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");
                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog(memberId, "ERROR", "UbyEmaarBAL", "RefundPoints", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }
        //Register as U by Emaar Member
        public QuickRegisterResponse RegisterMember(QuickRegisterRequest request, string cinemaId)
        {
            //declaration
            QuickRegisterResponse response = new QuickRegisterResponse();

            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { request = request, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.RegisterMember", string.Empty);

            try
            {
                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();
                    
                    request.ClientProfileId = config.ClientProfileId;
                    //Call Redeem endpoint 
                    response = client.QuickRegister(request);
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");
                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog("", "ERROR", "UbyEmaarBAL", "RegisterMember", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }

        //Register as U by Emaar Member
        public MemberDetailsResponse GetMemberDetails(string memberId, string cinemaId)
        {
            //declaration
            MemberDetailsResponse response = new MemberDetailsResponse();

            long externalRequestLogId = 0;
            Analizer analizer = new Analizer();
            string strReq = JsonConvert.SerializeObject(new { memberId = memberId, cinemaId = cinemaId });
            externalRequestLogId = analizer.InsertServiceLog(0, "request", strReq, null, null, null, cinemaId, "UbyEmaar", "restData.GetMemberDetails", string.Empty);
            try
            {
                using (ServiceClient client = new ServiceClient())
                {
                    //Read UbyEmaar Client App Settings
                    List<UbyEmaarConfig> configList = JsonConvert.DeserializeObject<List<UbyEmaarConfig>>(new CommonHelper().ReadJsonFile("UbyEmaar"));
                    UbyEmaarConfig config = configList.Where(x => x.CinemaId == cinemaId).FirstOrDefault();
                    MemberDetailsRequest memberDetailsRequest = new MemberDetailsRequest();
                    memberDetailsRequest.MemberId = memberId;
                    memberDetailsRequest.ClientProfileId = config.ClientProfileId;

                    //Call GetMemberDetails endpoint 
                    response = client.GetMemberDetails(memberDetailsRequest);
                    analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(response), "");
                }
            }
            catch (Exception ex)
            {
                analizer.UpdateServiceLog(externalRequestLogId, "response", JsonConvert.SerializeObject(ex), "");
                new CommonHelper().CreateLog(memberId, "ERROR", "UbyEmaarBAL", "GetMemberDetails", strReq, "REQUEST", JsonConvert.SerializeObject(ex), null);
            }
            return response;
        }
    }
}