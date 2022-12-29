using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.Grid;
using Dapper;


namespace Core.Infrastructure.Extensions
{
    public static class DapperExtension
    {
        public static async Task<GridEntity<T>> PagingData<T>(this IDbConnection cnn, string query, string orderByColumn, GridOptions options, string condition = "")
        {

            try
            {

                query = query.Replace(';', ' ');
                string orderby = "";
                string orderBy = orderByColumn;
                //string condition = "";
                string sqlQuery = query;
                if (options != null)
                {
                    if (options.pageSize > 0)
                    {
                        sqlQuery = GridQueryBuilder<T>.Query(options, query, orderBy, condition);
                    }
                    else
                    {
                        if (orderBy != "")
                        {
                            if (orderBy.ToLower().Contains("asc") || orderBy.ToLower().Contains("desc"))
                            {
                                orderby = string.Format(" order by {0}", orderBy);
                            }
                            else
                            {
                                orderby = string.Format(" order by {0} asc ", orderBy);
                            }
                        }
                    }
                }
                else
                {
                    if (orderBy != "")
                    {
                        if (orderBy.ToLower().Contains("asc") || orderBy.ToLower().Contains("desc"))
                        {
                            orderby = string.Format(" order by {0}", orderBy);
                        }
                        else
                        {
                            orderby = string.Format(" order by {0} asc ", orderBy);
                        }
                    }
                }

                //if (!string.IsNullOrEmpty(condition))
                //{
                //    condition = " WHERE " + condition;
                //}

                var condition1 = "";
                if (options != null)
                {
                    if (options.filter != null)
                    {
                        condition1 = GridQueryBuilder<T>.FilterCondition(options.filter).Trim();
                    }
                }
                if (!string.IsNullOrEmpty(condition1))
                {
                    if (!string.IsNullOrEmpty(condition))
                    {
                        condition += " And " + condition1;
                    }
                    else
                    {
                        condition = " WHERE " + condition1;
                    }
                }
                sqlQuery = "SELECT * FROM (" + sqlQuery + " ) As tbl " + condition;

                // DataTable dataTable = _conn.GetDataTable(sqlQuery + orderby);


                string finalQuery = sqlQuery + orderby;
                string sqlCount = "";
                int totalCount = 0;
                if (options != null)
                {
                    sqlCount = "SELECT cast(COUNT(*) AS INT) AS  Count FROM (" + query + " ) As tbl " + condition;


                    var count = await cnn.ExecuteScalarAsync(sqlCount);
                    totalCount = Convert.ToInt32(count);
                }
                var dataList = cnn.Query<T>(finalQuery).ToList();
                var result = new GridResult<T>().Data(dataList, totalCount);
                return await Task.Run(() => result);

                //return await result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //cnn.Close();
            }

            return await Task.Run(() => new GridEntity<T>());
        }

        public static bool BulkInsert(this IDbConnection cnn, DataTable table, string destinationTableName = "", List<SqlBulkCopyColumnMapping> columnMapps = null)
        {
            BulkInsertSQL bulkInsertSQL = new BulkInsertSQL(cnn.ConnectionString);
            if (destinationTableName != "")
                return bulkInsertSQL.InsertTable(table, destinationTableName);
            else if (destinationTableName != "" && columnMapps != null)
                return bulkInsertSQL.InsertTable(table, destinationTableName, columnMapps);
            return bulkInsertSQL.InsertTable(table);

        }
    }
}
