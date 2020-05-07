using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class UserRepository : IUserRepository {

        private GAIDBEntities dbEntities = DbEntitiesSingleton.Instance.GetDbEntities();

        public void AddUser(User user) {
            dbEntities.Users.Add(user);
            dbEntities.SaveChanges();
        }

        public void EditUser(User user) {
            User oldUser = dbEntities.Users.Find(user.login);
            oldUser.login = user.login;
            oldUser.password = user.password;
            oldUser.role = user.role;

            dbEntities.SaveChanges();
        }

        public User GetUser(string login) {
            return dbEntities.Users.Find(login);
        }

        public HashSet<User> GetAll() {
            return dbEntities.Users.ToHashSet();
        }

        public List<User> SearchUser(User searchedUser) {
            return dbEntities.Users.Where(val =>
                (searchedUser.login == default || val.login.Equals(searchedUser.login)) &&
                (searchedUser.role == default || val.role.Equals(searchedUser.role)))
                .ToList();
                
        }

        public void DeleteUser(string login) {
            User user = dbEntities.Users.Find(login);
            dbEntities.Users.Remove(user);
            dbEntities.SaveChanges();
        }
    }
}
