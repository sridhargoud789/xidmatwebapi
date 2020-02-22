using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models.Response
{
    public class DineInConfig
    {


        public DineInConfigList[] DineInConfigList { get; set; }
    }

        public class DineInConfigList
    {
            public string CinemaId { get; set; }
            public string Id { get; set; }
            public string Description { get; set; }
            public string ModifierGroupDescription { get; set; }
            public string TicketType { get; set; }
            public int TicketPriceInCents { get; set; }
            public int FoodPriceInCents { get; set; }
            public bool IsFreeTicket { get; set; }
        }


}