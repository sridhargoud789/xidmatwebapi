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
        public DataTable GetAllCompanyServices(Int64 MasterServiceID)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase("ServicesConString");
                DbCommand command = db.GetStoredProcCommand("GetAllCompanyServices");
                db.AddInParameter(command, "MasterServiceID", DbType.Int64, MasterServiceID);

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
