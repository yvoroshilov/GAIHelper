using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class AdminRepository : IAdminRepository {

        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public void AddAdimn(Admin admin) {
            dbEntities.instance.Admins.Add(admin);
            dbEntities.instance.SaveChanges();
        }

        public void EditAdmin(int id, Admin admin) {
            Admin oldAdmin = dbEntities.instance.Admins.Find(id);
            oldAdmin.username = admin.username;
            oldAdmin.password = admin.password;

            dbEntities.instance.SaveChanges();
        }

        public Admin GetAdmin(int id) {
            return dbEntities.instance.Admins.Find(id);
        }

        public HashSet<Admin> GetAll() {
            return dbEntities.instance.Admins.ToHashSet();
        }
    }
}
