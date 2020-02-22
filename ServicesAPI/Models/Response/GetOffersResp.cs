using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class GetOffersResp
    {

      
            public int statusCode { get; set; }
            public string statusMessage { get; set; }
            public string Experience { get; set; }
            public GetOffersSourcedata[] Sourcedata { get; set; }
            public int BookingTimer { get; set; }
            public int AddExtraTimer { get; set; }
            public int WhenToShowAddTimer { get; set; }
      
    }
    public class GetOffersSourcedata
    {
            public int id { get; set; }
            public string name { get; set; }
            public string title { get; set; }
            public string header { get; set; }
            public string description { get; set; }
            public string imageUrl { get; set; }
            public string termsAndCondition { get; set; }
            public string offerType { get; set; }
            public string validationType { get; set; }
            public string cinemaId { get; set; }
            public string sessionId { get; set; }
            public string binInfo { get; set; }
            public string termsAndConditionUrl { get; set; }
        }
}