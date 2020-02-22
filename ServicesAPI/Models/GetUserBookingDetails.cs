using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class GetUserBookingDetailsReq
    {
        public string UserId { get; set; }
        public string DataFrom { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }



    public class GetUserBookingDetailsRes
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public object SourceUrl { get; set; }
        public GetUserBookingDetailsResSourcedata[] Sourcedata { get; set; }
    }

    public class GetUserBookingDetailsResSourcedata
    {
        public string Experience { get; set; }
        public float OfferAmount { get; set; }
        public string MovieName { get; set; }
        public string Rating { get; set; }
        public string CinemaName { get; set; }
        public string DU { get; set; }
        public DateTime Showdate { get; set; }
        public object ShowdateDisplay { get; set; }
        public string Showtime { get; set; }
        public object ShowtimeDisplay { get; set; }
        public float TicketAmount { get; set; }
        public float SubTotalAmount { get; set; }
        public float FoodAmount { get; set; }
        public float VatAmount { get; set; }
        public float TotalAmount { get; set; }
        public string BookingId { get; set; }
        public string QRCodeUrl { get; set; }
        public string ScreenName { get; set; }
        public object FirstName { get; set; }
        public object LastName { get; set; }
        public object EmailId { get; set; }
        public object PhoneNo { get; set; }
        public string Flag { get; set; }
        public string TicketDescription { get; set; }
        public object[] FoodDetailList { get; set; }
        public Ticketdetaillist[] TicketDetailList { get; set; }
    }

    public class Ticketdetaillist
    {
        public string SeatInfo { get; set; }
        public string Description { get; set; }
        public float UnitAmount { get; set; }
        public int Quantity { get; set; }
    }


}

