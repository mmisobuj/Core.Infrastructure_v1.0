using Core.Infrastructure.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<T> Get(Expression<Func<T, bool>> where);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> where);


        Task Add(T entity);
        Task AddRange(List<T> entity);

        Task<int> Save(T entity);
        Task<int> Delete(int id);
        Task Remove(int id);
        Task RemoveRange(List<T> entities);
        Task Update(T entity);
        Task UpdateRange(List<T> entities);
        Task<int> UpdateChange(T entity);
        Task<GridEntity<T2>> GridDataSourceAync<T2>(string query, string orderByColumn, GridOptions options = null, string condition = "");
        Task<GridEntity<T>> GridDataSourceAync(GridOptions options = null, string condition = "");
        Task<GridEntity<T2>> GridDataSourceAync<T2>(GridOptions options);
        Task<GridEntity<T2>> GridDataSourceAync<T2>(GridOptions options, string filter);
        Task<GridEntity<T2>> GridDataSourceAync<T2>(GridOptions options, string orderbyColumn, string filter);

        Task<bool> IsExist(Expression<Func<T, bool>> where);



        Task<T> SingleOrDefault(Expression<Func<T, bool>> where);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> DataAync(Expression<Func<T, bool>> where = null);
        IQueryable<T> Data(Expression<Func<T, bool>> where = null);

        IDbTransaction BeginTransaction();
        void Commit();
        void Rollback();


    }
}
