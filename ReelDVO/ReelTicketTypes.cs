using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelDvo
{
    public class ReelTicketTypes
    {
        public string TicketTypeCode { get; set; }
        public int Qty { get; set; } 

    }

    public class KeyValuePairs
    {
        public string KEY;
        public int VALUE;
    }

    public class ConsessionKeyValuePairs
    {
        /// <summary>
        /// SEAT
        /// </summary>
        public string SeatKey;
        /// <summary>
        /// Consession array
        /// </summary>
        public string ConsessionValue;
    }
}
