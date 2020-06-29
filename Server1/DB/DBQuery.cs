using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class DBQuery
    {
        public static bool CheckUser_Password(string username, string password)
        {
            using (var db = new UserContext())
            {
                var myUser = db.Users
                    .FirstOrDefault(u => u.Username == username
                                 && u.Password == password);

                if (myUser != null)
                    return true;
            }
            return false;

        }

        public static bool CheckUser(string username)
        {
            using (var ctx = new UserContext())
            {
                var user = ctx.Users
                              .Where(s => s.Username == username)
                              .FirstOrDefault<User>();


                if (user != null)
                    return true;
            }
            return false;
        }

        public static bool CheckEmail(string email)
        {
            using (var usr = new UserContext())
            {
                var user = usr.Users
                                  .Where(s => s.Email == email)
                                  .FirstOrDefault<User>();


                if (user != null)
                    return true;
            }

            return false;
        }
        public static bool CreateUser(User user)
        {
            using (var context = new UserContext())
            {
                context.Users.Add(user);
                return context.SaveChanges() > 0;
            }
        }

        public static User GetUser(string username)
        {
            using (var ctx = new UserContext())
            {
                return ctx.Users
                                .Where(s => s.Username == username)
                                .FirstOrDefault<User>();
            }
        }

        public static void UpdateUserStatus(User user)
        {
            if (user == null)
                return;
            using (var db = new UserContext())
            {
                db.Entry(user).Property(x => x.isInGame).IsModified = true;
                db.SaveChanges();
            }
        }
    }
}
