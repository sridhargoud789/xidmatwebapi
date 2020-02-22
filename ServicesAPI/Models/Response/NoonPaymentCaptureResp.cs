using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models.Response
{
   

    public class NoonPaymentCaptureResp
    {
        public int resultCode { get; set; }
        public string message { get; set; }
        public int resultClass { get; set; }
        public string classDescription { get; set; }
        public string actionHint { get; set; }
        public string requestReference { get; set; }
        public NPCRespResult result { get; set; }
    }

    public class NPCRespResult
    {
        public NPCRespOrder order { get; set; }
        public NPCRespPayerauthentication payerAuthentication { get; set; }
        public NPCRespPaymentdetails paymentDetails { get; set; }
    }

    public class NPCRespOrder
    {
        public string status { get; set; }
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

    public class NPCRespPayerauthentication
    {
        public NPCRespEnrollment enrollment { get; set; }
        public NPCRespValidation validation { get; set; }
    }

    public class NPCRespEnrollment
    {
        public string status { get; set; }
        public string statusDescription { get; set; }
        public string xid { get; set; }
    }

    public class NPCRespValidation
    {
        public string cavv { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
        public string eci { get; set; }
        public string commerceIndicator { get; set; }
        public string xid { get; set; }
    }

    public class NPCRespPaymentdetails
    {
        public string tokenIdentifier { get; set; }
        public string instrument { get; set; }
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

}