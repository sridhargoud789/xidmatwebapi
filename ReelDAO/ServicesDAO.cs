using EEG_ReelCinemasRESTAPI.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json;
using ReelDAO;
using ReelDvo;
using ReelDVO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReelDAO
{
    public class ServicesDAO
    {
        private Database db = null;
        private LogDao log = null;
        public DataTable GetServiceSettings()
        {
            try
            {
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                DbCommand command = db.GetStoredProcCommand("GetAllMasterServices");

                return db.ExecuteDataSet(command).Tables[0];
            }
            catch (Exception ex)
            {
                //new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "GetServiceSettings", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE", null);
                return null;
            }
            finally
            {

            }
        }
        public DataTable GetUserDetailsByEmailId(string EmailId)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                DbCommand command = db.GetStoredProcCommand("GetUserDetailsByEmailId");
                db.AddInParameter(command, "EmailId", DbType.String, EmailId);

                return db.ExecuteDataSet(command).Tables[0];
            }
            catch (Exception ex)
            {
                //new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "GetServiceSettings", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE", null);
                return null;
            }
            finally
            {

            }
        }
        public DataTable SavePublicFiles(string strJSON)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                DbCommand command = db.GetStoredProcCommand("SavePublicFiles");
                db.AddInParameter(command, "strJSON", DbType.String, strJSON);

                return db.ExecuteDataSet(command).Tables[0];
            }
            catch (Exception ex)
            {
                //new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "GetServiceSettings", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE", null);
                return null;
            }
            finally
            {

            }
        }

public DataTable GetAllProducts(Int64 MasterProductId, Int64 UserId)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                DbCommand command = db.GetStoredProcCommand("GetAllProducts");
                db.AddInParameter(command, "MasterProductId", DbType.Int64, MasterProductId);
                db.AddInParameter(command, "UserId", DbType.Int64, UserId);


                return db.ExecuteDataSet(command).Tables[0];
            }
            catch (Exception ex)
            {
                //new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "GetServiceSettings", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE", null);
                return null;
            }
            finally
            {

            }
        }

        public DataTable GetAllCompanyServices(Int64 MasterServiceID, Int64 CompanyID)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                DbCommand command = db.GetStoredProcCommand("GetAllCompanyServices");
                db.AddInParameter(command, "MasterServiceID", DbType.Int64, MasterServiceID);
                db.AddInParameter(command, "CompanyID", DbType.Int64, CompanyID);


                return db.ExecuteDataSet(command).Tables[0];
            }
            catch (Exception ex)
            {
                //new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "GetServiceSettings", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE", null);
                return null;
            }
            finally
            {

            }
        }

        public DataTable GetAllServicesRequests(GetAllServicesRequestsReq req)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                DbCommand command = db.GetStoredProcCommand("GetAllServicesRequests");
                db.AddInParameter(command, "MasterServiceID", DbType.Int64, req.MasterServiceId);
                db.AddInParameter(command, "CompanyID", DbType.Int64, req.CompanyId);
                db.AddInParameter(command, "CompanyServiceId", DbType.Int64, req.CompanyServiceId);



                return db.ExecuteDataSet(command).Tables[0];
            }
            catch (Exception ex)
            {
                //new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "GetServiceSettings", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE", null);
                return null;
            }
            finally
            {

            }
        }

        public DataTable GetAllCompanies()
        {
            try
            {
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                DbCommand command = db.GetStoredProcCommand("GetAllCompanies");

                return db.ExecuteDataSet(command).Tables[0];
            }
            catch (Exception ex)
            {
                //new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "GetServiceSettings", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE", null);
                return null;
            }
            finally
            {

            }
        }

        public Int64 CreateCompany(string CompanyName, string Description, string CountryCode)
        {
            Int64 CompanyID = 0;
            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                command = db.GetStoredProcCommand("CreateCompany");
                db.AddInParameter(command, "CompanyName", DbType.String, CompanyName);
                db.AddInParameter(command, "Description", DbType.String, Description);
                db.AddInParameter(command, "CountryCode", DbType.String, CountryCode);
                db.AddOutParameter(command, "CompanyID", DbType.Int64, 10);
                result = db.ExecuteNonQuery(command);
                CompanyID = int.Parse(db.GetParameterValue(command, "CompanyID").ToString());
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return CompanyID;
        }


        public void ServiceRequest(ServiceRequestReq req)
        {

            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                command = db.GetStoredProcCommand("ServiceRequest");
                db.AddInParameter(command, "CompanyServiceID", DbType.Int64, req.CompanyServiceID);
                db.AddInParameter(command, "FullName", DbType.String, req.FullName);
                db.AddInParameter(command, "EmailID", DbType.String, req.EmailID);
                db.AddInParameter(command, "MobileNoCC", DbType.String, req.MobileNoCC);
                db.AddInParameter(command, "MobileNo", DbType.String, req.MobileNo);
                db.AddInParameter(command, "Description", DbType.String, req.Description);
                db.AddInParameter(command, "CountryCode", DbType.String, req.CountryCode);

                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }

        public void AddServiceViewCount(Int64 ServiceID)
        {

            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                command = db.GetStoredProcCommand("AddServiceViewCount");
                db.AddInParameter(command, "ServiceID", DbType.Int64, ServiceID);
                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }
        public void AddUpdateServicesMedia(Int64 ServicesID, string Filenames, string Filepaths, string FileIds)
        {

            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                command = db.GetStoredProcCommand("AddUpdateServicesMedia");
                db.AddInParameter(command, "ServicesID", DbType.Int64, ServicesID);
                db.AddInParameter(command, "Filenames", DbType.String, Filenames);
                db.AddInParameter(command, "Filepaths", DbType.String, Filepaths);
                db.AddInParameter(command, "FileIds", DbType.String, FileIds);

                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }

        public void AddUpdateProductsMedia(Int64 MyProductId, string FileIds)
        {

            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                command = db.GetStoredProcCommand("AddUpdateProductsMedia");
                db.AddInParameter(command, "ServicesID", DbType.Int64, MyProductId);
                db.AddInParameter(command, "FileIds", DbType.String, FileIds);

                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }
        public Int64 ManageMyProducts(ManageProductsReq req, out bool status, out string statusMessage)
        {
            Int64 MyProductId = 0;
            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                command = db.GetStoredProcCommand("ManageMyProducts");
                db.AddInParameter(command, "MasterProductId", DbType.Int64, req.MasterProductId);

                db.AddInParameter(command, "ProductName", DbType.String, req.ProductName);
                db.AddInParameter(command, "ProductDescription", DbType.String, req.ProductDescription);
                db.AddInParameter(command, "UserId", DbType.Int64, req.UserId);

                db.AddInParameter(command, "FileIds", DbType.String, req.FileIds);

                db.AddOutParameter(command, "MyProductId", DbType.Int64, 10);
                db.AddOutParameter(command, "Status", DbType.Boolean, 10);
                db.AddOutParameter(command, "StatusMessage", DbType.String, 50);
                result = db.ExecuteNonQuery(command);
                MyProductId = int.Parse(db.GetParameterValue(command, "MyProductId").ToString());
                status = Convert.ToBoolean(db.GetParameterValue(command, "Status"));
                statusMessage = db.GetParameterValue(command, "StatusMessage").ToString();
            }
            catch (Exception ex)
            {
                status = false;
                statusMessage = ex.Message;
            }
            finally
            {

            }
            return MyProductId;
        }
        public void AddUpdateProfileMedia(Int64 CompanyID, string Filenames, string Filepaths, string FileIds)
        {

            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                command = db.GetStoredProcCommand("AddUpdateProfileMedia");
                db.AddInParameter(command, "CompanyID", DbType.Int64, CompanyID);
                db.AddInParameter(command, "Filenames", DbType.String, Filenames);
                db.AddInParameter(command, "Filepaths", DbType.String, Filepaths);
                db.AddInParameter(command, "FileIds", DbType.String, FileIds);


                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }

        public Int64 CreateService(CreateServiceReq req, out bool status, out string statusMessage)
        {
            Int64 CompanyServiceID = 0;
            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                command = db.GetStoredProcCommand("CreateService");
                db.AddInParameter(command, "CompanyID", DbType.Int64, req.CompanyID);
                db.AddInParameter(command, "MasterServiceID", DbType.Int32, req.MasterServiceID);
                db.AddInParameter(command, "CreatedBy", DbType.Int64, req.CreatedBy);
                db.AddInParameter(command, "ServiceTitle", DbType.String, req.ServiceTitle);
                db.AddInParameter(command, "ServiceDescription", DbType.String, req.ServiceDescription);
                db.AddInParameter(command, "Timings", DbType.String, req.Timings);
                db.AddInParameter(command, "CountryCode", DbType.String, req.CountryCode);

                db.AddOutParameter(command, "CompanyServiceID", DbType.Int64, 10);
                db.AddOutParameter(command, "Status", DbType.Boolean, 10);
                db.AddOutParameter(command, "StatusMessage", DbType.String, 50);
                result = db.ExecuteNonQuery(command);
                CompanyServiceID = int.Parse(db.GetParameterValue(command, "CompanyServiceID").ToString());
                status = Convert.ToBoolean(db.GetParameterValue(command, "Status"));
                statusMessage = db.GetParameterValue(command, "StatusMessage").ToString();
            }
            catch (Exception ex)
            {
                status = false;
                statusMessage = ex.Message;
            }
            finally
            {

            }
            return CompanyServiceID;
        }

        public Int64 CreateUser(string EmailId, string Password, string PasswordSalt, string FirstName, string LastName, string Gender, string MobileNoCountryCode,
            string MobileNo, string PhoneNoCountryCode, string PhoneNo, Int64 CompanyID, out bool status, out string statusMessage)
        {
            Int64 UserID = 0;
            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                command = db.GetStoredProcCommand("CreateUser");

                db.AddInParameter(command, "EmailId", DbType.String, EmailId);
                db.AddInParameter(command, "Password", DbType.String, Password);
                db.AddInParameter(command, "PasswordSalt", DbType.String, PasswordSalt);
                db.AddInParameter(command, "FirstName", DbType.String, FirstName);
                db.AddInParameter(command, "LastName", DbType.String, LastName);
                db.AddInParameter(command, "Gender", DbType.String, Gender);
                //  db.AddInParameter(command, "DOB", DbType.Date, DOB);
                //db.AddInParameter(command, "MobileNoCountryCode", DbType.String, MobileNoCountryCode);
                db.AddInParameter(command, "MobileNo", DbType.String, MobileNo);
                //db.AddInParameter(command, "PhoneNoCountryCode", DbType.String, PhoneNoCountryCode);
                db.AddInParameter(command, "PhoneNo", DbType.String, PhoneNo);
                db.AddInParameter(command, "CompanyID", DbType.Int64, CompanyID);

                db.AddOutParameter(command, "UserID", DbType.Int64, 10);
                db.AddOutParameter(command, "Status", DbType.Boolean, 10);
                db.AddOutParameter(command, "StatusMessage", DbType.String, 50);
                result = db.ExecuteNonQuery(command);
                UserID = Int64.Parse(db.GetParameterValue(command, "UserID").ToString());
                status = Convert.ToBoolean(db.GetParameterValue(command, "Status"));
                statusMessage = db.GetParameterValue(command, "StatusMessage").ToString();

            }
            catch (Exception ex)
            {
                status = false;
                statusMessage = ex.Message;
            }
            finally
            {

            }
            return UserID;
        }
        public int UpdateNewsLetterFlag(string EmailId, bool isNewsletter, bool isIntrestedForLuckyDraw)
        {

            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                command = db.GetStoredProcCommand("Web_UpdateNewsLetterFlag");
                db.AddInParameter(command, "EmailId", DbType.String, EmailId);
                db.AddInParameter(command, "isNewsletter", DbType.Boolean, isNewsletter);
                db.AddInParameter(command, "isIntrestedForLuckyDraw", DbType.Boolean, isIntrestedForLuckyDraw);
                result = db.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "UpdateNewsLetterFlag", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE", null);

            }
            finally
            {

            }
            return result;
        }
    }
}
