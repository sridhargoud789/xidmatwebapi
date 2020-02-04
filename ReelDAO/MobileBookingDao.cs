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
    public class MobileBookingDao
    {
        private Database db = null;
        private LogDao log = null;
        public DataTable GetServiceSettings()
        {
            BaseAnalizer baseAnalizer = null;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.GetServiceSettings);
                db.AddInParameter(command, "Flag", DbType.String, "Get");
                return db.ExecuteDataSet(command).Tables[0];
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllLines(@"E:\Web\RevampedReelMobileWebService\testttttttt.txt", new[] { ex.Message + "==" + ex.StackTrace });
                //System.IO.File.WriteAllText("text.txt", ex.Message + "==" + ex.StackTrace);

                new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "GetServiceSettings", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);
                return null;
            }
            finally
            {

            }
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
                new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "UpdateNewsLetterFlag", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);

            }
            finally
            {

            }
            return result;
        }
        public int InsertSavedCardInfo(int userId, string tokenIdentifier, string paymentInfo, string brand, string cardType, int expiryMonth, int expiryYear, string cardName, string EmailId)
        {

            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                command = db.GetStoredProcCommand("Web_InsertSavedCardInfo");
                db.AddInParameter(command, "userId", DbType.Int32, userId);

                db.AddInParameter(command, "tokenIdentifier", DbType.String, tokenIdentifier);
                db.AddInParameter(command, "paymentInfo", DbType.String, paymentInfo);
                db.AddInParameter(command, "brand", DbType.String, brand);

                db.AddInParameter(command, "cardType", DbType.String, cardType);
                db.AddInParameter(command, "expiryMonth", DbType.Int16, expiryMonth);

                db.AddInParameter(command, "expiryYear", DbType.Int16, expiryYear);
                db.AddInParameter(command, "cardName", DbType.String, cardName);
                db.AddInParameter(command, "EmailId", DbType.String, EmailId);

                result = db.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "InsertSavedCardInfo", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);

            }
            finally
            {

            }
            return result;
        }

        public DataTable GetSavedCard(int userId, int cardId)
        {
            try
            {
                log = new LogDao();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand("Web_GetSavedCard");
                db.AddInParameter(command, "userId", DbType.Int32, userId);
                db.AddInParameter(command, "cardId", DbType.Int32, cardId);
                return db.ExecuteDataSet(command).Tables[0];
            }

            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "GetSavedCard", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);

                return null;

            }
            finally
            {
            }
        }

        public int DeleteSavedCard(int cardId)
        {
            int result = 0;
            try
            {
                log = new LogDao();

                DbCommand command = null;
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                command = db.GetStoredProcCommand("Web_DeleteSavedCard");
                db.AddInParameter(command, "cardId", DbType.Int32, cardId);
                result = db.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                new LogDao().InsertLog("", "ERROR", "MobileBookingDao", "DeleteSavedCard", "", "REQUEST", JsonConvert.SerializeObject(ex), "MOBILE",null);

            }
            finally
            {

            }
            return result;
        }

    }
}
