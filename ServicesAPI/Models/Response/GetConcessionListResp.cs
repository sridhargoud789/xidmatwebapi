using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models.Response.ConcessionList
{
    public class GetConcessionListResp
    {
        public Concessiontab[] ConcessionTabs { get; set; }
        public object ErrorDescription { get; set; }
        public int ResponseCode { get; set; }
    }

    public class Concessiontab
    {
        public Concessionitem[] ConcessionItems { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Concessionitem
    {
        public object[] AlternateItems { get; set; }
        public bool CanGetBarcode { get; set; }
        public string Description { get; set; }
        public string DescriptionAlt { get; set; }
        public object[] DescriptionTranslations { get; set; }
        public string ExtendedDescription { get; set; }
        public string ExtendedDescriptionAlt { get; set; }
        public string HOPK { get; set; }
        public string HeadOfficeItemCode { get; set; }
        public string Id { get; set; }
        public bool IsAvailableForInSeatDelivery { get; set; }
        public bool IsAvailableForPickupAtCounter { get; set; }
        public bool IsRecognitionOnly { get; set; }
        public string ItemClassCode { get; set; }
        public string LoyaltyDiscountCode { get; set; }
        public Modifiergroup[] ModifierGroups { get; set; }
        public object[] PackageChildItems { get; set; }
        public int PriceInCents { get; set; }
        public object RecognitionExpiryDate { get; set; }
        public int RecognitionId { get; set; }
        public int RecognitionMaxQuantity { get; set; }
        public float RecognitionPointsCost { get; set; }
        public int RecognitionSequenceNumber { get; set; }
        public int RedeemableType { get; set; }
        public bool RequiresPickup { get; set; }
        public bool RestrictToLoyalty { get; set; }
        public string ShippingMethod { get; set; }
        public object[] SmartModifiers { get; set; }
        public string VoucherSaleType { get; set; }
    }

    public class Modifiergroup
    {
        public string Description { get; set; }
        public int FreeQuantity { get; set; }
        public string Id { get; set; }
        public int MaximumQuantity { get; set; }
        public int MinimumQuantity { get; set; }
        public Modifier[] Modifiers { get; set; }
    }

    public class Modifier
    {
        public string Description { get; set; }
        public string Id { get; set; }
        public int PriceInCents { get; set; }
    }
}
