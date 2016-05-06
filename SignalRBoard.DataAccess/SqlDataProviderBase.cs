using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SignalRBoard.DataAccess
{
    public abstract class SqlDataProviderBase : IDisposable
    {
        const string DefaultConnectionStringName = "DatabaseConnectionString";

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[DefaultConnectionStringName].ConnectionString);
        }

        public virtual void Dispose()
        {

        }
    }
}
