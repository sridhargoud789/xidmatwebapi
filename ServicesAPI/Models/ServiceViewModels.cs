using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ServicesAPI.Models
{
    [System.Xml.Serialization.XmlRootAttribute("results", Namespace = "", IsNullable = false)]
    public class SMSResponseServiceModel
    {
        [XmlElement("result", typeof(SMSResultServiceModel))]
        public SMSResultServiceModel SMSResponse { get; set; }
    }

    public class SMSResultServiceModel
    {
        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("messageid")]
        public string Messageid { get; set; }

        [XmlElement("destination")]
        public string Destination { get; set; }
    }
}