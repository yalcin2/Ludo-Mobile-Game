using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Common;
using DataAccess;

namespace BusinessLogic
{
    public class PermissionsBL
    {
        public bool CheckPermissionExists(Guid userId, int musicId)
        {
            return new PermissionsRepository(false).CheckPermissionExists(userId, musicId);
        }

        public bool CheckPermission(int id)
        {
            return new PermissionsRepository(false).CheckPermission(id);
        }

        public void AddPermission(bool permission, int music_fk, Guid user_fk)
        {
            PermissionsRepository pr = new PermissionsRepository(false);

            if (pr.CheckPermissionExists(user_fk, music_fk) == false)
            {
                Permission p = new Permission();
                p.Permission1 = permission;
                p.Music_fk = music_fk;
                p.User_fk = user_fk;

                pr.AddPermission(p);
            }
            else
            {
                throw new Exception("Permission already exists");
            }
           
        }
        
    }
}
