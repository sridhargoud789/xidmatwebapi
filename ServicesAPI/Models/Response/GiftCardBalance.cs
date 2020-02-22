using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models.Response
{
    public class GiftCardBalanceResp
    {
        public int BalanceInCents { get; set; }
        public string CardExpiry { get; set; }
        public string CardNumber { get; set; }
        public int ResponseCode { get; set; }
    }
}