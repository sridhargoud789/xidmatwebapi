using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class SaveNewCardReq
    {
        public int UserId { get; set; }        
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string brand { get; set; }
        public string cardType { get; set; }
        public string cardNumber { get; set; }
        public string cardCVV { get; set; }
        public string cardName { get; set; }
        public string emailId { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
    public class SaveNewCardResp
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }

    }


    public class SaveNewCardNPReq
    {
        public SaveNewCardNPPaymentdata paymentData { get; set; }
        public SaveNewCardNPTransaction transaction { get; set; }
    }

    public class SaveNewCardNPPaymentdata
    {
        public string type { get; set; }
        public SaveNewCardNPData data { get; set; }
    }

    public class SaveNewCardNPData
    {
        public string numberPlain { get; set; }
        public string cvv { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
    }

    public class SaveNewCardNPTransaction
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }





    public class SaveNewCardNPResp
    {
        public int resultCode { get; set; }
        public string message { get; set; }
        public int resultClass { get; set; }
        public string classDescription { get; set; }
        public string actionHint { get; set; }
        public string requestReference { get; set; }
        public SaveNewCardNPRespResult result { get; set; }
    }

    public class SaveNewCardNPRespResult
    {
        public SaveNewCardNPRespOrder order { get; set; }
        public SaveNewCardNPRespPaymentdetails paymentDetails { get; set; }
    }

    public class SaveNewCardNPRespOrder
    {
        public string status { get; set; }
        public long id { get; set; }
        public float amount { get; set; }
        public string currency { get; set; }
    }

    public class SaveNewCardNPRespPaymentdetails
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