using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class AddTicketReq
    {
        public string OptionalClientId { get; set; }
        public string UserSessionId { get; set; }
        public string CinemaId { get; set; }
        public string SessionId { get; set; }

        public int OfferId { get; set; }
        public List<TicketType> TicketTypes { get; set; }
        public bool ReturnOrder { get; set; }
        public bool ReturnSeatData { get; set; }
        public bool ProcessOrderValue { get; set; }
        public bool UserSelectedSeatingSupported { get; set; }
        public bool SkipAutoAllocation { get; set; }
        public string DeviceType { get; set; }
        //public List<TicketDiscounts> TicketDiscounts { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }

    public class TicketType
    {
        public string TicketTypeCode { get; set; }
        public int Qty { get; set; }
        public string PriceInCents { get; set; }
        public string OptionalBarcode { get; set; }
        public string OptionalBarcodePin { get; set; }
        public string DiscountCode { get; set; }
    }
    public class TicketDiscounts
    {
        //public int PackageId { get; set; }
        //public string SessionId { get; set; }
        public int ItemId { get; set; }

        public string DiscountCode { get; set; }
    }
}