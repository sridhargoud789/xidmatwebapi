using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models.Response
{
    public class StandardTicketTypesConfig
    {


        public STTClass[] STTClass { get; set; }




    }

    public class STTClass
    {
        public string CinemaId { get; set; }
        public STTTickettype[] TicketTypes { get; set; }
    }

    public class STTTickettype
    {
        public string TicketsType { get; set; }
        public string TicketDescription { get; set; }
    }
}