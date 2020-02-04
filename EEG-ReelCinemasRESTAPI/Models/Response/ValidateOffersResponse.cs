using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class ValidateOffersResponse
    {
        public int? statusCode { get; set; }
        public object statusMessage { get; set; }
        public object Experience { get; set; }
        public Sourcedata Sourcedata { get; set; }
        public int BookingTimer { get; set; }
        public int AddExtraTimer { get; set; }
        public int WhenToShowAddTimer { get; set; }
    }

    public class Sourcedata
    {
        public Validateoffersresponse validateOffersResponse { get; set; }
        public Binfo bInfo { get; set; }
    }

    public class Validateoffersresponse
    {
        public int? Status { get; set; }
        public object Error { get; set; }
        public int OfferId { get; set; }
        public string OfferName { get; set; }
        public string[] TicketTypes { get; set; }
        public string[] FreeTicketTypes { get; set; }
        public object Code { get; set; }
        public object ConcessionPromoCode { get; set; }
        public object Discount { get; set; }
        public object ConcessionDiscount { get; set; }
        public string Pattern { get; set; }
        public object concessionItem { get; set; }
        public int? OfferQuantity { get; set; }
        public string[] InvalidVoucherOrPromo { get; set; }
        public string ValidationType { get; set; }
        public object ticket3DClassVistaCode { get; set; }
    }

    public class Binfo
    {
        public object BookingInfoId { get; set; }
        public object UserSessionId { get; set; }
        public string SessionId { get; set; }
        public string CinemaId { get; set; }
        public int? OfferId { get; set; }
        public string OfferName { get; set; }
        public float Savings { get; set; }
        public bool IsOfferApplied { get; set; }
        public Tickettype[] TicketTypes { get; set; }
        public object SelectedDineInItems { get; set; }
        public object VistaCode { get; set; }
        public object LogId { get; set; }
        public object Code { get; set; }
        public object ConcessionPromoCode { get; set; }
        public object Discount { get; set; }
        public object ConcessionDiscount { get; set; }
        public string Pattern { get; set; }
        public object MemberIdOrEmail { get; set; }
        public string CardNumber { get; set; }
        public int? OfferQuantity { get; set; }
        public object ConcessionItems { get; set; }
        public object Experience { get; set; }
        public DateTime Showdate { get; set; }
        public object Showtime { get; set; }
        public object OtherShowtime { get; set; }
        public object TicketTotal { get; set; }
        public float Vat { get; set; }
        public float Total { get; set; }
        public object Ticket3DVistacode { get; set; }
        public bool IsEnablePoints { get; set; }
    }

    public class Tickettype
    {
        public string TicketTypeCode { get; set; }
        public object TicketDescription { get; set; }
        public int? Qty { get; set; }
        public int? PriceInCents { get; set; }
        public object OptionalBarcode { get; set; }
        public bool IsFreeTicket { get; set; }
        public object PackageContent { get; set; }
        public float PriceInAed { get; set; }
        public object AreaCode { get; set; }
    }

}