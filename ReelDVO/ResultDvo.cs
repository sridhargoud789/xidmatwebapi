
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ReelDvo
{
    public class ResultDvo
    {
    }

    public class ResponseDvo
    {
        public int Result { get; set; }
        public Object CustomObject { get; set; }
        public bool Iserror { get; set; }
        public string ErrorMsg { get; set; }
        public string Message { get; set; }
    }

    public class ResultReturnDvo
    {
        public bool Success { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ArrayList Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Object CustomObject { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ResultSet { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StackTrace { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataSet Dataset { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<KeyValuePair<string, int>> TicketTypePairs { get; set; }

        public bool Iserror { get; set; }

        public string ErrorMsg { get; set; }

        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentUrl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PostData { get; set; }

        public int NoOfFreeTickets { get; set; }

        public int OrderId { get; set; }

        public string ComplimentaryMessage { get; set; }

        public string TicketTypeCodeList { get; set; }

        public string MovieLanguageSpecificOfferMessage { get; set; }

        // public float VatAmount { get; set; }

        public Object PromotionTicketType { get; set; }
    }

 

    public class PaymentResultReturnDvo
    {
        public string Decision { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
        public string Url { get; set; }
        public string VistaBookingId { get; set; }
        public string VistaBookingNum { get; set; }        
        public string MobileBookingId { get; set; }
        public string MemberPointsAvailable { get; set;}
        public string TransactionPointValue { get; set; }
        public string MovieLanguageSpecificOfferMessage { get; set; } 
    }
}
