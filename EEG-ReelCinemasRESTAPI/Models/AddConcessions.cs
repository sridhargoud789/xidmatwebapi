using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{

    public class AddConcessionsReq
    {
        public string CinemaId { get; set; }
        public string UserSessionId { get; set; }

        public string SessionId { get; set; }
        public ConcessionDetail[] ConcessionDetail { get; set; }
        public string fooddesc { get; set; }
        public int bookingInfoId { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }

    public class ConcessionDetail
    {
        public string SeatKey { get; set; }
        public string ConsessionValue { get; set; }
        public string PromoCode { get; set; }
    }
    
    public class AddConcession
    {

        public string UserSessionId { get; set; }
        public string CinemaId { get; set; }
        public Concession[] Concessions { get; set; }
        public bool ReturnOrder { get; set; }
        public bool ProcessOrderValue { get; set; }
        public int BookingMode { get; set; }
        public string SessionId { get; set; }
        public Inseatorderdeliveryinfo InSeatOrderDeliveryInfo { get; set; }
        public string OptionalClientId { get; set; }
    }

    public class Inseatorderdeliveryinfo
    {
        public string Comment { get; set; }
        public string DeliveryWindowDescription { get; set; }
        public string DeliveryWindowValue { get; set; }
    }

    public class Concession
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public int RecognitionSequenceNumber { get; set; }
        public bool IsLoyaltyMembershipActivation { get; set; }
        public bool GetBarcodeFromVGC { get; set; }
        public Deliveryinfo DeliveryInfo { get; set; }
        public int DeliveryOption { get; set; }
        public int DealPriceCents { get; set; }

        public string PromoCode { get; set; }
    }

    public class Deliveryinfo
    {
        public Seat[] Seats { get; set; }
        public string Comment { get; set; }
    }

    public class Seat
    {
        public string RowId { get; set; }
        public string SeatNumber { get; set; }
    }

}