using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class ValidateEmaarEmpReq
    {
        public string SessionId { get; set; }
        public string CinemaId { get; set; }

        public string UserSessionId { get; set; }
        public string OTPKey { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceType { get; set; }
    }


    public class ValidateEmaarEmpRes
    {
        public int statusCode { get; set; }
        public string status { get; set; }
        public string statusMessage { get; set; }

        public ValidateEmaarEmpResposedata ResposeData { get; set; }
    }

    public class ValidateEmaarEmpResposedata
    {
        public string accountStatus { get; set; }
        public bool isValidUser { get; set; }
        public ValidateEmaarEmpAccountdetail[] accountDetails { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountPercentage_FNB { get; set; }
        public string DiscountCode_FNB { get; set; }
        public int NoOfFreeTickets { get; set; }
    }

    public class ValidateEmaarEmpAccountdetail
    {
        public string firstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string designation { get; set; }
        public string displayName { get; set; }
        public string telephone { get; set; }
        public string department { get; set; }
        public string mgrDisplayName { get; set; }
        public string employeeID { get; set; }
        public string AccountName { get; set; }
        public string thumbnailPhotoBase64Enc { get; set; }
        public string Mobile { get; set; }
    }

}