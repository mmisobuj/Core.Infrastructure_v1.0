
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.Common;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Grid;
using Core.Infrastructure.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Base
{
    public class BaseDapperRepository : IBaseDapperRepository
    {
        public readonly ILogger _logger;
        private readonly IDbConnection _conn;
        private IDbTransaction _tran;
        int commandTimeOut = 1000 * 60 * 5;
        private IConnectionFactory _conFactory;
        public BaseDapperRepository()
        {

        }
        public BaseDapperRepository(IConnectionFactory conn)
        {

            _conn = conn.GetConnection;
            _conFactory = conn;
        }
        public BaseDapperRepository(ILogger logger, IConnectionFactory conn)
        {
            _logger = logger;
            _conn = conn.GetConnection;
            _conFactory = conn;

        }

        public IDbTransaction BeginTransaction()
        {
            try
            {
                _tran = _conFactory.CreateTransaction();

                //if (_conn.State == ConnectionState.Closed)
                //    _conn.Open();

                return _tran;


            }
            catch (Exception ex)
            {

                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);
                throw ex;
            }
            // _dbContext.Database.BeginTransaction();
        }
        public void SetTran(IDbTransaction tran)
        {
            _tran = tran;
        }

        public void Commit()
        {
            try
            {

                //  _conn.BeginTransaction()
                _tran = _tran = _conFactory.GetTransaction;

                if (_tran != null)
                {
                    _tran.Commit();
                    //_tran.Dispose();
                    if (_conn.State == ConnectionState.Open)
                        _conn.Close();
                }
            }
            catch (Exception ex)
            {

                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);
                throw ex;
            }
            //  _conn.Close();
        }
        public void Rollback()
        {
            try
            {
                _tran = _tran = _conFactory.GetTransaction;

                if (_tran != null)
                {
                    _tran.Rollback();
                    //_tran.Dispose();
                    if (_conn.State == ConnectionState.Open)
                        _conn.Close();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);
                throw ex;
            }
            //_conn.Close();
        }
        public async Task<IEnumerable<T>> Query<T>(string query, object parameters = null)
        {
            try
            {
                return await _conn.QueryAsync<T>(query, parameters, commandType: CommandType.Text, commandTimeout: commandTimeOut);
            }
            catch (Exception ex)
            {
                if (_logger == null)
                    throw ex;
                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);

            }
            return null;
        }
        public async Task<T> QuerySingle<T>(string query, object parameters = null)
        {
            try
            {
                return await _conn.QuerySingleAsync<T>(query, parameters, commandType: CommandType.Text, commandTimeout: commandTimeOut);
            }
            catch (Exception ex)
            {
                if (_logger == null)
                    throw ex;
                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);


            }
            return default;
        }
        public async Task<T> QuerySingleOrDefaultAsync<T>(string query, object parameters = null)
        {
            try
            {
                return await _conn.QuerySingleOrDefaultAsync<T>(query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                if (_logger == null)
                    throw ex;
                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);


            }
            return default;
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters = null)
        {
            try
            {
                return await _conn.QueryFirstOrDefaultAsync<T>(query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                if (_logger == null)
                    throw ex;
                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);

            }
            return default;
        }
        public async Task<IEnumerable<T>> SpQuery<T>(string query, object parameters = null)
        {
            try
            {
                return await _conn.QueryAsync<T>(query, parameters, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeOut);
            }
            catch (Exception ex)
            {
                if (_logger == null)
                    throw ex;
                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);

            }
            return null;
        }

        public async Task<T> SpQuerySingle<T>(string query, object parameters = null)
        {
            try
            {
                return await _conn.QuerySingleAsync<T>(query, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                if (_logger == null)
                    throw ex;
                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);

            }
            return default;

        }

        public async Task<T> SpQueryFirst<T>(string query, object parameters = null)
        {
            try
            {
                return await _conn.QueryFirstAsync<T>(query, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                if (_logger == null)
                    throw ex;
                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);

            }
            return default;

        }
        public async Task<int> ExecuteSpAsync(string query, object parameters = null)
        {
            //  ResponseResult result = new();
            try
            {
                _tran = _conFactory.GetTransaction;
                if (_tran != null)
                    return await _conn.ExecuteAsync(query, parameters, _tran, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeOut);
                else
                    return await _conn.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeOut);
                //result.EffectedRow = count;
                //result.IsSuccessStatus = true;
                //result.StatusCode = 200;
            }
            catch (Exception ex)
            {


                _logger?.LogError(ex.Message + " InnerException: ", ex.InnerException);
                //result.IsSuccessStatus = false;
                //result.Message = ex.Message;
                //result.StatusCode = 500;
                throw ex;
            }
            finally
            {
                //if (_tran == null)
                //{
                //    _conn.Close();
                //}
            }
            return -1;
        }
        public async Task<int> ExecuteAsync(string query, object parameters = null)
        {
            ResponseResult result = new();

            try
            {
                _tran = _conFactory.GetTransaction;
                if (_tran != null)
                {
                    return await _conn.ExecuteAsync(query, parameters, _tran, commandTimeout: commandTimeOut);

                }
                else
                {
                    return await _conn.ExecuteAsync(query, parameters, commandTimeout: commandTimeOut);

                }

                //result.EffectedRow = count;
                //result.IsSuccessStatus = true;
                //result.StatusCode = 200;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message + " InnerException: ", ex.InnerException);
                //result.IsSuccessStatus = false;
                //result.Message = ex.Message;
                //result.StatusCode = 500;
                throw ex;
            }
            finally
            {
                if (_tran == null)
                {
                    _conn.Close();
                }
            }
            return -1;
        }

        public async Task<int> ExecuteScalerAsync(string query, object parameters = null)
        {
            int result = 0;

            try
            {
                _tran = _conFactory.GetTransaction;
                if (_tran != null)
                {
                    var obj = await _conn.ExecuteScalarAsync(query, parameters, _tran, commandTimeout: commandTimeOut);
                    if (obj != null)
                    {
                        int.TryParse(obj.ToString(), out result);
                    }
                }
                else
                {
                    var obj = await _conn.ExecuteScalarAsync(query, parameters, commandTimeout: commandTimeOut);
                    if (obj != null)
                    {
                        int.TryParse(obj.ToString(), out result);
                    }
                }



                //result.EffectedRow = count;
                //result.IsSuccessStatus = true;
                //result.StatusCode = 200;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " InnerException: ", ex.InnerException);
                //result.IsSuccessStatus = false;
                //result.Message = ex.Message;
                //result.StatusCode = 500;
                throw ex;

            }
            finally
            {
                if (_tran == null)
                {
                    _conn.Close();
                }
            }
            return result;
        }
        public async Task<int> ExecuteIdentityAsync(string query, object parameters = null)
        {
            int result = 0;

            try
            {
                query = query.Trim();


                if (_conFactory.DBServer == DbServer.MSSQL)
                {
                    query += " select @@identity outId ";

                }
                else if (_conFactory.DBServer == DbServer.MariaDB || _conFactory.DBServer == DbServer.MySQL)
                {
                    if (!query.EndsWith(";"))
                    {
                        query += ";SELECT LAST_INSERT_ID() ";
                    }
                    else
                    {
                        query += " SELECT LAST_INSERT_ID() ";
                    }
                }

                _tran = _conFactory.GetTransaction;
                if (_tran != null)
                {


                    var obj = await _conn.ExecuteScalarAsync(query, parameters, _tran);
                    if (obj != null)
                    {
                        int.TryParse(obj.ToString(), out result);
                    }
                }
                else
                {
                    var obj = await _conn.ExecuteScalarAsync(query, parameters);
                    if (obj != null)
                    {
                        int.TryParse(obj.ToString(), out result);
                    }
                }


                //result.EffectedRow = count;
                //result.IsSuccessStatus = true;
                //result.StatusCode = 200;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " InnerException: ", ex.InnerException);
                //result.IsSuccessStatus = false;
                //result.Message = ex.Message;
                //result.StatusCode = 500;
                throw ex;

            }
            finally
            {
                //if (_tran == null)
                //{
                //    _conn.Close();
                //}
            }
            return result;
        }


        public async Task<GridEntity<T2>> GridDataSourceAync<T2>(string query, string orderByColumn, GridOptions options = null, string condition = "")
        {

            return await _conn.PagingData<T2>(query, orderByColumn, options, condition);
        }

        public async Task<bool> Backup(string backup_path)
        {
            return await _conFactory.Backup(backup_path);
        }
        public async Task<bool> Restore(string restore_path)
        {
            return await _conFactory.Restore(restore_path);

        }


    }
}
