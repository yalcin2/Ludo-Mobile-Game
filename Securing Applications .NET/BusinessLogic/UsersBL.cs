using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Common;
using DataAccess;

namespace BusinessLogic
{
    public class UsersBL
    {
        public bool Login(string email, string password)
        {
            return new UsersRepository(false).Login(email, Encryption.HashPassword(password));
        }

        public IQueryable<User> GetUsers()
        {
            return new UsersRepository(false).GetUsers();
        }


        public string GetUserPublicKey(Guid ID)
        {
            return new UsersRepository(false).GetUserPublicKey(ID);
        }

        public string GetUserPrivateKey(Guid ID)
        {
            return new UsersRepository(false).GetUserPrivateKey(ID);
        }

        public Guid GetUserID(string email)
        {
            return new UsersRepository(false).GetUserID(email);
        }

        public void SetRecoveryCode(string email)
        {
            new UsersRepository(false).SetRecoveryCode(email);
        }

        public string GetRecoveryCode(string email)
        {
            return new UsersRepository(false).GetRecoveryCode(email);
        }

        public IQueryable<Role> GetRolesForUser(string email)
        {
            return new UsersRepository(false).GetRolesForUser(email);
        }

        public User GetUser(string email)
        {
            return new UsersRepository(false).GetUser(email);
        }

        public bool CheckAccountBlocked(string email)
        {
            return new UsersRepository(false).CheckAccountBlocked(email);
        }

        public int ValidationAttempts(string email)
        {
            return new UsersRepository(false).ValidationAttempts(email);
        }

        public void ResetPassword(string email, string password)
        {
            new UsersRepository(false).ResetPassword(email, Encryption.HashPassword(password));
        }


        public void Register(string email, string password, string firstName, string lastName, string mobileNumber)
        {

            UsersRepository ur = new UsersRepository(false);
            RolesRepository rr = new RolesRepository();
            ur.Entity = rr.Entity;



            using (TransactionScope ts = new TransactionScope())
            {

                if (ur.GetUser(email) == null)
                {
                    //check /validate the password
                    //if password does not follow the policy throw new Exception("password is not valid")

                    Role r = rr.GetRole(1);

                    User u = new User();
                    u.Id = Guid.NewGuid();
                    u.Email = email;
                    u.Password = Encryption.HashPassword(password);
                    u.FirstName = firstName;
                    u.LastName = lastName;
                    u.Mobile = mobileNumber;

                    var myKeys = Encryption.GenerateAsymmetricKeys();
                    u.PublicKey = myKeys.PublicKey;
                    u.PrivateKey = myKeys.Privatekey;


                    ur.AddUser(u);

                    rr.AllocateRoleToUser(r, u);
                }
                else
                {
                    throw new Exception("Email is already taken.");
                }

                ts.Complete();//the line of code which actually saves everything permanently in db

            }
        }
    }
}
