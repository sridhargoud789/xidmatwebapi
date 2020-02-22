using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class DeleteSavedCardReq
    {
        public int cardId { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }

    public class DeleteSavedCardResp
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }       

    }
}