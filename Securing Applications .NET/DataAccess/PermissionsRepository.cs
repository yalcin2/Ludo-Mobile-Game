using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DataAccess
{
    public class PermissionsRepository : ConnectionClass
    {
        public PermissionsRepository(bool isAdmin) : base(isAdmin)
        { }

        public bool CheckPermissionExists(Guid userId, int musicId)
        {
            return false;

        }

        public bool CheckPermission(int userId)
        {
            return Entity.Permissions.SingleOrDefault(x => x.Id == userId).Permission1;
        }

        public void AddPermission(Permission p)
        {
            Entity.Permissions.Add(p);
            Entity.SaveChanges();
        }

    }
}
