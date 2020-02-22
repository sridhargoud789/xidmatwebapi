using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class GetSavedCardReq
    {
        public int userId { get; set; }
        public int cardId { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
    public class GetSavedCardResp
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }        
        public GetSavedCardSourceData[] Sourcedata { get; set; }
      
    }
    public class GetSavedCardSourceData
    {
        public int id { get; set; }
        public string tokenIdentifier { get; set; }
        public int expiryMonth { get; set; }
        public int expiryYear { get; set; }
        public string brand { get; set; }
        public string cardType { get; set; }
        //public DateTime createdDate { get; set; }
        public string cardNumber { get; set; }
        public string cardName { get; set; }
    }
}