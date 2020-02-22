using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class SetSeatReq
    {
        public string OptionalClientId { get; set; }
        public string UserSessionId { get; set; }
        public List<SelectedSeat> SelectedSeats { get; set; }
        public string CinemaId { get; set; }
        public string SessionId { get; set; }
        public bool ReturnOrder { get; set; }
        public int bookingInfoId { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }

    public class SelectedSeat
    {
        public string AreaCategoryCode { get; set; }
        public string AreaNumber { get; set; }
        public string ColumnIndex { get; set; }
        public string RowIndex { get; set; }
        public string SeatNo { get; set; }
    }
}