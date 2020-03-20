using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class ServicesBO
    {
    }

    public class ManageCompanyServiceReq
    {
        public Int64 UserId { get; set; }

        public Int64 ServiceId{ get; set; }
       public bool IsActive { get; set; }
       public bool IsApproved { get; set; }
       public int Flag { get; set; }
    }

    public class ManageUserReq
    {
        public Int64 UserId { get; set; }
        public bool IsActive { get; set; }

    }

    public class EmailReq {
        public string lang { get; set; }
        public string Name { get; set; }
        public int templateCode { get; set; }
        public string to { get; set; }
        public bool IsApproved { get; set; }
        public string EmailId { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Description { get; set; }
        public string ServiceTitle { get; set; }
        public string ProductTitle { get; set; }
    }

    public class GetAllCompanyServicesReq
    {
        public Int64 MasterServiceID { get; set; }
        public Int64 CompanyID { get; set; }
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
    public class RegisterResp
    {
        public bool status { get; set; }
        public string statusMessage { get; set; }
    }
    public class ValidateUserResp
    {
        public bool status { get; set; }
        public string statusMessage { get; set; }
        public UserBO UserObject { get; set; }
    }

    public class UserBO
    {
        public Int64 UserId { get; set; }
        public int RoleId { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string MobileNoCountryCode { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNoCountryCode { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public Int64 CompanyID { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }

    public class ValidateLoginReq
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
    }

    public class RegisterReq
    {
        public string CompanyName { get; set; }
        public string Description { get; set; }

        public string EmailId { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string MobileNoCountryCode { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNoCountryCode { get; set; }
        public string PhoneNo { get; set; }
        public Int64 CompanyID { get; set; }

        public string Filenames { get; set; }
        public string Filepaths { get; set; }
        public string FileIds { get; set; }
        public string CountryCode { get; set; }


    }
    
    public class CreateServiceReq
    {
        public Int64 UserId { get; set; }

        public Int64 CompanyID { get; set; }
        public int MasterServiceID { get; set; }
        public Int64 CreatedBy { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        public string Timings { get; set; }
        public string Filenames { get; set; }
        public string Filepaths { get; set; }
        public string FileIds { get; set; }
        public string CountryCode { get; set; }
    }

    public class ManageProductsReq
    {
        public int MasterProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public Int64 UserId { get; set; }
        public string FileIds { get; set; }


    }
  public class ProductRequestReq
    {
        public Int64 MyProductId { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string Description { get; set; }
    }
    public class ServiceRequestReq
    {
        public Int64 CompanyServiceID { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string MobileNoCC { get; set; }
        public string MobileNo { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
    }

    public class AddServiceViewCountReq
    {
        public Int64 ServiceID { get; set; }
    }

    public class GetAllServicesRequestsReq
    {

        public Int64 CompanyId { get; set; }
        public Int64 MasterServiceId { get; set; }
        public Int64 CompanyServiceId { get; set; }

    }
 public class GetAllProductsReq
    {

        public Int64 MasterProductId { get; set; }
        public Int64 UserId { get; set; }

    }


}