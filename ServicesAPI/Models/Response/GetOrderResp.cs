using System;

namespace ServicesAPI.Models.Response.GetOrder
{

    public class GetOrderResp
    {
        public object ErrorDescription { get; set; }
        public int ExtendedResultCode { get; set; }
        public Order Order { get; set; }
        public int Result { get; set; }
    }

    public class Order
    {
        public object[] AppliedGiftCards { get; set; }
        public int BookingFeeValueCents { get; set; }
        public string CinemaId { get; set; }
        public Concession[] Concessions { get; set; }
        public object Customer { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public object LoyaltyPointsCost { get; set; }
        public Session[] Sessions { get; set; }
        public object[] SuggestedDeals { get; set; }
        public int TotalOrderCount { get; set; }
        public object TotalTicketFeeValueInCents { get; set; }
        public int TotalValueCents { get; set; }
        public string UserSessionId { get; set; }
        public int VistaBookingNumber { get; set; }
        public int VistaTransactionNumber { get; set; }
    }

    public class Concession
    {
        public object DealDefinitionId { get; set; }
        public object DealDescription { get; set; }
        public int DealPriceCents { get; set; }
        public object DeliveryInfo { get; set; }
        public int DeliveryOption { get; set; }
        public bool GetBarcodeFromVGC { get; set; }
        public object HeadOfficeItemCode { get; set; }
        public bool IsLoyaltyMembershipActivation { get; set; }
        public string ItemId { get; set; }
        public object Modifiers { get; set; }
        public object PackageChildItems { get; set; }
        public object ParentSaleItem { get; set; }
        public object PromoCode { get; set; }
        public int Quantity { get; set; }
        public string RecognitionId { get; set; }
        public int RecognitionSequenceNumber { get; set; }
        public object VoucherBarcode { get; set; }
    }

    public class Session
    {
        public bool AllocatedSeating { get; set; }
        public string AltFilmClassification { get; set; }
        public string AltFilmTitle { get; set; }
        public string CinemaId { get; set; }
        public string FilmClassification { get; set; }
        public string FilmTitle { get; set; }
        public object[] FilmTitleTranslations { get; set; }
        public bool SeatsAllocated { get; set; }
        public int SessionId { get; set; }
        public DateTime ShowingRealDateTime { get; set; }
        public DateTime ShowingRealDateTimeOffset { get; set; }
        public ServicesAPI.Models.Response.Offer.Ticket[] Tickets { get; set; }
    }
}

namespace ServicesAPI.Models.Response.Offer
{
    public class Ticket
    {
        public string AltDescription { get; set; }
        public string Barcode { get; set; }
        public object DealDefinitionId { get; set; }
        public object DealDescription { get; set; }
        public int DealPriceCents { get; set; }
        public string Description { get; set; }
        public object[] DescriptionTranslations { get; set; }
        public int DiscountPriceCents { get; set; }
        public object[] DiscountsAvailable { get; set; }
        public string Id { get; set; }
        public bool IsTicketPackage { get; set; }
        public string LoyaltyRecognitionId { get; set; }
        public int LoyaltyRecognitionSequence { get; set; }
        public object PackageConcessions { get; set; }
        public object PackageId { get; set; }
        public object PackageTickets { get; set; }
        public int PriceCents { get; set; }
        public object PromotionInstanceGroupNumber { get; set; }
        public object PromotionTicketTypeId { get; set; }
        public string SeatAreaCatCode { get; set; }
        public string SeatAreaNumber { get; set; }
        public int SeatColumnIndex { get; set; }
        public string SeatData { get; set; }
        public object SeatIsDeliverable { get; set; }
        public string SeatNumber { get; set; }
        public string SeatRowId { get; set; }
        public int SeatRowIndex { get; set; }
        public string TicketTypeCode { get; set; }
        public string TicketTypeHOPK { get; set; }
    }

}