using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string FileUploadPath { get; set; }
        public string DbServer { get; set; }
        public string ConnectionString { get; set; }

        public double AccessTokenExpirationMinutes { get; set; }
        public string RefreshTokenSecret { get; set; }
        public double RefreshTokenExpirationMinutes { get; set; }


    }
    public enum DbServer
    {
        MySQL,
        MSSQL,
        PostGreSQL,
        MongoDB,
        MariaDB,

    }

    public class DbSettings
    {
        public DbServer DbServer { get; set; }
        public string ConnectionString { get; set; }
    }

    public class ConnectionSettings
    {

        public DbSettings AbsDbSettings { get; set; }
        public DbSettings HRDbSettings { get; set; }
        public DbSettings InvDbSettings { get; set; }
        public DbSettings BillgenixDbSettings { get; set; }

        public DbSettings DashboardDbSettings { get; set; }



    }

}
