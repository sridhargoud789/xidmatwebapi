using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class NoonPaymentCaptureReq
    {
        public string apiOperation { get; set; }
        public NPCReqOrder order { get; set; }
        public NPCReqTransaction transaction { get; set; }
    }

    public class NPCReqOrder
    {
        public long id { get; set; }
    }

    public class NPCReqTransaction
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

}