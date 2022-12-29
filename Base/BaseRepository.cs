
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.Common;
using Core.Infrastructure.Grid;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Base
{
    public abstract class BaseRepository<T> : BaseDapperRepository, IGenericRepository<T> where T : class
    {
        public readonly ILogger _logger;
        //private readonly IDbConnection _conn;
        //private IDbTransaction _tran;
        private readonly DbContext _dbContext;
        private DbSet<T> _dbSet;
        public BaseRepository(ILogger logger, IConnectionFactory conn) : base(logger, conn)
        {
            _logger = logger;
            // _conn = conn.GetConnection;
        }
        public BaseRepository(ILogger logger, IConnectionFactory conn, DbContext dbContext) : base(logger, conn)
        {
            _logger = logger;
            //_conn = conn.GetConnection;
            _dbContext = dbContext;
        }
        public BaseRepository(ILogger logger, DbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public DbSet<T> DB
        {
            get
            {
                if (_dbSet == null)
                    _dbSet = _dbContext.Set<T>();


                return _dbSet;
            }
        }


        public virtual async Task<T> Get(int id)
        {
            return await DB.FindAsync(id);
        }
        public virtual async Task<T> Get(Expression<Func<T, bool>> where)
        {
            return await DB.SingleOrDefaultAsync(where);
        }
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await DB.ToListAsync();

        }
        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> where)
        {
            return await DB.Where(where).ToListAsync();
        }
        public virtual async Task<T> SingleOrDefault(Expression<Func<T, bool>> where)
        {
            if (where == null)
                return await DB.SingleOrDefaultAsync();
            return await DB.SingleOrDefaultAsync(where);

        }
        public virtual async Task<T> FirstOrDefault(Expression<Func<T, bool>> where = null)
        {
            if (where == null)
                return await DB.FirstOrDefaultAsync();
            return await DB.FirstOrDefaultAsync(where);

        }

        public virtual async Task<IEnumerable<T>> DataAync(Expression<Func<T, bool>> where = null)
        {
            if (where == null)
                return await DB.ToListAsync();
            else
                return await DB.Where(where).ToListAsync();

        }

        public virtual IQueryable<T> Data(Expression<Func<T, bool>> where = null)
        {
            if (where == null)
                return DB.AsQueryable();
            else
                return DB.Where(where).AsQueryable();

        }



        public virtual async Task Add(T entity)
        {
            await DB.AddAsync(entity);
            // _dbContext.SaveChanges();

        }
        public virtual async Task AddRange(List<T> entity)
        {
            await DB.AddRangeAsync(entity);
            // _dbContext.SaveChanges();

        }


        public virtual async Task<int> Delete(int id)
        {
            var data = Get(id).Result;
            DB.Remove(data);
            return _dbContext.SaveChanges();

        }

        public virtual async Task Update(T entity)
        {
            _dbContext.Update(entity);
            //this.DB.Attach(entity);
            //entry.State = EntityState.Modified;

        }
        public virtual async Task UpdateRange(List<T> entities)
        {
            _dbContext.UpdateRange(entities);
            //this.DB.Attach(entity);
            //entry.State = EntityState.Modified;

        }
        public virtual async Task<int> UpdateChange(T entity)
        {
            var entry = _dbContext.Update(entity);
            return _dbContext.SaveChanges();


        }


        public virtual async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<GridEntity<T>> GridDataSourceAync(GridOptions options = null, string condition = "")
        {
            return await GetPagingData<T>(options);
        }
        public virtual async Task<GridEntity<T2>> GridDataSourceAync<T2>(GridOptions options)
        {
            return await GetPagingData<T2>(options);
        }
        public virtual async Task<GridEntity<T2>> GridDataSourceAync<T2>(GridOptions options, string condition = "")
        {
            return await GetPagingData<T2>(options, condition);
        }
        public async Task<GridEntity<T2>> GridDataSourceAync<T2>(GridOptions options, string orderbyColumn, string filter)
        {
            return await GetPagingData<T2>(options, orderbyColumn, filter);


        }
        private async Task<GridEntity<T1>> GetPagingData<T1>(GridOptions options = null, string condition = "")
        {
            return await GetPagingData<T1>(options, "", condition);
        }
        private async Task<GridEntity<T1>> GetPagingData<T1>(GridOptions options, string orderByColumn, string condition)
        {
            try
            {
                string query = "";
                string orderByColumnName = "";
                T oT = default;
                oT = Activator.CreateInstance<T>();
                var properties = oT.GetType().GetProperties();
                var attr = oT.GetType().CustomAttributes;
                if (attr != null && attr.Any())
                {
                    var tablename = attr.FirstOrDefault().ConstructorArguments.FirstOrDefault().Value;
                    query = string.Format("Select * from {0}", tablename);


                }
                if (properties != null && properties.Any())
                {
                    if (orderByColumn == "")
                        orderByColumnName = properties.FirstOrDefault().Name;
                }
                if (query != "")
                {
                    return await GridDataSourceAync<T1>(query, orderByColumnName, options, condition);
                }
            }
            catch (Exception)
            {


            }


            return await Task.Run(() => new GridEntity<T1>());
        }

        public async Task<int> Save(T entity)
        {
            await DB.AddAsync(entity);
            return _dbContext.SaveChanges();
        }

        public async Task Remove(int id)
        {
            var data = Get(id).Result;
            DB.Remove(data);
        }
        public async Task RemoveRange(List<T> entities)
        {
            DB.RemoveRange(entities);
        }

        public async Task<bool> IsExist(Expression<Func<T, bool>> where)
        {
            var c = DB.Where(where).Count();
            return c > 0;
        }



        public IDbTransaction BeginTransaction()
        {
            return base.BeginTransaction();
        }

        public void Commit()
        {
            base.Commit();
        }

        public void Rollback()
        {
            base.Rollback();
        }
    }
}
