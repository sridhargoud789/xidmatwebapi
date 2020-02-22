using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class BookingSummary
    {
        public long BookingInfoId { get; set; }
        public string Experience { get; set; }
        public string ExperienceName { get; set; }
        public decimal OfferAmount { get; set; }
        public string MovieName { get; set; }
        public string MovieId { get; set; }
        public string Rating { get; set; }
        public string OfferName { get; set; }
        public decimal? Savings { get; set; }
        public string CinemaId { get; set; }
        public string CinemaName { get; set; }
        public DateTime Showdate { get; set; }
        public string ShowdateDisplay { get; set; }
        public DateTime Showtime { get; set; }
        public string ShowtimeDisplay { get; set; }
        public decimal TicketAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingId { get; set; }

        public string QRCodeUrl { get; set; }
        public string ScreenName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public bool PGStatus { get; set; }
        public bool VistaStatus { get; set; }
        public bool Emailstatus { get; set; }
        public bool SMSStatus { get; set; }
        public string EmailTemplate { get; set; }
        public string OfferMessage { get; set; }
        public string CardNumber { get; set; }
        public bool? IsEnablePoints { get; set; }
        public decimal? EarnPoints { get; set; }
        public bool? IsNewsletter { get; set; }

        public string AddCalendarUrl { get; set; }
        public string LocationPageUrl { get; set; }
        public string AddWalletUrl { get; set; }

        public List<FoodInfo> FoodInfoList { get; set; }
        public List<TicketInfo> TicketInfoList { get; set; }
    }
    public class FoodInfo
    {
        public string SeatInfo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitAmount { get; set; }
        public int Quantity { get; set; }
    }
}