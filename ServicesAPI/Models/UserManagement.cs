using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class UserRegistrationReq
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string CountryCode { get; set; }
        public string CountryShortCode { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }
        public string Nationality { get; set; }
        public bool IsNewsletter { get; set; }
        public bool UbyEmaarRegister { get; set; }
        public string DeviceType { get; set; }
        public int DataFrom { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }

    public class UserRegistrationResp
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public object SourceUrl { get; set; }
        public int Sourcedata { get; set; }
    }
    public class UserLoginReq
    {


        public string EmailId { get; set; }
        public string Password { get; set; }
        public string SocialMediaType { get; set; }
        public string DataFrom { get; set; }
        public DeviceDetails DeviceDetails { get; set; }

    }

    public class UserLoginResp
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public string SourceUrl { get; set; }
        public ULSourceData Sourcedata { get; set; }
    }

    public class ULSourceData
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailId { get; set; }
        public string countryCode { get; set; }
        public string phoneNo { get; set; }
        public string password { get; set; }
        public string gender { get; set; }
        public DateTime? dateofBirth { get; set; }
        public bool mobileNotification { get; set; }
        public string socialMediaType { get; set; }
        public string emailVerificationCode { get; set; }
        public DateTime? emailVerifiedDate { get; set; }
        public string mobileVerificationCode { get; set; }
        public DateTime? mobileVerifiedDate { get; set; }
        public bool emailVerified { get; set; }
        public bool mobileVerified { get; set; }
        public bool mailStatus { get; set; }
        public bool smsStatus { get; set; }
        public string passwordVerificationCode { get; set; }
        public int passwordVerified { get; set; }
        public DateTime? passwordVerifiedDate { get; set; }
        public string countryShortCode { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool isNewsletter { get; set; }
        public string nationality { get; set; }
        public bool isActive { get; set; }
        public int? dataFrom { get; set; }
    }

    public class GetUserDetailsReq
    {
        public int UserId { get; set; }
        public int DataFrom { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }

    public class GetUserDetailsResp
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public string SourceUrl { get; set; }
        public GetULSourcedata Sourcedata { get; set; }
    }

    public class GetULSourcedata
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string CountryCode { get; set; }
        public string CountryShortCode { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MarriedStatus { get; set; }
        public bool IsActive { get; set; }
        public string City { get; set; }
        public string ProfileImage { get; set; }
        public string Type { get; set; }
        public string MemberID { get; set; }
        public string CardNumber { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public bool IsNewsletter { get; set; }
        public string PromotionalMail { get; set; }
        public bool EmailVerified { get; set; }
        public bool MobileVerified { get; set; }
        public string SourceUrl { get; set; }
        public string EncryptData { get; set; }
        public int? DataFrom { get; set; }
    }


    public class UpdateUserReq
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string CountryCode { get; set; }
        public string CountryShortCode { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public bool IsNewsletter { get; set; }
        public int? DataFrom { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }

    public class UserResp
    {
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public string SourceUrl { get; set; }
        public string Sourcedata { get; set; }
    }

    public class ForgotPasswordReq
    {
        public string EmailId { get; set; }
        public int? DataFrom { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }

}