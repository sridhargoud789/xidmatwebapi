using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class AddWalletReq
    {
        public string bookingId { get; set; }
        public string cinemaName { get; set; }
        public string movieName { get; set; }
        public DateTime showtime { get; set; }
        public string screenName { get; set; }
        public string experience { get; set; }
        public string ticketDescription { get; set; }
        public string movieId { get; set; }
        public string rating { get; set; }
        public Decimal totalAmount { get; set; }
        public string UserSessionId { get; set; }
        public string CinemaId { get; set; }
        public string SessionId { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
    public class AddWalletRes
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public string FileBase64String { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }
    }
}