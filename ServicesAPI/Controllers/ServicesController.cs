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
            return "";// GoogleDriveAPIHelper.DownloadGoogleFile(FileId);//("1RWFwIVGWH0aX7klTWyLtV2N361bkC9He");


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

                //var li = GoogleDriveAPIHelper.GetDriveFiles("1xI0fBTf8LzVpW-TPAeI8CTx7qptyAl18").ToList();
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

                return null;
            }
            catch (Exception ex)
            {

                return ex;
            }
        }

        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/GetAllProductCategories")]
        public async Task<object> GetAllProductCategories()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllProductCategories();
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/GetAllUsers")]
        public async Task<object> GetAllUsers()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllUsers(0);
                return Request.CreateResponse(HttpStatusCode.OK, dt);
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
        [Route("api/Services/GetAllProducts")]
        public async Task<object> GetAllProducts(GetAllProductsReq req)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllProducts(req.MasterProductId, req.UserId, 0, req.FreeText);
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [HttpPost]
        // [AuthenticateRequest]
        [Route("api/Services/GetAllAdminCompanyServices")]
        public async Task<object> GetAllAdminCompanyServices()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllAdminCompanyServices();
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
                dt = new ServicesDAO().GetAllCompanyServices(req.MasterServiceID, req.CompanyID, 0, req.FreeText);
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
                if (status)
                {
                    UserBO u = new UserBO();
                    u = GetUserDetails(req.UserId);
                    if (u != null)
                    {
                        SendEmail(new EmailReq()
                        {
                            templateCode = 3,
                            Name = u.FirstName + " " + u.LastName,
                            to = u.EmailId
                        },out status, out statusMessage);
                    }

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
        [Route("api/Services/ActiveInActiveUser")]
        public async Task<object> ActiveInActiveUser(ManageUserReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                new ServicesDAO().ActiveInActiveUser(req.UserId, req.IsActive);

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
        [Route("api/Services/ManageCompanyService")]
        public async Task<object> ManageCompanyService(ManageCompanyServiceReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                new ServicesDAO().ManageCompanyService(req.ServiceId, req.IsActive, req.IsApproved, req.Flag);
                if (status)
                {
                    if (req.Flag == 1)
                    {
                        UserBO u = new UserBO();
                        u = GetUserDetails(req.UserId);
                        if (u != null)
                        {
                            SendEmail(new EmailReq()
                            {
                                templateCode = 4,
                                Name = u.FirstName + " " + u.LastName,
                                to = u.EmailId,
                                IsApproved = req.IsActive
                            }, out status, out statusMessage);
                        }

                    }
                }
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
        [Route("api/Services/ApproveUser")]
        public async Task<object> ApproveUser(ManageUserReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                new ServicesDAO().ApproveUser(req.UserId, req.IsActive);

                UserBO u = new UserBO();
                u = GetUserDetails(req.UserId);
                if (u != null)
                {
                    SendEmail(new EmailReq()
                    {
                        templateCode = 1,
                        Name = u.FirstName + " " + u.LastName,
                        to = u.EmailId,
                        IsApproved = req.IsActive
                    }, out status, out statusMessage);
                }
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
        [Route("api/Services/ProductRequest")]
        public async Task<object> ProductRequest(ProductRequestReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                new ServicesDAO().ProductRequest(req);

                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllProducts(0, 0, req.MyProductId, "");
                UserBO u = new UserBO();
                u = GetUserDetails(Int64.Parse(dt.Rows[0]["UserId"].ToString()));
                if (u != null)
                {
                    SendEmail(new EmailReq()
                    {
                        templateCode = 6,
                        Name = u.FirstName + " " + u.LastName,
                        to = u.EmailId,
                        EmailId = req.EmailID,
                        FullName = req.FullName,
                        MobileNo = req.MobileNo,
                        Description = req.Description,
                        ProductTitle = dt.Rows[0]["ProductName"].ToString()
                    }, out status, out statusMessage);
                }

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
        [Route("api/Services/ServiceRequest")]
        public async Task<object> ServiceRequest(ServiceRequestReq req)
        {
            RegisterResp oResp = new RegisterResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                new ServicesDAO().ServiceRequest(req);


                Int64 CreatedBy = 0;
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllCompanyServices(0, 0, req.CompanyServiceID, "");
                UserBO u = new UserBO();
                u = GetUserDetails(Int64.Parse(dt.Rows[0]["CreatedBy"].ToString()));
                if (u != null)
                {
                    SendEmail(new EmailReq()
                    {
                        templateCode = 5,
                        Name = u.FirstName + " " + u.LastName,
                        to = u.EmailId,
                        EmailId = req.EmailID,
                        FullName = req.FullName,
                        MobileNo = req.MobileNo,
                        Description = req.Description,
                        ServiceTitle = dt.Rows[0]["ServiceTitle"].ToString()
                    }, out status, out statusMessage);
                }

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
                bool status = true;
                string statusMessage = string.Empty;

                Int64 CompanyId = 0;
                Int64 UserId = 0;

                if (req.CompanyName != "")
                {
                    CompanyId = new ServicesDAO().CreateCompany(req.CompanyName, req.Description, req.CountryCode, out status, out statusMessage);
                    if (status && CompanyId > 0 && req.FileIds != "")
                    {
                        new ServicesDAO().AddUpdateProfileMedia(CompanyId, req.Filenames, req.Filepaths, req.FileIds);
                    }
                }
                if (status)
                {
                    var PasswordSalt = PasswordHelper.GeneratePassword(10);
                    var password = PasswordHelper.EncodePassword(req.Password, PasswordSalt);
                    UserId = new ServicesDAO().CreateUser(req.EmailId, password, PasswordSalt, req.FirstName, req.LastName, req.Gender, req.MobileNoCountryCode,
                                                        req.MobileNo, req.PhoneNoCountryCode, req.PhoneNo, CompanyId, out status, out statusMessage);
                    if (status)
                    {
                        SendEmail(new EmailReq()
                        {
                            templateCode = 1,
                            Name = req.FirstName + " " + req.LastName,
                            to = req.EmailId
                        }, out status, out statusMessage);
                    }
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

        private UserBO GetUserDetails(Int64 UserId)
        {
            UserBO oUserBO = new UserBO();
            try
            {
                DataTable dt = new DataTable();
                dt = new ServicesDAO().GetAllUsers(UserId);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];

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
                    return oUserBO;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private void SendEmail(EmailReq req, out bool status, out string message)
        {
            try
            {
                string body = string.Empty;
                string subject = string.Empty;
                string cc = ConfigurationManager.AppSettings["SMTP_FROM_CC"].ToString();
                if (req.templateCode == 1)
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplates"), "Registration.html");

                    body = System.IO.File.ReadAllText(path);
                    body = body.Replace("[NAME]", req.Name);
                    subject = "Xidmat Registration";
                }
                else if (req.templateCode == 2)
                {

                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplates"), "ApproveRejectUser.html");

                    body = System.IO.File.ReadAllText(path);
                    body = body.Replace("[NAME]", req.Name);
                    string bodyMessage = string.Empty;
                    if (req.IsApproved)
                    {
                        bodyMessage = "Your request for Xidmat registration has been approved please login <a href='https://xidmat.com'>here</a> to create and manage services";
                        subject = "Xidmat Registration Approved";
                    }
                    else
                    {

                        bodyMessage = "Your request for Xidmat registration has been rejected.";
                        subject = "Xidmat Registration Rejected";
                    }
                    body = body.Replace("[Message]", bodyMessage);

                }
                else if (req.templateCode == 3)
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplates"), "CreateService.html");

                    body = System.IO.File.ReadAllText(path);
                    body = body.Replace("[NAME]", req.Name);
                    subject = "Created new service.";
                }
                else if (req.templateCode == 4)
                {

                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplates"), "ApproveRejectService.html");

                    body = System.IO.File.ReadAllText(path);
                    body = body.Replace("[NAME]", req.Name);
                    string bodyMessage = string.Empty;
                    if (req.IsApproved)
                    {
                        bodyMessage = "Your request for service registration has been approved please login <a href='https://xidmat.com'>here</a> to manage services";
                        subject = "Service Registration Approved";
                    }
                    else
                    {

                        bodyMessage = "Your request for service registration has been rejected.";
                        subject = "Service  Registration Rejected";
                    }
                    body = body.Replace("[Message]", bodyMessage);
                }
                else if (req.templateCode == 5 || req.templateCode == 6)
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplates"),
                        req.templateCode == 5 ? "ServiceRequest.html" : "ProductRequest.html");

                    subject = "Request for " + (req.templateCode == 5 ? req.ServiceTitle : req.ProductTitle) + "";

                    body = System.IO.File.ReadAllText(path);
                    body = body.Replace("[NAME]", req.Name);
                    body = body.Replace("[FullName]", req.FullName);
                    body = body.Replace("[EmailId]", req.EmailId);
                    body = body.Replace("[MobileNo]", req.MobileNo);
                    body = body.Replace("[Description]", req.Description);
                    body = body.Replace("[ProductTitle]", req.ProductTitle);
                    body = body.Replace("[ServiceTitle]", req.ServiceTitle);
                }
                else if (req.templateCode == 7)
                {
                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/EmailTemplates"), "ForgotPassword.html");

                    body = System.IO.File.ReadAllText(path);
                    body = body.Replace("[OTP]", req.ForgotPasswordUID);
                    subject = "Xidmat Reset Password OTP";
                }
                new WebMail().SendMailMessage(req.to, cc, subject, body, out status, out message);

            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message;
            }
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
                    bool IsApproved = Convert.ToBoolean(r["IsApproved"].ToString());

                    string strEncryptPassword = PasswordHelper.EncodePassword(req.Password, strPasswordSalt);
                    if (strPassword == strEncryptPassword)
                    {
                        if (!IsApproved)
                        {
                            status = false;
                            statusMessage = "AccountNotApproved";
                        }
                        else
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

        [HttpPost]
        [Route("api/Services/UpdatePassword")]
        public async Task<object> UpdatePassword(UpdatePasswordReq req)
        {
            ValidateUserResp validateUserResp = new ValidateUserResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                string empty1 = string.Empty;
                string empty2 = string.Empty;
                string password = PasswordHelper.GeneratePassword(10);
                new ServicesDAO().UpdatePassword(req.UserId, PasswordHelper.EncodePassword(req.Password, password), password, out status, out statusMessage);
                validateUserResp.status = status;
                validateUserResp.statusMessage = statusMessage;
            }
            catch (Exception ex)
            {
                validateUserResp.status = false;
                validateUserResp.statusMessage = ex.Message;
            }
            return validateUserResp;
        }

        [HttpPost]
        [Route("api/Services/ValidateForgotPasswordOTP")]
        public async Task<object> ValidateForgotPasswordOTP(ValidateOTPReq req)
        {
            ValidateUserResp validateUserResp = new ValidateUserResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                new ServicesDAO().ValidateForgotPasswordOTP(req.EmailId, req.OTP, out status, out statusMessage);
                validateUserResp.status = status;
                validateUserResp.statusMessage = statusMessage;
            }
            catch (Exception ex)
            {
                validateUserResp.status = false;
                validateUserResp.statusMessage = ex.Message;
            }
            return (object)validateUserResp;
        }

        [HttpPost]
        [Route("api/Services/ForgotPassword")]
        public async Task<object> ForgotPassword(ValidateLoginReq req)
        {
            ServicesController servicesController1 = this;
            ValidateUserResp validateUserResp = new ValidateUserResp();
            try
            {
                bool status = false;
                string statusMessage = string.Empty;
                string ForgotPasswordUID = string.Empty;
                string UserName = string.Empty;
                new ServicesDAO().ForgotPassword(req.EmailId, out ForgotPasswordUID, out UserName, out status, out statusMessage);
                if (status)
                {
                    ServicesController servicesController2 = servicesController1;
                    EmailReq req1 = new EmailReq();
                    req1.templateCode = 7;
                    req1.Name = UserName;
                    req1.to = req.EmailId;
                    req1.ForgotPasswordUID = ForgotPasswordUID;
                    SendEmail(req1, out status, out statusMessage );
                }
                validateUserResp.status = status;
                validateUserResp.statusMessage = statusMessage;
            }
            catch (Exception ex)
            {
                validateUserResp.status = false;
                validateUserResp.statusMessage = ex.Message;
            }
            return (object)validateUserResp;
        }

        [HttpPost]
        [Route("api/Services/GetUserByForgotPasswordIUD")]
        public async Task<object> GetUserByForgotPasswordIUD(ForgotPasswordUIDReq req)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable forgotPasswordIud = new ServicesDAO().GetUserByForgotPasswordIUD(req.ForgotPasswordUID);
                return Request.CreateResponse(HttpStatusCode.OK, dt);

            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
