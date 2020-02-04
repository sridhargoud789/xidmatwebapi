using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReelDvo
{
    public class BaseAnalizer
    {
        #region Modes

        public string Event = "event";
        public string Exception = "exception";
        public string DllLog = "dll";

        #endregion

        #region SPs

        public string SPInsertventAndExceptionLog = "spExeternalInsertEventAndExceptionLog";
        public string SPGetErrorMessages = "spExternalGetErrorMessages";
        public string SPExecuteBookingQueries = "spExternalInsertUpdateIphoneBookingTable";
        public string SPInsertMobileBooking = "SpInsertMobileBookingInfo";
        public string SPInsertMobileBookingInfoNonCreditCardPayment = "SpInsertMobileBookingInfoNonCreditCardPayment";
        public string SPFetchMobileBooking = "SPFetchMobileBooking";
        public string SPUpdateTransactionDetails = "SpUpdateMobileTransactionDetails";
        public string SPUpdateVistaDetails = "SpUpdateMobileVistaDetails";
        public string SpCancelMobileBooking = "SpCancelMobileBooking";
        public string SpInsertFailureEmail = "SpInsertFailureEmail";
        public string SpInsertServiceLog = "spInsertServiceLog";
        public string GetServiceSettings = "spGetAllMobileServiceSettings";
        public string SpUpdateVcoRequestDetails = "spUpdateVcoRequestDetails";
        public string SpInsertECIRule = "spInsertDMRuleLogs";
        public string SPGetOpenUBEMember = "spGetOpenUBEMemberByMemberId";
        public string SPInsertBookingInfo = "Web_InsertBookingInfo";
        public string SPInsertPaymentLog = "Mob_InsertPaymentLog";
        public string SPUpdatePaymentLog = "Mob_UpdatePaymentLog";
        public string SPInsertNoonPaymentResponse = "Web_InsertNoonPaymentResponse";
        public string SPGetBookingDetailsByOrderId = "Mob_GetBookingDetailsByOrderId";

        public string SPUpdateBookingDetails = "Mob_UpdateBookingDetails";
        public string SPGetBookingSummary = "Web_GetBookingSummary";
        public string UpdateTicketDescription = "Mob_UpdateTicketDescription";
        public string SPGetPaymentTypes = "Web_GetPaymentTypes";
        public string SPInsertLog = "Web_InsertLog";

        #endregion


        public string WEBSERVICEAUTHERRORMSG = "WEBSERVICEAUTHERRORMSG";
        public string CONS_SUCCESS = "Success";
        public string OK = "OK";
        public string ACCEPT = "ACCEPT";
        public string CANCEL = "CANCEL";
        public string REJECT = "REJECT";
        public string APPTYPE = "Reel Mobile WebService";
        public string EXCEPTION = "exception";
        public string EVENT = "event";
        public string CustomerAttempts = "CustomerAttempts";

    }
}
