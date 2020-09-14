using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;


namespace DataAccess
{
    public class ConnectionClass
    {
        public SecurityDatabaseEntities Entity { get; set; }

        public ConnectionClass()
        {
            Entity = new SecurityDatabaseEntities();
        }

        public ConnectionClass(bool isAdmin) {
            if (isAdmin) {
                Entity = new SecurityDatabaseEntities();

            }
            else
            {
                Entity = new SecurityDatabaseEntities();
                string userConnectionString = ConfigurationManager.ConnectionStrings["SecurityDatabaseEntities_user"].ConnectionString;
                Entity.Database.Connection.ConnectionString = userConnectionString;
            }
        }
    }
}
