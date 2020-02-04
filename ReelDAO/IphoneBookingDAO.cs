using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ReelDao
{
    public class IphoneBookingDAO : BaseAnalizer
    {
        public bool ExecuteIhoneBookingQuaries(string quary)
        {
            Database database = null;
            DbCommand storedProcCommand;
            try
            {
                database = DatabaseFactory.CreateDatabase("ReelConString");
                storedProcCommand = database.GetStoredProcCommand(SPExecuteBookingQueries);
                database.AddInParameter(storedProcCommand, "QuaryString", DbType.String, quary);
                return database.ExecuteNonQuery(storedProcCommand) > 0;
            }
            catch (Exception )
            {
                return false;
            }
            finally
            {
                database = null;
                storedProcCommand = null;
            }
        }

        public DataTable ExecuteIhoneBookingSelectQuaries(string quary)
        {
            Database database = null;
            DbCommand storedProcCommand;
            DataTable returnDataTable = null;
            try
            {
                returnDataTable = new DataTable();
                database = DatabaseFactory.CreateDatabase("ReelConString");
                storedProcCommand = database.GetStoredProcCommand(SPExecuteBookingQueries);
                database.AddInParameter(storedProcCommand, "QuaryString", DbType.String, quary);
                returnDataTable = database.ExecuteDataSet(storedProcCommand).Tables[0];
            }
            catch (Exception)
            {
                return returnDataTable;
            }
            finally
            {
                database = null;
                storedProcCommand = null;
            }
            return returnDataTable;
        }

        public bool InsertEmailLog(string quary)
        {
            int result = 0;
            SqlConnection conn = null;
            SqlCommand comm = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ReelConString"].ConnectionString);
                conn.Open();
                comm = conn.CreateCommand();
                comm.CommandText = quary;
                result = comm.ExecuteNonQuery();
            }
            catch (Exception )
            {

            }

            finally
            {
                conn = null;
                comm = null;
            }
            return !result.Equals(0);
        }
        public string AppType(int refNumber)
        {
            Database db = null;
            DbCommand command = null;
            string app = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("ReelConString");
                command = db.GetSqlStringCommand("select AppType from iPhoneBooking where vpc_MerchTxnRef=" + refNumber + "");
                object obj = db.ExecuteScalar(command);
                app = obj.ToString();


            }
            catch (Exception )
            {


            }
            finally
            {
                db = null;
            }

            return app;
        }
    }
}
