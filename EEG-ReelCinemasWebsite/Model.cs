using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG_ReelCinemasWebsite
{
    public class MovieDataModel
    {
        public string MT { get; set; }
        public string MID { get; set; }
        public string CN { get; set; }
        public string CID { get; set; }
        public string MN { get; set; }
        public string ML { get; set; }
        public string MI { get; set; }
        public string MTR { get; set; }
        public string SD { get; set; }
        public string DU { get; set; }
        public string RT { get; set; }
        public string SP { get; set; }
        public string GR { get; set; }
        public string MDU { get; set; }
        public string DD { get; set; }
        public List<MoviesessionModel> MSLst { get; set; }
        public List<MovieExperienceModel> Experiences { get; set; }
        public string YTU { get; set; }
        public int SC { get; set; }

    }

    public class MoviesessionModel
    {
        public string SC { get; set; }
        public string SD { get; set; }
        public string ASD { get; set; }
        //public string MN { get; set; }
        public string CID { get; set; }
        public string CN { get; set; }
        public string SID { get; set; }
        // public string SessionDate { get; set; }
        // public string SessionTime { get; set; }
        public string EX { get; set; }
        public string VISTAEX { get; set; }
        //  public string OpenDate { get; set; }
        public string AV { get; set; }
        public bool isAgeRestricted { get; set; }
        public string CPPText{ get; set; }
        public string CPPHText { get; set; }
        public bool isSessionUpgradable { get; set; }
        //  public List<string> CinemaNameLst { get; set; }

    }

    public class MovieExperienceModel
    {
        public string Type { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ODataSession
    {
        public List<Session> Value { get; set; }
    }

    public class Session
    {
        public string ID { get; set; }
        public string CinemaId { get; set; }
        public string ScheduledFilmId { get; set; }
        public string SessionId { get; set; }
        public List<object> AreaCategoryCodes { get; set; }
        public DateTime Showtime { get; set; }
        public bool IsAllocatedSeating { get; set; }
        public bool AllowChildAdmits { get; set; }
        public int SeatsAvailable { get; set; }
        public bool AllowComplimentaryTickets { get; set; }
        public string EventId { get; set; }
        public string PriceGroupCode { get; set; }
        public string ScreenName { get; set; }
        public string ScreenNameAlt { get; set; }
        public int ScreenNumber { get; set; }
        public string CinemaOperatorCode { get; set; }
        public string FormatCode { get; set; }
        public string SalesChannels { get; set; }
        public List<object> SessionAttributesNames { get; set; }
        public List<object> ConceptAttributesNames { get; set; }
        public bool AllowTicketSales { get; set; }
        public bool HasDynamicallyPricedTicketsAvailable { get; set; }
        public object PlayThroughId { get; set; }
        public DateTime SessionBusinessDate { get; set; }
        public int SessionDisplayPriority { get; set; }
        public bool GroupSessionsByAttribute { get; set; }
        public string Experience { get; set; }

    }
    public class ODataMovies
    {
        public List<Movies> Value { get; set; }
    }
    public class Movies
    {
        public string ScheduledFilmId { get; set; }
        public string CinemaId { get; set; }
        public string Title { get; set; }
        public string TitleAlt { get; set; }

        public string ShowDate { get; set; }

        public string Rating { get; set; }
    }

    public class oDataMovies
    {
        public List<FilmsData> value { get; set; }
    }

    public class FilmsData
    {
        public string ID { get; set; }
        public string ShortCode { get; set; }
        public string Title { get; set; }
        public string Rating { get; set; }
        public string RatingDescription { get; set; }
        public string Synopsis { get; set; }
        public string ShortSynopsis { get; set; }
        public string HOFilmCode { get; set; }
        public string CorporateFilmId { get; set; }
        public int RunTime { get; set; }
        public DateTime? OpeningDate { get; set; }
        public string TrailerUrl { get; set; }
        public bool IsComingSoon { get; set; }
        public bool IsScheduledAtCinema { get; set; }
        public string TitleAlt { get; set; }
        public string RatingAlt { get; set; }
        public string RatingDescriptionAlt { get; set; }
        public string SynopsisAlt { get; set; }
        public string ShortSynopsisAlt { get; set; }
        public string WebsiteUrl { get; set; }
        public string GenreId { get; set; }
        public object EDICode { get; set; }
        public string TwitterTag { get; set; }
        public List<object> TitleTranslations { get; set; }
        public List<object> SynopsisTranslations { get; set; }
        public List<object> RatingDescriptionTranslations { get; set; }
        public CustomerRatingStatistics CustomerRatingStatistics { get; set; }
        public CustomerRatingTrailerStatistics CustomerRatingTrailerStatistics { get; set; }

        public string ExperienceList { get; set; }
        public string ShowCount { get; set; }
        public string FilmWebId { get; set; }
        public int MovieOrder { get; set; }

        public string Actors { get; set; }
        public string Directors { get; set; }
        public string FilmSynopsis { get; set; }
        public string Experience { get; set; }
        public string Language { get; set; }
        public string SubTitle { get; set; }

    }

    public class CustomerRatingStatistics
    {
        public int RatingCount { get; set; }
        public object AverageScore { get; set; }
    }

    public class TotalExperiences {
        public List<Experiences> value { get; set; }
    }
    public class Experiences
    {
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }


    public class CustomerRatingTrailerStatistics
    {
        public int RatingCount { get; set; }
        public int RatingCountLiked { get; set; }
    }

    public class ShowTime
    {

        public string ID { get; set; }
        public string CinemaId { get; set; }
        public string ScheduledFilmId { get; set; }
        public string SessionId { get; set; }
        public string[] AreaCategoryCodes { get; set; }
        public string CinemaOperatorCode { get; set; }
        public string FormatCode { get; set; }
        public string ScreenNumber { get; set; }
        public string ScreenName { get; set; }
        public string Showtime { get; set; }
        public int SeatsAvailable { get; set; }
        public string SalesChannels { get; set; }
        public string[] SessionAttributesNames { get; set; }
        public string ExperienceName { get; set; }

        public string ExperienceCode { get; set; }
        public string ExperienceNameURL { get; set; }
        public string ExperienceName1 { get; set; }
        public string ExperienceNameAll { get; set; }
        public string FirstExperienceName { get; set; }
        public string MovieFormat { get; set; }
        public string MovieFormatAlt { get; set; }
        public string MovieName { get; set; }
        public string CinemaName { get; set; }
    }

    public class ODataFilmGenres
    {
        //[JsonProperty("odata.metadata")]
        //public string Metadata { get; set; }
        public List<FilmGenres> Value { get; set; }
    }
    public class FilmGenres
    {

        public string ID { get; set; }
        public string Name { get; set; }

    }


    public class oDataCinemas
    {
        public List<Value> Value { get; set; }
    }

    public class Value
    {
        public string ID { get; set; }
        public object CinemaNationalId { get; set; }
        public string Name { get; set; }
        public string NameAlt { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public object Latitude { get; set; }
        public object Longitude { get; set; }
        public string ParkingInfo { get; set; }
        public string LoyaltyCode { get; set; }
        public bool IsGiftStore { get; set; }
        public string Description { get; set; }
        public object DescriptionAlt { get; set; }
        public string PublicTransport { get; set; }
        public string CurrencyCode { get; set; }
        public bool AllowPrintAtHomeBookings { get; set; }
        public bool AllowOnlineVoucherValidation { get; set; }
        public bool DisplaySofaSeats { get; set; }
        public object TimeZoneId { get; set; }
        public object HOPK { get; set; }
        public object[] NameTranslations { get; set; }
        public object[] DescriptionTranslations { get; set; }
        public object[] ParkingInfoTranslations { get; set; }
        public object[] PublicTransportTranslations { get; set; }
        public bool TipsCompulsory { get; set; }
        public string TipPercentages { get; set; }
        public string ServerName { get; set; }
    }

}
