using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GaiWcfService.Util.DbEntitiesSingleton;

namespace GaiWcfService.Repository.implementation {
    public class UserRepository : IUserRepository {
        public void AddUser(User user) {
            GAIDBEntities entities = dbEntities;
            entities.Users.Add(user);
            entities.SaveChanges();
        }

        public void EditUser(User user) {
            GAIDBEntities entities = dbEntities;
            User oldUser = entities.Users.Find(user.login);
            oldUser.login = user.login;
            oldUser.password = user.password;
            oldUser.role = user.role;

            entities.SaveChanges();
        }

        public User GetUser(string login) {
            return dbEntities.Users.Find(login);
        }

        public List<User> SearchUser(User searchedUser) {
            return dbEntities.Users.Where(val =>
                (searchedUser.login == default || val.login.Equals(searchedUser.login)) &&
                (searchedUser.role == default || val.role.Equals(searchedUser.role)))
                .ToList();
                
        }

        public void DeleteUser(string login) {
            GAIDBEntities entities = dbEntities;
            User user = entities.Users.Find(login);
            entities.Users.Remove(user);
            entities.SaveChanges();
        }
    }
}
