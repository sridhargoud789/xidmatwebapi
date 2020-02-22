using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class RemoveConcessionsReq
    {
        public string CinemaId { get; set; }
        public bool ProcessOrderValue { get; set; }
        public List<ConcessionRemoval> ConcessionRemovals { get; set; }
        public string UserSessionId { get; set; }
        public string OptionalClientId { get; set; }
        public bool ReturnOrder { get; set; }

        public bool RemoveAllConcessions { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
        //public string DeviceType { get; set; }

        //public string SessionId { get; set; }
    }

    public class ConcessionRemoval
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
