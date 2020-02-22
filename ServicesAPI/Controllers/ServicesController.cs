using ServicesAPI.Common;

using ServicesAPI.Models;
using ServicesAPI.Models.Response;

using System.Web.Http.Cors;

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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ServicesAPI.Models;
using EEG_ReelCinemasRESTAPI.Models;

namespace ServicesAPI.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        [Route("api/Services/UploadFiles")]
        public async Task<object> UploadFiles(Files[] req)
        {
            try
            {
                FileUploadResp resp = new FileUploadResp();
                List<FilesResp> respfiles = new List<FilesResp>();
                for (int i = 0; i < req.Length; i++)
                {
                    var file = req[i];
                
                    String path = HttpContext.Current.Server.MapPath("~/PublicFiles"); //Path

                    Guid obj = Guid.NewGuid();
                    int icnt = file.name.Split('.').Length;

                    string extension = file.name.Split('.')[icnt-1].ToString();
                    string fileName = obj.ToString()+"." + extension;

                    //set the image path
                    string filePath = Path.Combine(path, fileName);

                    byte[] fileBytes = Convert.FromBase64String(file.base64.Split(',')[1].ToString());

                    System.IO.File.WriteAllBytes(filePath, fileBytes);


                    respfiles.Add(new FilesResp()
                    {
                        FileName = fileName,
                        FilePath = filePath
                    });
                }
                resp.Files = respfiles.ToArray();
                return resp;
            }
            catch (Exception ex)
            {

                return null;
            }
        }



        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/GetAllMasterServices")]
        public async Task<object> GetAllMasterServices()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetServiceSettings();
                GetAllMasterServicesResp resp = new GetAllMasterServicesResp();
                return Request.CreateResponse(HttpStatusCode.OK, dt);
                //var oResp = JsonConvert.SerializeObject(dt);
                //resp = JsonConvert.DeserializeObject<GetAllMasterServicesResp>(oResp);
                //return resp;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/GetAllCompanyServices")]
        public async Task<object> GetAllCompanyServices(GetAllCompanyServicesReq req)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllCompanyServices(req.MasterServiceID, req.CompanyID);
                var oResp = JsonConvert.SerializeObject(dt);
                return oResp;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/GetAllServicesRequests")]
        public async Task<object> GetAllServicesRequests(GetAllServicesRequestsReq req)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllServicesRequests(req);
                var oResp = JsonConvert.SerializeObject(dt);
                return oResp;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/GetAllServicesRequests")]
        public async Task<object> GetAllCompanies()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllCompanies();
                var oResp = JsonConvert.SerializeObject(dt);
                return oResp;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/CreateService")]
        public async Task<object> CreateService(CreateServiceReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                Int64 ServicesID = new ServicesDAO().CreateService(req, out status, out statusMessage);
                if (ServicesID > 0 && req.Filenames != "")
                {
                    new ServicesDAO().AddUpdateServicesMedia(ServicesID, req.Filenames, req.Filepaths);
                }

                oResp.status = status;
                oResp.statusMessage = statusMessage;
            }
            catch (Exception ex)
            {
                oResp.status = false;
                oResp.statusMessage = ex.Message;
            }
            return oResp;
        }

        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/ServiceRequest")]
        public async Task<object> ServiceRequest(ServiceRequestReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                new ServicesDAO().ServiceRequest(req);

                oResp.status = true;
                oResp.statusMessage = "SUCCESS";
            }
            catch (Exception ex)
            {
                oResp.status = false;
                oResp.statusMessage = ex.Message;
            }
            return oResp;
        }
        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/AddServiceViewCount")]
        public async Task<object> AddServiceViewCount(AddServiceViewCountReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                new ServicesDAO().AddServiceViewCount(req.ServiceID);

                oResp.status = true;
                oResp.statusMessage = "SUCCESS";
            }
            catch (Exception ex)
            {
                oResp.status = false;
                oResp.statusMessage = ex.Message;
            }
            return oResp;
        }

        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/Register")]
        public async Task<object> Register(RegisterReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;

                Int64 CompanyId = 0;
                Int64 UserId = 0;
                CompanyId = new ServicesDAO().CreateCompany(req.CompanyName, req.Description);

                if (CompanyId > 0 && req.Filenames != "")
                {
                    new ServicesDAO().AddUpdateProfileMedia(CompanyId, req.Filenames, req.Filepaths);
                }


                var PasswordSalt = PasswordHelper.GeneratePassword(10);
                var password = PasswordHelper.EncodePassword(req.Password, PasswordSalt);
                UserId = new ServicesDAO().CreateUser(req.EmailId, password, PasswordSalt, req.FirstName, req.LastName, req.Gender, req.DOB, req.MobileNoCountryCode,
                                                    req.MobileNo, req.PhoneNoCountryCode, req.PhoneNo, CompanyId, out status, out statusMessage);
                oResp.status = status;
                oResp.statusMessage = statusMessage;
            }
            catch (Exception ex)
            {
                oResp.status = false;
                oResp.statusMessage = ex.Message;
            }
            return oResp;
        }


        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/ValidateLogin")]
        public async Task<object> ValidateLogin(ValidateLoginReq req)
        {
            ValidateUserResp oResp = new ValidateUserResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;

                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetUserDetailsByEmailId(req.EmailId);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    string strPassword = r["Password"].ToString();
                    string strPasswordSalt = r["PasswordSalt"].ToString();
                    bool IsActive = Convert.ToBoolean(r["IsActive"].ToString());
                    string strEncryptPassword = PasswordHelper.EncodePassword(req.Password, strPasswordSalt);
                    if (strPassword == strEncryptPassword)
                    {
                        if (!IsActive)
                        {
                            status = false;
                            statusMessage = "InActiveUser";
                        }
                        else
                        {
                            UserBO oUserBO = new UserBO();
                            oUserBO.UserId = Int64.Parse(r["UserId"].ToString());
                            oUserBO.EmailId = r["EmailId"].ToString();
                            oUserBO.Password = r["Password"].ToString();
                            oUserBO.PasswordSalt = r["PasswordSalt"].ToString();
                            oUserBO.FirstName = r["FirstName"].ToString();
                            oUserBO.LastName = r["LastName"].ToString();
                            oUserBO.Gender = r["Gender"].ToString();
                            if (r["DOB"].ToString() != "")
                            {
                                oUserBO.DOB = Convert.ToDateTime(r["DOB"].ToString());
                            }

                            oUserBO.MobileNoCountryCode = r["MobileNoCountryCode"].ToString();
                            oUserBO.MobileNo = r["MobileNo"].ToString();
                            oUserBO.PhoneNoCountryCode = r["PhoneNoCountryCode"].ToString();
                            oUserBO.PhoneNo = r["PhoneNo"].ToString();
                            if (r["CreatedOn"].ToString() != "")
                            {
                                oUserBO.CreatedOn = Convert.ToDateTime(r["CreatedOn"].ToString());
                            }

                            if (r["UpdatedOn"].ToString() != "")
                            {
                                oUserBO.UpdatedOn = Convert.ToDateTime(r["UpdatedOn"].ToString());
                            }

                            oUserBO.CreatedBy = r["CreatedBy"].ToString();
                            oUserBO.IsActive = r["IsActive"].ToString() == "1" ? true : false;
                            oUserBO.CompanyID = Int64.Parse(r["CompanyID"].ToString());

                            status = true;
                            statusMessage = "SUCCESS";
                            oResp.UserObject = oUserBO;
                        }
                    }
                    else
                    {
                        status = true;
                        statusMessage = "InvalidCredentials";
                    }
                }
                else
                {
                    status = false;
                    statusMessage = "UserNotFound";
                }
                oResp.status = status;
                oResp.statusMessage = statusMessage;
            }
            catch (Exception ex)
            {
                oResp.status = false;
                oResp.statusMessage = ex.Message;
            }
            return oResp;
        }


    }
}
