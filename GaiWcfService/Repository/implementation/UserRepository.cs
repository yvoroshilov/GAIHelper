using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class UserRepository : IUserRepository {

        private GAIDBEntities dbEntities = DbEntitiesSingleton.GetDbEntities().instance;

        public void AddUser(User user) {
            dbEntities.Users.Add(user);
            dbEntities.SaveChanges();
        }

        public void EditUser(int id, User user) {
            User oldUser = dbEntities.Users.Find(id);
            oldUser.Role1 = dbEntities.Roles.Find(user.role);
            oldUser.login = user.login;
            oldUser.password = user.password;
            oldUser.role = user.role;

            dbEntities.SaveChanges();
        }

        public User GetUser(int id) {
            return dbEntities.Users.Find(id);
        }

        public User GetUser(string login) {
            return dbEntities.Users
                .Where(val => val.login == login)
                .SingleOrDefault();
        }

        public HashSet<User> GetAll() {
            return dbEntities.Users.ToHashSet();
        }
    }
}
