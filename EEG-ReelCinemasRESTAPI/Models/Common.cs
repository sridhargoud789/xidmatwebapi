using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class CommonResp
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
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
}