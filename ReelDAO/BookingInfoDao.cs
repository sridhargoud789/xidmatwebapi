using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json;
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
    public class BookingInfoDao
    {
        private Database db = null;
        private LogDao log = null;
        public int InsertPaymentLog(string UserSessionId,string OptionalClientId, Int64 SessionId, string Request, decimal amount, string tokenIdentifier, string App_Source, out int LogId,
            int UserId, string CustomerEmail, string CustomerPhone,string PhoneCountryCode, string CustomerName,int bookingInfoId, bool isNewsLetter,bool isIntrestedForLuckyDraw)
        {

            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.SPInsertPaymentLog);

                db.AddInParameter(command, "userSessionId", DbType.String, UserSessionId);
                db.AddInParameter(command, "OptionalClientId", DbType.String, OptionalClientId);
                
                db.AddInParameter(command, "SessionId", DbType.Int64, SessionId);
                db.AddInParameter(command, "Request", DbType.String, Request);
                db.AddInParameter(command, "amount", DbType.Decimal, amount);
                db.AddInParameter(command, "tokenIdentifier", DbType.String, tokenIdentifier);
                db.AddInParameter(command, "App_Source", DbType.String, App_Source);

                db.AddInParameter(command, "UserId", DbType.Int32, UserId);
                db.AddInParameter(command, "bookingInfoId", DbType.Int32, bookingInfoId);
                db.AddInParameter(command, "CustomerEmail", DbType.String, CustomerEmail);
                db.AddInParameter(command, "CustomerPhone", DbType.String, CustomerPhone);
                db.AddInParameter(command, "PhoneCountryCode", DbType.String, PhoneCountryCode);
                db.AddInParameter(command, "CustomerName", DbType.String, CustomerName);
                db.AddInParameter(command, "isNewsLetter", DbType.Boolean, isNewsLetter);
                db.AddInParameter(command, "isIntrestedForLuckyDraw", DbType.Boolean, isIntrestedForLuckyDraw);
                

                db.AddOutParameter(command, "LogId", DbType.Int16, 10);

                result = db.ExecuteNonQuery(command);
                LogId = int.Parse(db.GetParameterValue(command, "LogId").ToString());

            }
            catch (Exception ex)
            {
                LogId = 0;                
                new LogDao().InsertLog(UserSessionId, "ERROR", "BookingInfoDao", "InsertPaymentLog", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);
            }
            finally
            {

            }
            return result;
        }

        public int UpdateBookingDetails(Int64 bookingInfoId, string bookingId, string vistaBookingNo, string vistaTransNo,bool pgStatus,string paymentMessage)
        {

            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.SPUpdateBookingDetails);
                db.AddInParameter(command, "bookingInfoId", DbType.Int64, bookingInfoId);
                db.AddInParameter(command, "bookingId", DbType.String, bookingId);
                db.AddInParameter(command, "vistaBookingNo", DbType.String, vistaBookingNo);
                db.AddInParameter(command, "vistaTransNo", DbType.String, vistaTransNo);
                db.AddInParameter(command, "pgStatus", DbType.Boolean, pgStatus);
                db.AddInParameter(command, "paymentMessage", DbType.String, paymentMessage);
                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "BookingInfoDao", "UpdateBookingDetails", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);
            }
            finally
            {

            }
            return result;
        }

        public int UpdateTicketDescription(Int64 bookingInfoId, string TicketDescription,decimal totalAmount,string fooddesc)
        {

            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.UpdateTicketDescription);
                db.AddInParameter(command, "bookingInfoId", DbType.Int64, bookingInfoId);
                db.AddInParameter(command, "TicketDescription", DbType.String, TicketDescription);
                db.AddInParameter(command, "fooddesc", DbType.String, fooddesc);

                
                db.AddInParameter(command, "totalAmount", DbType.Decimal, totalAmount);
                
                result = db.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "BookingInfoDao", "UpdateTicketDescription", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);
            }
            finally
            {

            }
            return result;
        }
        public int UpdatePaymentLog(int LogId,Int64 bookingInfoId, Int64 OrderId, string Response, string statusCode, string statusMessage,string EncryptedCardNo)
        {

            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.SPUpdatePaymentLog);
                db.AddInParameter(command, "LogId", DbType.Int16, LogId);
                db.AddInParameter(command, "bookingInfoId", DbType.Int64, bookingInfoId);
                db.AddInParameter(command, "OrderId", DbType.Int64, OrderId);
                db.AddInParameter(command, "Response", DbType.String, Response);
                db.AddInParameter(command, "statusCode", DbType.String, statusCode);
                db.AddInParameter(command, "statusMessage", DbType.String, statusMessage);
                db.AddInParameter(command, "EncryptedCardNo", DbType.String, EncryptedCardNo);

                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                LogId = 0;
                new LogDao().InsertLog("", "ERROR", "BookingInfoDao", "UpdatePaymentLog", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);
            }
            finally
            {

            }
            return result;
        }
        public int InsertBookingInfo(InsertBookingInfo obj, DeviceDetails dd)
        {
            // ResultReturnDvo resultObj = null;
            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.SPInsertBookingInfo);
                db.AddInParameter(command, "userId", DbType.Int32, obj.userId);
                db.AddInParameter(command, "userSessionId", DbType.String, obj.userSessionId);
                db.AddInParameter(command, "movieId", DbType.String, obj.movieId);
                db.AddInParameter(command, "movieName", DbType.String, obj.movieName);
                db.AddInParameter(command, "genre", DbType.String, obj.genre);
                db.AddInParameter(command, "language", DbType.String, obj.language);
                db.AddInParameter(command, "cinemaId", DbType.String, obj.cinemaId);
                db.AddInParameter(command, "cinemaName", DbType.String, obj.cinemaName);
                db.AddInParameter(command, "screenName", DbType.String, obj.screenName);
                db.AddInParameter(command, "screenNo", DbType.String, obj.screenNo);
                db.AddInParameter(command, "experience", DbType.String, obj.experience);
                db.AddInParameter(command, "rating", DbType.String, obj.rating);
                db.AddInParameter(command, "sessionId", DbType.String, obj.sessionId);
                db.AddInParameter(command, "noOfSeats", DbType.Int16, obj.noOfSeats);
                db.AddInParameter(command, "showdate", DbType.DateTime, obj.showdate);
                db.AddInParameter(command, "showtime", DbType.DateTime, obj.showtime);
                db.AddInParameter(command, "ticketAmount", DbType.Decimal, obj.ticketAmount);
                db.AddInParameter(command, "totalAmount", DbType.Decimal, obj.totalAmount);
                db.AddInParameter(command, "seatInfo", DbType.String, obj.seatInfo);
                db.AddInParameter(command, "ticketTypeCode", DbType.String, obj.ticketTypeCode);
                db.AddInParameter(command, "ticketDescription", DbType.String, obj.ticketDescription);
                db.AddInParameter(command, "bookingdate", DbType.DateTime, obj.bookingdate);
                db.AddInParameter(command, "offerId", DbType.Int16, obj.offerId);
                db.AddInParameter(command, "offerQuantity", DbType.Int16, obj.offerQuantity);
                db.AddInParameter(command, "promoCode", DbType.String, obj.promoCode);
                db.AddInParameter(command, "voucherCode", DbType.String, obj.voucherCode);
                db.AddInParameter(command, "VatAmount", DbType.Decimal, obj.VatAmount);
                db.AddInParameter(command, "foodDesc", DbType.String, obj.foodDesc);
                db.AddInParameter(command, "foodAmount", DbType.Decimal, obj.foodAmount);
                db.AddInParameter(command, "memberIdOrEmailId", DbType.String, obj.memberIdOrEmailId);
                db.AddInParameter(command, "savings", DbType.Decimal, obj.savings);
                db.AddInParameter(command, "offerDescription", DbType.String, obj.offerDescription);

                if (dd != null)
                {
                    db.AddInParameter(command, "DeviceName", DbType.String, dd.DeviceName);
                    db.AddInParameter(command, "DeviceOSType", DbType.String, dd.DeviceOSType);
                    db.AddInParameter(command, "DeviceOSVersion", DbType.String, dd.DeviceOSVersion);
                    db.AddInParameter(command, "AppVersion", DbType.String, dd.AppVersion);
                }

                DataTable dt = new DataTable();
                dt = db.ExecuteDataSet(command).Tables[0];
                result = int.Parse(dt.Rows[0]["UserId"].ToString());

            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "BookingInfoDao", "InsertBookingInfo", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);
                //log.WriteEventAndExceptions(ex.Message, transactionDvo.CreatedBy, transactionDvo.CreatedBy, transactionDvo.SessionId, null);                
            }
            finally
            {

            }
            return result;
        }
        public int InsertNoonPaymentResponse(InsertNoonPaymentResp obj)
        {
            // ResultReturnDvo resultObj = null;
            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.SPInsertNoonPaymentResponse);
                db.AddInParameter(command, "bookingInfoId", DbType.Int64, obj.bookingInfoId);
                db.AddInParameter(command, "resultCode", DbType.Int16, obj.resultCode);
                db.AddInParameter(command, "message", DbType.String, obj.message);
                db.AddInParameter(command, "requestReference", DbType.String, obj.requestReference);
                db.AddInParameter(command, "authorizationCode", DbType.String, obj.authorizationCode);
                db.AddInParameter(command, "transactionStatus", DbType.String, obj.transactionStatus);
                db.AddInParameter(command, "reasonCode", DbType.String, obj.reasonCode);
                db.AddInParameter(command, "amount", DbType.Decimal, obj.amount);
                db.AddInParameter(command, "orderId", DbType.Int64, obj.orderId);
                db.AddInParameter(command, "orderStatus", DbType.String, obj.orderStatus);
                db.AddInParameter(command, "tokenIdentifier", DbType.String, obj.tokenIdentifier);
                db.AddInParameter(command, "paymentMechanism", DbType.String, obj.paymentMechanism);
                db.AddInParameter(command, "paymentInfo", DbType.String, obj.paymentInfo);
                db.AddInParameter(command, "brand", DbType.String, obj.brand);
                db.AddInParameter(command, "cardType", DbType.String, obj.cardType);
                db.AddInParameter(command, "expiryMonth", DbType.Int16, obj.expiryMonth);
                db.AddInParameter(command, "expiryYear", DbType.Int32, obj.expiryYear);
                db.AddInParameter(command, "App_Source", DbType.String, obj.App_Source);


                result = db.ExecuteNonQuery(command);


            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "BookingInfoDao", "InsertNoonPaymentResponse", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);
                //log.WriteEventAndExceptions(ex.Message, transactionDvo.CreatedBy, transactionDvo.CreatedBy, transactionDvo.SessionId, null);                
            }
            finally
            {

            }
            return result;
        }

        public DataTable GetBookingDetailsByOrderId(Int64 OrderId)
        {
            // ResultReturnDvo resultObj = null;
            DataTable dt = new DataTable();
            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.SPGetBookingDetailsByOrderId);
                db.AddInParameter(command, "OrderId", DbType.Int64, OrderId);
                
                
                dt = db.ExecuteDataSet(command).Tables[0];

            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "BookingInfoDao", "GetBookingDetailsByOrderId", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);
                //log.WriteEventAndExceptions(ex.Message, transactionDvo.CreatedBy, transactionDvo.CreatedBy, transactionDvo.SessionId, null);                
            }
            finally
            {

            }
            return dt;
        }

        public DataTable GetBookingSummary(Int64 bookingInfoId)
        {
            // ResultReturnDvo resultObj = null;
            DataTable dt = new DataTable();
            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.SPGetBookingSummary);
                db.AddInParameter(command, "bookingInfoId", DbType.Int64, bookingInfoId);


                dt = db.ExecuteDataSet(command).Tables[0];

            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "BookingInfoDao", "GetBookingSummary", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);

                //log.WriteEventAndExceptions(ex.Message, transactionDvo.CreatedBy, transactionDvo.CreatedBy, transactionDvo.SessionId, null);                
            }
            finally
            {

            }
            return dt;
        }


        public DataTable GetPaymentTypes(string cinemaId)
        {
            // ResultReturnDvo resultObj = null;
            DataTable dt = new DataTable();
            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.SPGetPaymentTypes);
                db.AddInParameter(command, "cinemaId", DbType.String, cinemaId);
                dt = db.ExecuteDataSet(command).Tables[0];

            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "BookingInfoDao", "GetPaymentTypes", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE", null);

                //log.WriteEventAndExceptions(ex.Message, transactionDvo.CreatedBy, transactionDvo.CreatedBy, transactionDvo.SessionId, null);                
            }
            finally
            {

            }
            return dt;
        }
    }
}
