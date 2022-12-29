using Core.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Common
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
        IDbTransaction GetTransaction { get; }
        IDbTransaction CreateTransaction();
        Task<bool> Backup(string backup_path);
        Task<bool> Restore(string restore_path);
        public DbServer DBServer { get; }


        //string GetConnectionString();
    }
}
