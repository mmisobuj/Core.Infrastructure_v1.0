using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Infrastructure.Extensions
{

    public class BulkInsertSQL
    {
        string _connectionString = "";
        public BulkInsertSQL(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool InsertTable(DataTable dtTable)
        {
            bool rv = false;
            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    var bulkCopy = new SqlBulkCopy(sqlConnection,
                        SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction, null);
                    bulkCopy.DestinationTableName = dtTable.TableName;
                    bulkCopy.WriteToServer(dtTable);
                    rv = true;
                }
            }
            catch (SqlException ex)
            {
                rv = false;
            }
            return rv;
        }
        public bool InsertTable(DataTable dtTable, string destinationTableName)
        {
            bool rv = false;
            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();
                    var bulkCopy = new SqlBulkCopy(sqlConnection,
                        SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction, null);
                    bulkCopy.DestinationTableName = destinationTableName;

                    bulkCopy.WriteToServer(dtTable);
                    sqlConnection.Close();
                    rv = true;
                }
            }
            catch (Exception ex)
            {
                rv = false;
            }

            return rv;
        }
        public bool InsertTable(DataTable dtTable, string destinationTableName, List<SqlBulkCopyColumnMapping> columnMapps)
        {
            bool rv = false;
            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();
                    var bulkCopy = new SqlBulkCopy(sqlConnection,
                        SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction, null);
                    bulkCopy.DestinationTableName = destinationTableName;
                    foreach (SqlBulkCopyColumnMapping column in columnMapps)
                    {
                        bulkCopy.ColumnMappings.Add(column);
                    }

                    bulkCopy.WriteToServer(dtTable);
                    sqlConnection.Close();
                    rv = true;
                }
            }
            catch (Exception ex)
            {
                rv = false;
            }

            return rv;
        }



    }

    //    public class Oracle
    //    {
    //    //    private OracleConnection orclConnection = null;
    //    //    public Oracle()
    //    //    {

    //    //    }
    //    //    public Oracle(CommonConnection connection)
    //    //    {

    //    //        orclConnection = connection.OracleConnection;

    //    //    }
    //    //    public string InsertTable(DataTable dtTable)
    //    //    {
    //    //        string rv = "";
    //    //        try
    //    //        {
    //    //            if (orclConnection == null)
    //    //            {
    //    //                using (var oraclConn = new CommonConnection().OracleConnection)
    //    //                {
    //    //                    if (oraclConn.State == ConnectionState.Closed) { oraclConn.Open(); }
    //    //                    using (var bulkCopy = new OracleBulkCopy(oraclConn, OracleBulkCopyOptions.UseInternalTransaction))
    //    //                    {
    //    //                        bulkCopy.DestinationTableName = dtTable.TableName;
    //    //                        bulkCopy.WriteToServer(dtTable);
    //    //                        rv = Operation.Success.ToString();

    //    //                    }
    //    //                }
    //    //            }
    //    //            else
    //    //            {
    //    //                using (orclConnection)
    //    //                {
    //    //                    if (orclConnection.State == ConnectionState.Closed) { orclConnection.Open(); }
    //    //                    using (var bulkCopy = new OracleBulkCopy(orclConnection, OracleBulkCopyOptions.UseInternalTransaction))
    //    //                    {
    //    //                        bulkCopy.DestinationTableName = dtTable.TableName;
    //    //                        bulkCopy.WriteToServer(dtTable);
    //    //                        rv = Operation.Success.ToString();

    //    //                    }
    //    //                }
    //    //            }

    //    //        }
    //    //        catch (Exception)
    //    //        {
    //    //            rv = Operation.Failed.ToString();

    //    //        }
    //    //        finally
    //    //        {
    //    //            orclConnection.Close();
    //    //        }

    //    //        return rv;


    //    //    }

    //    //    public string InsertTable(DataTable dtTable, string destinationTable)
    //    //    {
    //    //        string rv = "";
    //    //        var orclConnection = new CommonConnection().OracleConnection;
    //    //        try
    //    //        {

    //    //                orclConnection.Open();
    //    //                using (var bulkCopy = new OracleBulkCopy(orclConnection, OracleBulkCopyOptions.Default))
    //    //                {

    //    //                    bulkCopy.DestinationTableName = destinationTable;
    //    //                    //foreach (KeyValuePair<string, string> k in ColumnMappings())
    //    //                    //{
    //    //                    //    loader.ColumnMappings.Add(k.Key, k.Value);
    //    //                    //}

    //    //                    //bulkCopy.ColumnMappings.Add("DISCON_MSTR_ID", "DISCON_MSTR_ID");
    //    //                    //bulkCopy.ColumnMappings.Add("PHONE_NO", "PHONE_NO");
    //    //                    //bulkCopy.ColumnMappings.Add("SUB_NAME", "SUB_NAME");
    //    //                    //bulkCopy.ColumnMappings.Add("TOTAL_MONTH", "TOTAL_MONTH");
    //    //                    //bulkCopy.ColumnMappings.Add("TOTAL_AMOUNT", "TOTAL_AMOUNT");
    //    //                    //bulkCopy.ColumnMappings.Add("ADDRESS", "ADDRESS");

    //    //                    //bulkCopy.ColumnMappings.Add("ISSUE_DATE", DateTime.Now.ToString("MM/dd/yyyy"));

    //    //                    bulkCopy.WriteToServer(dtTable);


    //    //                    rv = Operation.Success.ToString();
    //    //                }


    //    //        }
    //    //        catch (Exception)
    //    //        {
    //    //            rv = Operation.Failed.ToString();

    //    //        }
    //    //        finally
    //    //        {
    //    //            orclConnection.Dispose();
    //    //            orclConnection.Close();

    //    //        }

    //    //        return rv;


    //    //    }

    //    //    //private static Dictionary<string, string> ColumnMappings()
    //    //    //{
    //    //    //    Dictionary<string, string> mapping = new Dictionary<string, string>();
    //    //    //    mapping.Add("last_name", "last_name");
    //    //    //    mapping.Add("first_name", "first_name");
    //    //    //    mapping.Add("middle_name", "middle_name");
    //    //    //    mapping.Add("name_suffix", "name_suffix");
    //    //    //    mapping.Add("gender_id", "gender");
    //    //    //    mapping.Add("date_of_birth", "date_of_birth");
    //    //    //    mapping.Add("is_fbo", "is_fbo");
    //    //    //    mapping.Add("confirmation_number", "confirmation_number");
    //    //    //    mapping.Add("id", "id");


    //    //    //    return mapping;
    //    //    //}
    //    //}



    //}

}
