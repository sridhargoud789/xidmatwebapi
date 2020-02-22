using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models.Response
{
    public class GetNoonPaymentOrderDetails
    {
        public int resultCode { get; set; }
        public string message { get; set; }
        public int resultClass { get; set; }
        public string classDescription { get; set; }
        public string actionHint { get; set; }
        public string requestReference { get; set; }
        public GetNPODResult result { get; set; }
    }

    public class GetNPODResult
    {
        public string nextActions { get; set; }
        public GetNPODTransaction[] transactions { get; set; }
        public GetNPODOrder order { get; set; }
        public GetNPODConfiguration configuration { get; set; }
        public GetNPODCvv cvv { get; set; }
        public GetNPODAvs avs { get; set; }
        public GetNPODMerchant merchant { get; set; }
        public GetNPODBusiness business { get; set; }
        public GetNPODEvent[] events { get; set; }
        public GetNPODFraudcheck fraudCheck { get; set; }
        public GetNPODDevicefingerprint deviceFingerPrint { get; set; }
        public GetNPODPayerauthentication payerAuthentication { get; set; }
        public GetNPODPaymentdetails paymentDetails { get; set; }
    }

    public class GetNPODOrder
    {
        public string status { get; set; }
        public string errorMessage { get; set; }
        public DateTime creationTime { get; set; }
        public float totalAuthorizedAmount { get; set; }
        public float totalCapturedAmount { get; set; }
        public float totalRefundedAmount { get; set; }
        public float totalRemainingAmount { get; set; }
        public float totalReversedAmount { get; set; }
        public float totalSalesAmount { get; set; }
        public long id { get; set; }
        public float amount { get; set; }
        public string currency { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string channel { get; set; }
    }

    public class GetNPODConfiguration
    {
        public bool tokenizeCc { get; set; }
        public string returnUrl { get; set; }
        public string locale { get; set; }
        public DateTime initiationValidity { get; set; }
        public string paymentAction { get; set; }
    }

    public class GetNPODCvv
    {
        public string status { get; set; }
    }

    public class GetNPODAvs
    {
        public string status { get; set; }
    }

    public class GetNPODMerchant
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class GetNPODBusiness
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class GetNPODFraudcheck
    {
        public GetNPODInternal _internal { get; set; }
    }

    public class GetNPODInternal
    {
        public string result { get; set; }
        public string screeningResponse { get; set; }
    }

    public class GetNPODDevicefingerprint
    {
        public string sessionId { get; set; }
    }

    public class GetNPODPayerauthentication
    {
        public GetNPODEnrollment enrollment { get; set; }
        public GetNPODValidation validation { get; set; }
    }

    public class GetNPODEnrollment
    {
        public string status { get; set; }
        public string statusDescription { get; set; }
        public string xid { get; set; }
    }

    public class GetNPODValidation
    {
        public string cavv { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
        public string eci { get; set; }
        public string commerceIndicator { get; set; }
        public string xid { get; set; }
    }

    public class GetNPODPaymentdetails
    {
        public string instrument { get; set; }
        public string tokenIdentifier { get; set; }
        public string mode { get; set; }
        public string integratorAccount { get; set; }
        public string paymentInfo { get; set; }
        public string paymentMechanism { get; set; }
        public string brand { get; set; }
        public string scheme { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string isNetworkToken { get; set; }
        public string cardType { get; set; }
        public string cardCountry { get; set; }
        public string cardCountryName { get; set; }
    }

    public class GetNPODTransaction
    {
        public string type { get; set; }
        public string authorizationCode { get; set; }
        public DateTime creationTime { get; set; }
        public string status { get; set; }
        public string stan { get; set; }
        public string id { get; set; }
        public float amount { get; set; }
        public string currency { get; set; }
        public float amountRefunded { get; set; }
        public string targetTransactionId { get; set; }
    }

    public class GetNPODEvent
    {
        public DateTime creationTime { get; set; }
        public string apiOperation { get; set; }
        public string result { get; set; }
        public string requestReference { get; set; }
        public int durationInMs { get; set; }
    }

}