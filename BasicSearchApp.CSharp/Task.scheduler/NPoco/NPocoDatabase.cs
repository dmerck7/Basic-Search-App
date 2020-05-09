using NPoco;
using System.Linq;
using System.Threading;
using System.Configuration;
using System.Security.Claims;

namespace Scheduler.NPoco {
    class NPocoDatabase : Database {
        public NPocoDatabase(string connectionStringName) : base(AppendAppNameToConnectionString(connectionStringName), DatabaseType.SqlServer2005) {
            // although a connection string name was passed in, the actual connection string is retrieved and the user name appended
            // before supplying it to the base constructor.
        }

        private static string AppendAppNameToConnectionString(string connectionStringName) {
            // get the user name
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string userName = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

            // get the actual connection string from web.config
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            // append the user name as Application Name to the current connection string
            return connectionString + ";Application Name=" + userName;
        }
    }
}