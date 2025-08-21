using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using System.Linq;
using System.Web;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace backEnd.clases
{
    public class DBConnection
    {
        /*------------------------------------------------------------------*/
        /*-------------------------DB Methods-------------------------------*/
        /// <summary>
        /// Name of connection using in IIS
        /// </summary>
        /// <param name="Server_DB">Receive name conection.</param>
        /// <returns>String connection</returns>
        public string StringConnection(string Server_DB)
        {
            //var conexion = configuration.GetValue<string>("ConnectionStrings:DefaultDatabase");
            //var conexion = "Server=.\\SQLExpress;Database=promopolis2020;Trusted_Connection=True;";

            // Establecemos el archivo de configuracion que deseamos leer
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var conexion = configuration.GetValue<string>("ConnectionStrings:DefaultDatabase");

            return conexion;
        }
        /// <summary>
        /// Get a data set from query select.(Parameter should be declared like @Example in query)
        /// </summary>
        /// <param name="Server_DB">Name of connection for be used</param>
        /// <param name="query">Query for getting data set</param>
        /// <param name="Parameters">List of sqlparameters</param>
        /// <returns>Dataset</returns>
        public DataSet GetDataSet_fromSelect(string Server_DB, string query, List<SqlParameter> Parameters)
        {
            SqlConnection objConnection = new SqlConnection(StringConnection(Server_DB));
            SqlCommand objCommand = new SqlCommand(query, objConnection);
            if (Parameters != null)
                objCommand.Parameters.AddRange(Parameters.ToArray());

            SqlDataAdapter objAdapater = new SqlDataAdapter();
            objAdapater.SelectCommand = objCommand;
            DataSet ObjDataset = new DataSet();
            objConnection.Open();
            try
            {
                objAdapater.Fill(ObjDataset);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                objConnection.Close();
            }
            return ObjDataset;
        }
        /// <summary>
        /// Exec procedure with list of sql paramters.(Parameter should be declared like @Example in query)
        /// </summary>
        /// <param name="Parameters">List of sqlparameters</param>
        /// <param name="Server_DB">Server connection name</param>
        /// <param name="ProcedureName">Name of procedure</param>
        /// <returns>Dataset</returns>
        public DataSet ExecProcedure(List<SqlParameter> Parameters, string Server_DB, string ProcedureName)
        {
            SqlConnection SqlConn = new SqlConnection(StringConnection(Server_DB));
            SqlCommand SqlCmd = new SqlCommand(ProcedureName, SqlConn);
            SqlDataAdapter objAdapater = new SqlDataAdapter();

            DataSet dtab = new DataSet();
            SqlCmd.CommandType = CommandType.StoredProcedure;
            if (Parameters != null)
                SqlCmd.Parameters.AddRange(Parameters.ToArray());

            objAdapater.SelectCommand = SqlCmd;
            SqlConn.Open();
            try
            {
                objAdapater.Fill(dtab);

            }
            catch (Exception ex)
            {
                //Error ex

            }
            finally
            {
                SqlCmd.Parameters.Clear();
                SqlConn.Close();
            }
            return dtab;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="Server_DB"></param>
        /// <param name="ProcedureName"></param>
        /// <returns>Datatable</returns>
        public DataTable ExecProcedure_getDataReader(List<SqlParameter> Parameters, string Server_DB, string ProcedureName, string SourceApp)
        {
            SqlConnection SqlConn = new SqlConnection(StringConnection(Server_DB));
            SqlCommand SqlCmd = new SqlCommand(ProcedureName, SqlConn);
            DataTable dt = new DataTable();
            SqlDataReader DReader = null;
            SqlCmd.CommandTimeout = 120;
            SqlCmd.CommandType = CommandType.StoredProcedure;
            if (Parameters != null)
                SqlCmd.Parameters.AddRange(Parameters.ToArray());
            SqlConn.Open();
            try
            {
                DReader = SqlCmd.ExecuteReader();
                dt.Load(DReader);
            }
            catch (Exception ex)
            {
                //
                //ex.Source;

                //description ex.Message;
                //user db id
                //source : SourceApp
            }
            finally
            {
                SqlCmd.Parameters.Clear();
                SqlConn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Execute nonquery (Insert, Update, Delete) and get quantity rows affected. (Parameter should be declared like @Example in query)
        /// </summary>
        /// <param name="Server_DB">Server connection name</param>
        /// <param name="query">Instruction for be executed</param>
        /// <param name="Parameters">List of sqlparameters needed</param>
        /// <returns>Int quantity of rows</returns>
        public int ExecuteNonQuery_getRowsAff(string Server_DB, string query, List<SqlParameter> Parameters)
        {
            SqlConnection SqlConn = new SqlConnection(StringConnection(Server_DB));
            SqlCommand SqlCmd = new SqlCommand(query, SqlConn);
            SqlCmd.Parameters.AddRange(Parameters.ToArray());
            int RowsAffected = 0;
            SqlCmd.CommandType = CommandType.Text;
            SqlConn.Open();
            try
            {
                RowsAffected = SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally
            {
                SqlCmd.Parameters.Clear();
                SqlConn.Close();
            }
            return RowsAffected;
        }
        /// <summary>
        /// Get table from query sql. (Parameter should be declared like @Example in query)
        /// </summary>
        /// <param name="Server_DB">Name of server connection</param>
        /// <param name="query">Query of sql for getting table</param>
        /// <param name="Parameters">List of sqlparamters needed</param>
        /// <returns>Datatable</returns>
        public DataTable GetTable(string Server_DB, string query, List<SqlParameter> Parameters)
        {
            SqlConnection SqlConn = new SqlConnection(StringConnection(Server_DB));
            SqlCommand SqlCmd = new SqlCommand(query, SqlConn);
            if (Parameters != null)
                SqlCmd.Parameters.AddRange(Parameters.ToArray());
            DataTable rows = new DataTable();
            SqlCmd.CommandType = CommandType.Text;
            SqlConn.Open();

            try
            {
                rows.Load(SqlCmd.ExecuteReader());
            }
            catch (Exception ex) { }
            finally
            {
                SqlCmd.Parameters.Clear();
                SqlConn.Close();
            }
            return rows;
        }
        /// <summary>
        /// Get once data, get scalar from DB. (Parameter should be declared like @Example in query)
        /// </summary>
        /// <param name="Server_DB">Name of connection db</param>
        /// <param name="query">query for getting just once date</param>
        /// <param name="Parameters">List of sqlparamters needed</param>
        /// <returns>String of scalar</returns>
        public string getScalar(string Server_DB, string query, List<SqlParameter> Parameters)
        {
            SqlConnection SqlConn = new SqlConnection(StringConnection(Server_DB));
            SqlCommand SqlCmd = new SqlCommand(query, SqlConn);
            String Escalarvalue = "";
            SqlCmd.CommandType = CommandType.Text;
            SqlCmd.Parameters.AddRange(Parameters.ToArray());
            SqlConn.Open();

            try
            {
                Escalarvalue = SqlCmd.ExecuteScalar().ToString();
            }
            catch (Exception ex) { }
            finally
            {
                SqlCmd.Parameters.Clear();
                SqlConn.Close();
            }
            return Escalarvalue;
        }

        public string GetFn(string Server_DB, string FN, List<SqlParameter> Parameters)
        {
            SqlConnection SqlConn = new SqlConnection(StringConnection(Server_DB));
            SqlCommand SqlCmd = new SqlCommand();
            SqlCmd.Connection = SqlConn;
            SqlCmd.CommandType = CommandType.Text;
            SqlCmd.CommandText = FN;
            if (Parameters != null)
                SqlCmd.Parameters.AddRange(Parameters.ToArray());
            string dt = "";
            SqlConn.Open();
            try
            {
                dt = SqlCmd.ExecuteScalar().ToString();
            }
            catch (Exception ex) { }

            finally
            {
                SqlCmd.Parameters.Clear();
                SqlConn.Close();
            }
            return dt;
        }
    }
}