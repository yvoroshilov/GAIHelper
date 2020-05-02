using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {

    public interface IUserRepository {
        void AddUser(User user);

        void EditUser(int id, User user);

        User GetUser(int id);

        User GetUser(string login);

        HashSet<User> GetAll();
    }
}
