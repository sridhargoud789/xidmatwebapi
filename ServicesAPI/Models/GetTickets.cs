using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class GetTicketsReq
    {
        public string CinemasId { get; set; }
        public string SessionId { get; set; }
        public string DeviceType { get; set; }
        public string SalesChannelFilter { get; set; }
        public string OfferedTicketTypes { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
    public class TicketInfo
    {
        public string SeatInfo { get; set; }
        public string Description { get; set; }
        public decimal UnitAmount { get; set; }
        public int Quantity { get; set; }
    }
}