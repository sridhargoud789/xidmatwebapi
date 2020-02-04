using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReelDvo;
using ReelDVO;

namespace ReelDAO
{
    public class EventAndExceptionsDAO : BaseAnalizer
    {
        public bool InsertEventAndExceptionLogs(EventAndExceptionsDVO eventAndException)
        {

            Database database = null;
            DbCommand storedProcCommand;
            try
            {
                database = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                storedProcCommand = database.GetStoredProcCommand(SPInsertventAndExceptionLog);
                database.AddInParameter(storedProcCommand, "Mode", DbType.String, eventAndException.Mode);
                database.AddInParameter(storedProcCommand, "Record", DbType.String, eventAndException.Record);
                database.AddInParameter(storedProcCommand, "Session", DbType.String, eventAndException.Session);
                database.AddInParameter(storedProcCommand, "AppType", DbType.String, eventAndException.AppType);
                database.AddInParameter(storedProcCommand, "DeviceType", DbType.String, eventAndException.Device);
                return database.ExecuteNonQuery(storedProcCommand) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                database = null;
                storedProcCommand = null;
            }
        }
        


        public Int64 InsertServiceLogs(ServiceLogDvo serviceLogDvo, DeviceDetails dd)
        {

            Database database = null;
            DbCommand storedProcCommand;
            DataSet ds = null;
            try
            {
                database = DatabaseFactory.CreateDatabase("ReelCinemasConString");
                storedProcCommand = database.GetStoredProcCommand(SpInsertServiceLog);
                database.AddInParameter(storedProcCommand, "LogId", DbType.String, serviceLogDvo.LogId);
                database.AddInParameter(storedProcCommand, "UserSession", DbType.String, serviceLogDvo.UserSession);
                database.AddInParameter(storedProcCommand, "SessionId", DbType.String, serviceLogDvo.SessionId);
                database.AddInParameter(storedProcCommand, "CinemaId", DbType.String, serviceLogDvo.CinemaId);
                database.AddInParameter(storedProcCommand, "BookingId", DbType.String, serviceLogDvo.BookingId);
                database.AddInParameter(storedProcCommand, "ReelServiceName", DbType.String, serviceLogDvo.ReelServiceName);
                database.AddInParameter(storedProcCommand, "ServiceCall", DbType.String, serviceLogDvo.ServiceCall);
                database.AddInParameter(storedProcCommand, "Device", DbType.String, serviceLogDvo.Device);
                database.AddInParameter(storedProcCommand, "LogType", DbType.String, serviceLogDvo.LogType);
                database.AddInParameter(storedProcCommand, "LogData", DbType.String, serviceLogDvo.LogData);
                database.AddInParameter(storedProcCommand, "ServiceStatus", DbType.String, serviceLogDvo.ServiceStatus);
                if (dd !=null)
                {
                    database.AddInParameter(storedProcCommand, "DeviceName", DbType.String, dd.DeviceName);
                    database.AddInParameter(storedProcCommand, "DeviceOSType", DbType.String, dd.DeviceOSType);
                    database.AddInParameter(storedProcCommand, "DeviceOSVersion", DbType.String, dd.DeviceOSVersion);
                    database.AddInParameter(storedProcCommand, "AppVersion", DbType.String, dd.AppVersion);
                }
                
                //database.AddOutParameter(storedProcCommand, "InsertId", DbType.Int64, 10);

                ds = database.ExecuteDataSet(storedProcCommand);
                if (ds.Tables.Count > 0)
                {
                    return Int64.Parse(ds.Tables[0].Rows[0]["LogId"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                database = null;
                storedProcCommand = null;
            }
        }
    }
}
