using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelDvo
{
    public class ServiceLogDvo
    {
        public Int64 LogId { get; set; }
        public string UserSession { get; set; }
        public string SessionId { get; set; }
        public string CinemaId { get; set; }
        public string BookingId { get; set; }
        public string ReelServiceName { get; set; }
        public string ServiceCall { get; set; }
        public string Device { get; set; }
        public string LogType { get; set; }
        public string LogData { get; set; }
        public string ServiceStatus { get; set; }
    }
}
