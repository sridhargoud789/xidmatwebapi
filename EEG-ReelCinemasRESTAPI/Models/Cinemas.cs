using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class Cinemas
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string FilmID { get; set; }
        public string NameAlt { get; set; }
        public string WebpageUrl { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    public class ODataCinemas
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }
        public List<Cinemas> Value { get; set; }
    }
}