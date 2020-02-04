using Microsoft.Practices.EnterpriseLibrary.Data;
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
    public class LogDao
    {
        private Database db = null;
        private LogDao log = null;

        public void InsertLog(string userSessionId, string type, string applicationName, string funcationName, string customData,
            string requestType, string Description, string requestSource,DeviceDetails dd)
        {
            BaseAnalizer baseAnalizer = null;
            int result = 0;
            try
            {
                log = new LogDao();
                baseAnalizer = new BaseAnalizer();
                db = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                DbCommand command = db.GetStoredProcCommand(baseAnalizer.SPInsertLog);

                db.AddInParameter(command, "userSessionId", DbType.String, userSessionId);
                db.AddInParameter(command, "type", DbType.String, type);
                db.AddInParameter(command, "applicationName", DbType.String, applicationName);
                db.AddInParameter(command, "funcationName", DbType.String, funcationName);
                db.AddInParameter(command, "customData", DbType.String, customData);
                db.AddInParameter(command, "requestType", DbType.String, requestType);
                db.AddInParameter(command, "Description", DbType.String, Description);
                db.AddInParameter(command, "requestSource", DbType.String, requestSource);

                if (dd != null)
                {
                    db.AddInParameter(command, "DeviceName", DbType.String, dd.DeviceName);
                    db.AddInParameter(command, "DeviceOSType", DbType.String, dd.DeviceOSType);
                    db.AddInParameter(command, "DeviceOSVersion", DbType.String, dd.DeviceOSVersion);
                    db.AddInParameter(command, "AppVersion", DbType.String, dd.AppVersion);
                }
                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                db = null;
                baseAnalizer = null;
            }
        }
        public void WriteEventAndExceptions(string record, string appType, string deviceType, string userSessionId, string mode = null)
        {
            EventAndExceptionsDAO eventAndExceptionsOp = null;
            EventAndExceptionsDVO eventAndException = null;
            try
            {
                eventAndException = new EventAndExceptionsDVO()
                {
                    AppType = appType,
                    Device = deviceType,
                    Record = record,
                    Session = userSessionId
                };
                eventAndException.Mode = mode == null ? eventAndException.Mode : mode;
                eventAndExceptionsOp = new EventAndExceptionsDAO();
                eventAndExceptionsOp.InsertEventAndExceptionLogs(eventAndException);
            }
            catch (Exception)
            {

            }
            finally
            {
                eventAndException = null;
                eventAndExceptionsOp = null;
            }
        }
    }
}
