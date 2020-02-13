using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class ServicesBO
    {
    }
    public class GetAllCompanyServicesReq
    {
        public Int64 MasterServiceID { get; set; }
    }

    public class GetAllMasterServicesResp
    {
        public GetAllMasterServicesRespData[] data { get; set; }
    }

    public class GetAllMasterServicesRespData
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ImagePath { get; set; }
    }

}