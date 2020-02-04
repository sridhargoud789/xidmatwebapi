using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelDVO
{
    public class InsertBookingInfo
    {
        public long userId { get; set; }
        public string userSessionId { get; set; }
        public string movieId { get; set; }
        public string movieName { get; set; }
        public string genre { get; set; }
        public string language { get; set; }
        public string cinemaId { get; set; }
        public string cinemaName { get; set; }
        public string screenName { get; set; }
        public string screenNo { get; set; }
        public string experience { get; set; }
        public string rating { get; set; }
        public string sessionId { get; set; }
        public int noOfSeats { get; set; }
        public DateTime showdate { get; set; }
        public DateTime showtime { get; set; }
        public decimal ticketAmount { get; set; }
        public decimal totalAmount { get; set; }
        public string seatInfo { get; set; }
        public string ticketTypeCode { get; set; }
        public string ticketDescription { get; set; }
        public DateTime bookingdate { get; set; }
        public int offerId { get; set; }
        public int offerQuantity { get; set; }
        public string promoCode { get; set; }
        public string voucherCode { get; set; }
        public decimal VatAmount { get; set; }
        public string foodDesc { get; set; }
        public decimal foodAmount { get; set; }
        public string memberIdOrEmailId { get; set; }
        public decimal savings { get; set; }
        public string offerDescription { get; set; }

    }

    public class InsertNoonPaymentResp
    {
        public long bookingInfoId { get; set; }
        public int resultCode { get; set; }
        public string message { get; set; }
        public string requestReference { get; set; }
        public string authorizationCode { get; set; }
        public string transactionStatus { get; set; }
        public string reasonCode { get; set; }
        public float amount { get; set; }
        public long orderId { get; set; }
        public string orderStatus { get; set; }
        public string tokenIdentifier { get; set; }
        public string paymentMechanism { get; set; }
        public string paymentInfo { get; set; }
        public string brand { get; set; }
        public string cardType { get; set; }
        public int expiryMonth { get; set; }
        public int expiryYear { get; set; }
        public string App_Source { get; set; }

    }
}
