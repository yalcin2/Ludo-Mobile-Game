using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DataAccess
{
    public class RolesRepository : ConnectionClass
    {
        public RolesRepository() : base()
        { }

        public Role GetRole(int id)
        {
            return Entity.Roles.SingleOrDefault(x => x.Id == id);
        }

        public void AllocateRoleToUser(Role r, User u)
        {
            r.Users.Add(u);
            Entity.SaveChanges();
        }

    }
}
