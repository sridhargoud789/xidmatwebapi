﻿using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class CheckGiftCardBalanceReq
    {
        public string GiftCardNumber { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
}