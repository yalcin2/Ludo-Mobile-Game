using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DataAccess
{
    public class UsersRepository: ConnectionClass
    {
        public UsersRepository(bool isAdmin):base(isAdmin)
        { }

        public bool Login(string email, string password)
        {
            if(Entity.Users.SingleOrDefault(x=> x.Email == email && x.Password == password) == null)
            { return false; }
            else
            {
                User u = Entity.Users.SingleOrDefault(x => x.Email == email);

                u.NoOfAttempts = 0;
                Entity.SaveChanges();

                return true;
            }
        }
        public IQueryable<User> GetUsers()
        {
            return Entity.Users;
        }

        public string GetUserPublicKey(Guid ID)
        {
            return Entity.Users.SingleOrDefault(x => x.Id == ID).PublicKey;
        }


        public string GetUserPrivateKey(Guid ID)
        {
            return Entity.Users.SingleOrDefault(x => x.Id == ID).PrivateKey;
        }

        public IQueryable<Role> GetRolesForUser(string email)
        {
            return Entity.Users.SingleOrDefault(x => x.Email == email).Roles.AsQueryable();
        }

        public User GetUser(string email)
        {
            return Entity.Users.SingleOrDefault(x => x.Email == email);
        }

        public void SetRecoveryCode(string email)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[15];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            User u = Entity.Users.SingleOrDefault(x => x.Email == email);

            u.RecoveryCode = finalString;
            Entity.SaveChanges();

        }

        public string GetRecoveryCode(string email)
        {
            return Entity.Users.SingleOrDefault(x => x.Email == email).RecoveryCode;
        }

        public Guid GetUserID(string email)
        {
            return Entity.Users.SingleOrDefault(x => x.Email == email).Id;
        }

        public bool CheckAccountBlocked(string email)
        {
            return Entity.Users.SingleOrDefault(x => x.Email == email).Blocked;
        }

        public int ValidationAttempts(string email)
        {
            User u = Entity.Users.SingleOrDefault(x => x.Email == email);

            if (u.NoOfAttempts < 3)
            {
                u.NoOfAttempts = u.NoOfAttempts + 1;
                Entity.SaveChanges();
            }
            else if (u.NoOfAttempts == 3)
            {
                u.Blocked = true;
                Entity.SaveChanges();
            }

            return u.NoOfAttempts;
        }

        public void ResetPassword(string email, string password)
        {
            User u = Entity.Users.SingleOrDefault(x => x.Email == email);
            u.RecoveryCode = ""; 
            u.Password = password;
            Entity.SaveChanges();
        }

        public void AddUser(User u)
        {
            Entity.Users.Add(u);
            Entity.SaveChanges();
        }

    }
}
