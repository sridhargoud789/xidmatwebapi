using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models.Response.TicketTypes
{
    public class GetTicketTypesResp
    {
        public int ResponseCode { get; set; }
        public Ticket[] Tickets { get; set; }
        public DateTime ActualShowDateTime { get; set; }
    }

    public class Ticket
    {
        public string AreaCategoryCode { get; set; }
        public string CinemaId { get; set; }
        public string Description { get; set; }
        public string DescriptionAlt { get; set; }
        public object[] DescriptionTranslations { get; set; }
        public object DiscountsAvailable { get; set; }
        public int DisplaySequence { get; set; }
        public string HOPK { get; set; }
        public string HeadOfficeGroupingCode { get; set; }
        public bool IsAllocatableSeating { get; set; }
        public bool IsAvailableAsLoyaltyRecognitionOnly { get; set; }
        public bool IsAvailableForLoyaltyMembersOnly { get; set; }
        public bool IsChildOnlyTicket { get; set; }
        public bool IsComplimentaryTicket { get; set; }
        public bool IsDynamicallyPriced { get; set; }
        public bool IsPackageTicket { get; set; }
        public bool IsRedemptionTicket { get; set; }
        public bool IsThirdPartyMemberTicket { get; set; }
        public string LongDescription { get; set; }
        public string LongDescriptionAlt { get; set; }
        public object[] LongDescriptionTranslations { get; set; }
        public object LoyaltyBalanceTypeId { get; set; }
        public object LoyaltyPointsCost { get; set; }
        public object LoyaltyQuantityAvailable { get; set; }
        public object LoyaltyRecognitionId { get; set; }
        public object MaxServiceFeeInCents { get; set; }
        public object MinServiceFeeInCents { get; set; }
        public Packagecontent PackageContent { get; set; }
        public string PriceGroupCode { get; set; }
        public int PriceInCents { get; set; }
        public string ProductCodeForVoucher { get; set; }
        public int QuantityAvailablePerOrder { get; set; }
        public object ResalePriceInCents { get; set; }
        public string[] SalesChannels { get; set; }
        public int SurchargeAmount { get; set; }
        public string ThirdPartyMembershipName { get; set; }
        public string TicketTypeCode { get; set; }
        public int TotalTicketFeeAmountInCents { get; set; }
        public string CustomDescription { get; set; }
    }

    public class Packagecontent
    {
        public Concession[] Concessions { get; set; }
        public Ticket1[] Tickets { get; set; }
    }

    public class Concession
    {
        public string Description { get; set; }
        public string DescriptionAlt { get; set; }
        public object[] DescriptionTranslations { get; set; }
        public string ExtendedDescription { get; set; }
        public string ExtendedDescriptionAlt { get; set; }
        public object[] ExtendedDescriptionTranslations { get; set; }
        public string HeadOfficeItemCode { get; set; }
        public string Id { get; set; }
        public int Quantity { get; set; }
    }

    public class Ticket1
    {
        public string Description { get; set; }
        public string DescriptionAlt { get; set; }
        public object[] DescriptionTranslations { get; set; }
        public int Quantity { get; set; }
        public string TicketTypeCode { get; set; }
    }
}
