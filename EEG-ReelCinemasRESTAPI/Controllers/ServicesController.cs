using EEG_ReelCinemasRESTAPI.Common;
using EEG_ReelCinemasRESTAPI.Common.Smartbutton;
using EEG_ReelCinemasRESTAPI.Models;
using Newtonsoft.Json;
using ReelDAO;
using ReelDvo;
using ReelDVO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace EEG_ReelCinemasRESTAPI.Controllers
{
    public class ServicesController : ApiController
    {
        private DataTable ServiceSettings = null;
        private BaseAnalizer baseAnalizer = null;
        private Analizer analizer = null;
        MobileBookingDao oMobileBookingDao = null;
        private string VistaOptionalClientId = "";
        private string ReturnValue = string.Empty;

        [HttpPost]
       // [AuthenticateRequest]
        [Route("api/Services/GetAllMasterServices")]
        public async Task<object> GetAllMasterServices()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetServiceSettings();
                var oResp = JsonConvert.SerializeObject(dt);
                return oResp;
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }
    }
}
