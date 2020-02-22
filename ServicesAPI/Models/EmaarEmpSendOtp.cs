using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class EmaarEmpSendOtpReq
    {
        public string SessionId { get; set; }
        public string CinemaId { get; set; }
        public string Email { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
    public class OTPDataRes
    {
        public string OTPKey { get; set; }
    }

    public class EmaarEmpSendOTPRes
    {
        public string Status { get; set; }
        public int statusCode { get; set; }
        public string OTPKey { get; set; }        
        public string Message { get; set; }

    }
}