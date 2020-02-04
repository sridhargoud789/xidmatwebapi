using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Configuration;

using ReelDvo;
using ReelDVO;

namespace ReelDAO
{
    public class Analizer
    {



        public void WriteEventAndExceptions(string record, string deviceType, string userSessionId, string mode = null)
        {
            EventAndExceptionsDAO eventAndExceptionsOp = null;
            EventAndExceptionsDVO eventAndException = null;
            BaseAnalizer baseAnalizer = null;
            try
            {
                baseAnalizer = new BaseAnalizer();
                eventAndException = new EventAndExceptionsDVO()
                {
                    AppType = baseAnalizer.APPTYPE,
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


        public void WriteOptionalEventAndExceptions(string record, string deviceType, string userSessionId, string mode = null)
        {


            EventAndExceptionsDAO eventAndExceptionsOp = null;
            EventAndExceptionsDVO eventAndException = null;
            BaseAnalizer baseAnalizer = null;
            try
            {
                //if (ConfigurationManager.AppSettings["WriteOptionalLog"].ToString() == "Y")
                //{
                baseAnalizer = new BaseAnalizer();
                eventAndException = new EventAndExceptionsDVO()
                {
                    AppType = baseAnalizer.APPTYPE,
                    Device = deviceType,
                    Record = record,
                    Session = userSessionId

                };
                eventAndException.Mode = mode == null ? eventAndException.Mode : mode;
                eventAndExceptionsOp = new EventAndExceptionsDAO();
                eventAndExceptionsOp.InsertEventAndExceptionLogs(eventAndException);

                //}

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


        public Int64 InsertServiceLog(Int64 logId, string logType, string logData, string device = "", string userSessionId = "", string sessionId = ""
            , string cinemaId = "", string reelServiceName = "", string serviceCall = "", string bookingId = "",DeviceDetails dd = null)
        {


            EventAndExceptionsDAO eventAndExceptionsOp = null;
            ServiceLogDvo oServiceLogDvo = null;
            BaseAnalizer baseAnalizer = null;
            try
            {

                baseAnalizer = new BaseAnalizer();
                oServiceLogDvo = new ServiceLogDvo()
                {
                    LogId = logId,
                    UserSession = userSessionId,
                    SessionId = sessionId,
                    CinemaId = cinemaId,
                    BookingId = bookingId,
                    ReelServiceName = reelServiceName,
                    ServiceCall = serviceCall,
                    Device = device,
                    LogType = logType,
                    LogData = logData
                };
                //eventAndException.Mode = mode == null ? eventAndException.Mode : mode;
                eventAndExceptionsOp = new EventAndExceptionsDAO();
                return eventAndExceptionsOp.InsertServiceLogs(oServiceLogDvo,dd);
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                oServiceLogDvo = null;
                eventAndExceptionsOp = null;
            }
        }

        public Int64 UpdateServiceLog(Int64 logId, string logType, string logData, string serviceStatus = "", DeviceDetails dd = null)
        {


            EventAndExceptionsDAO eventAndExceptionsOp = null;
            ServiceLogDvo oServiceLogDvo = null;
            BaseAnalizer baseAnalizer = null;
            try
            {
                baseAnalizer = new BaseAnalizer();
                oServiceLogDvo = new ServiceLogDvo()
                {
                    LogId = logId,
                    LogType = logType,
                    LogData = logData,
                    ServiceStatus = serviceStatus
                };
                //eventAndException.Mode = mode == null ? eventAndException.Mode : mode;
                eventAndExceptionsOp = new EventAndExceptionsDAO();
                return eventAndExceptionsOp.InsertServiceLogs(oServiceLogDvo,dd);
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                oServiceLogDvo = null;
                eventAndExceptionsOp = null;
            }
        }

  
    }
}