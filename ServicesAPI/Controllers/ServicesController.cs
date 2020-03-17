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
using ServicesAPI.Helpers;

using System.Web.Hosting;
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

        [HttpGet]
        [Route("api/Services/GetFile/{FileId}")]
        public string GetFile(string FileId)
        {
            return GoogleDriveAPIHelper.DownloadGoogleFile(FileId);//("1RWFwIVGWH0aX7klTWyLtV2N361bkC9He");


        }

        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/UploadFiles")]
        public async Task<object> UploadFiles()
        {
            try
            {
                string GDrivefileID = string.Empty;
                var file = HttpContext.Current.Request.Files.Count > 0 ?
                        HttpContext.Current.Request.Files[0] : null;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    Files oFile = new Files();

                    Guid guid = Guid.NewGuid();
                    string FileName = guid.ToString() + System.IO.Path.GetExtension(file.FileName).ToLower();

                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/PublicFiles"), Path.GetFileName(FileName));
                    file.SaveAs(path);

                    //HttpPostedFileBase filebase =new HttpPostedFileWrapper(file);

                    // GDrivefileID = GoogleDriveAPIHelper.FileUploadInFolder(filebase,FileName);

                    //byte[] oBytes = new byte[file.ContentLength];
                    //using (BinaryReader theReader = new BinaryReader(file.InputStream))
                    //{
                    //    oBytes  = theReader.ReadBytes(file.ContentLength);
                    //}
                    //oFile.base64 = Convert.ToBase64String(oBytes);
                    //oFile.name = fileName.ToString();
                    //oFile.size = file.ContentLength.ToString();
                    //oFile.type = file.ContentType;

                    //List<Files> obj = new List<Files>();
                    //obj.Add(oFile);

                    //DataTable dt = new DataTable();
                    //dt = new ServicesDAO().SavePublicFiles(JsonConvert.SerializeObject(obj.ToArray()));

                    return Request.CreateResponse(HttpStatusCode.OK, FileName);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(ex);
            }
        }


        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/GetAdvtMedia")]
        public async Task<object> GetAdvtMedia()
        {
            try
            {
                string strOutPut = string.Empty;

                var li = GoogleDriveAPIHelper.GetDriveFiles("1xI0fBTf8LzVpW-TPAeI8CTx7qptyAl18").ToList();
                //foreach (var l in li)
                //{
                //    if (l.MimeType== "image/jpeg" || l.MimeType == "image/png" || l.MimeType == "image/jpg")
                //    {
                //        strOutPut += "<div class='item'><img src='https://drive.google.com/uc?id=" + l.Id + "' alt='Image Description' /></div>";
                //    }
                //    else if (l.MimeType== "video/mp4")
                //    {
                //        strOutPut += "<div class='item'><iframe src='https://drive.google.com/uc?id=" + l.Id + "'></div>";
                //    }

                //}

                return Request.CreateResponse(HttpStatusCode.OK, li);
            }
            catch (Exception ex)
            {

                return ex;
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
        [Route("api/Services/GetAllProducts")]
        public async Task<object> GetAllProducts(GetAllProductsReq req)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllProducts(req.MasterProductId, req.UserId);
                return Request.CreateResponse(HttpStatusCode.OK, dt);
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
                return Request.CreateResponse(HttpStatusCode.OK, dt);
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
                return Request.CreateResponse(HttpStatusCode.OK, dt);
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
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/ManageMyProducts")]
        public async Task<object> ManageMyProducts(ManageProductsReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                Int64 ProductId = new ServicesDAO().ManageMyProducts(req, out status, out statusMessage);
                if (ProductId > 0 && req.FileIds != "")
                {
                    new ServicesDAO().AddUpdateProductsMedia(ProductId, req.FileIds);
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
        [Route("api/Services/CreateService")]
        public async Task<object> CreateService(CreateServiceReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                Int64 ServicesID = new ServicesDAO().CreateService(req, out status, out statusMessage);
                if (ServicesID > 0 && req.FileIds != "")
                {
                    new ServicesDAO().AddUpdateServicesMedia(ServicesID, req.Filenames, req.Filepaths, req.FileIds);
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

                if(req.CompanyName != "")
                {
                     CompanyId = new ServicesDAO().CreateCompany(req.CompanyName, req.Description, req.CountryCode);
                     if (CompanyId > 0 && req.FileIds != "")
                        {
                          new ServicesDAO().AddUpdateProfileMedia(CompanyId, req.Filenames, req.Filepaths, req.FileIds);
                        }
                }
                var PasswordSalt = PasswordHelper.GeneratePassword(10);
                var password = PasswordHelper.EncodePassword(req.Password, PasswordSalt);
                UserId = new ServicesDAO().CreateUser(req.EmailId, password, PasswordSalt, req.FirstName, req.LastName, req.Gender, req.MobileNoCountryCode,
                                                    req.MobileNo, req.PhoneNoCountryCode, req.PhoneNo, CompanyId, out status, out statusMessage);


                //new WebMail().SendMailMessage(req.EmailId, "feroz@xidmat.com", "sridhargoud789@gmail.com", "", "test", "test body", null);

                oResp.status = status;
                oResp.statusMessage = statusMessage;
            }
            catch (Exception ex)
            {
                oResp.status = false;
                oResp.statusMessage = ex.Message;
            }
            return req;
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
                            oUserBO.RoleId = int.Parse(r["RoleId"].ToString());
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
