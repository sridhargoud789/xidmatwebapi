using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelDVO
{
    public class TransactionDetailsDvoForGift
    {
        public string OptionalClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MemberId { get; set; }
        public string Phone { get; set; }
        public string CinemaId { get; set; }
        public string CinemaName { get; set; }
        public string MovieName { get; set; }
        public string MovieDateTime { get; set; }
        public string TicketsQuantity { get; set; }
        public string TicketType { get; set; }
        public string TicketPrice { get; set; }
        public string TotalTicketPrice { get; set; }
        public string PackageTicket { get; set; }
        public string PackageConcession { get; set; }
        public string SeatList { get; set; }
        public string BookingFee { get; set; }
        public string TotalPrice { get; set; }
        public string TransactionType { get; set; }
        public string ReleaseReason { get; set; }
        public string CreatedBy { get; set; }
        public string UserSessionId { get; set; }
        public string SessionId { get; set; }
        public string Nationality { get; set; }
        public decimal VATPercent { get; set; }
        public decimal VATPrice { get; set; }
        public string ItemType { get; set; }
        public string ItemPrice { get; set; }
        public string ItemDesc { get; set; }
        public string ItemQty { get; set; }
        public string TotalItemPrice { get; set; }
        public string ScreenName { get; set; }
        public string offerType { get; set; }
        public bool IsDolbyPromotionApplicable { get; set; }
        //public bool IsCaptainMarvel { get; set; }
        public bool IsComplimentarySelected { get; set; }
        public string OrderId { get; set; }
        public string CustomerIPAddress { get; set; }
        public bool BookingNotes { get; set; }
        public string ADEmail { get; set; }
        public string MovieLanguage { get; set; }
        public string MovieRating { get; set; }
        public string MovieGenre { get; set; }
        public int FreeTicketQty { get; set; }


        public bool isNewsletter { get; set; }
        public bool isIntrestedForLuckyDraw { get; set; }
    }

    public class TransactionDetailsDvo
    {
        public string MobileBookingId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MemberId { get; set; }
        public string Phone { get; set; }
        public string CinemaId { get; set; }
        public string CinemaName { get; set; }
        public string MovieName { get; set; }
        public string MovieDateTime { get; set; }
        public string TicketsQuantity { get; set; }

        public string VistaBookingId { get; set; }
        public string VistaBookingNumber { get; set; }
        public string TicketType { get; set; }
        public string TicketPrice { get; set; }
        public string TotalTicketPrice { get; set; }
        public string PackageTicket { get; set; }
        public string PackageConcession { get; set; }
        public string SeatList { get; set; }
        public string BookingFee { get; set; }
        public string TotalPrice { get; set; }
        public DateTime SignedDateTime { get; set; }
        public string AuthorizedDateTime { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
        public string MerchantProfileID { get; set; }
        public string ReferenceNo { get; set; }
        public string AuthorizedCode { get; set; }
        public string AuthorizedTransRefNo { get; set; }
        public int AuthFailureCount { get; set; }
        public DateTime TicketDate { get; set; }
        public string Reqaccesskey { get; set; }
        public string Reqprofileid { get; set; }
        public string Reasoncode { get; set; }
        public string Authavscode { get; set; }
        public string Authresponse { get; set; }
        public string Authcvresult { get; set; }
        public string Authtransrefno { get; set; }
        public string CTransactionNO { get; set; }
        public string Settlement { get; set; }
        public string MerchantRequestID { get; set; }
        public string MerchantID { get; set; }
        public string ReleaseReason { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsCancelled { get; set; }
        public string UserSessionId { get; set; }
        public string SessionId { get; set; }
        public string VistaTransNumber { get; set; }
        public int RetryCount { get; set; }
        public string ReasonofException { get; set; }

        public string Surname { get; set; }
        public string Addressline1 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public Double TransactionAmount { get; set; }
        public string Nationality { get; set; }
        public string CreditCardNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsPaymentError { get; set; }
        public bool isVistaError { get; set; }
        public string CreditCardExpiryMonth { get; set; }
        public string CreditCardExpiryYear { get; set; }
        public string CreditCardCvNumber { get; set; }


        public string PaymentsettingDetails { get; set; }
        public string Url { get; set; }
        public string Language { get; set; }
        public string Currency { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public bool IstestTransaction { get; set; }

        public string Postalcode { get; set; }
        public string State { get; set; }

        public string CallId { get; set; }
        public string EncKey { get; set; }
        public string EncPaymentData { get; set; }

        public bool IsPromotionalTicket { get; set; }
        public bool IsUByEmaarOfferSelected { get; set; }

        public string DeviceType { get; set; }
        public decimal VATPercent { get; set; }
        public decimal VATPrice { get; set; }

        public string ItemType { get; set; }
        public string ItemPrice { get; set; }
        public string ItemDesc { get; set; }
        public string ItemQty { get; set; }
        public string TotalItemPrice { get; set; }

        public string ScreenName { get; set; }
        public string offerType { get; set; }

        public bool IsDolbyPromotionApplicable { get; set; }

        //public bool IsCaptainMarvel { get; set; }
        public bool IsComplimentarySelected { get; set; }

        public string OrderId { get; set; }
        public string Promocode { get; set; }

        public string CustomerIPAddress { get; set; }

        public bool BookingNotes { get; set; }

        public string ADEmail { get; set; }

        public string MovieLanguage { get; set; }

        public string MovieRating { get; set; }

        public string MovieGenre { get; set; }

        public int FreeTicketQty { get; set; }

        public string UBEAvailedPoints { get; set; }

        public bool isNewsletter { get; set; }
        public bool isIntrestedForLuckyDraw { get; set; }
    }
}