using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class ConfirmationEntity
    {
    }

    public class BookingConfirmationDetail
    {
        public string Experience { get; set; }
        public string ExperienceName { get; set; }
        public decimal OfferAmount { get; set; }
        public string MovieName { get; set; }
        public string Rating { get; set; }
        public string CinemaName { get; set; }

        public string OfferName { get; set; }
        public decimal? Savings { get; set; }

        public string CinemaId { get; set; }
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
        public long BookingInfoId { get; set; }
        public bool? IsEnablePoints { get; set; } = true;
        public string EncryptPoints { get; set; }
        public decimal? EarnedPoints { get; set; }
        public List<FoodDetail> FoodDetailList { get; set; }
        public List<TicketDetail> TicketDetailList { get; set; }
        public ConfirmationBanners Banners { get; set; }
        public string LocationPageUrl { get; set; }
        public string AddCalendarUrl { get; set; }
        public string AddWalletUrl { get; set; }
        public string GoogleACDataLayerCode { get; set; }


    }


    public class TicketDetail
    {
        public string SeatInfo { get; set; }
        public string Description { get; set; }
        public decimal UnitAmount { get; set; }
        public int Quantity { get; set; }
    }

    public class FoodDetail
    {
        public string SeatInfo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitAmount { get; set; }
        public int Quantity { get; set; }
    }


    #region Banners    
    public class Banner1
    {
        public bool ShowBanner { get; set; }
        public string BannerTitle { get; set; }
        public string BannerInnerContent { get; set; }
        public string BannerImageUrl { get; set; }
        public string BannerPointsHeading { get; set; }
        public string BannerPoints { get; set; }
        public string RedirectUrlWithHeading { get; set; }
        public string BrandLogo { get; set; }
        public string MainBannerRedirectUrl { get; set; }
    }
    public class Banner2
    {
        public bool ShowBanner { get; set; }
        public string BannerTitle { get; set; }
        public string BannerInnerContent { get; set; }
        public string BannerImageUrl { get; set; }
        public string MainBannerRedirectUrl { get; set; }
        public string BannerPointsHeading { get; set; }
        public string BannerPoints { get; set; }
        public string RedirectUrlWithHeading { get; set; }
        public string BrandLogo { get; set; }
    }
    public class ConfirmationBanners
    {
        public string CinemaId { get; set; }
        public string[] AllowedExperiences { get; set; }
        public Banner1 Banner1 { get; set; }
        public Banner2 Banner2 { get; set; }
    }
    #endregion
}