using GaiWcfService.Repository.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class RoleRepository : IRoleRepository {
        private GAIDBEntities dbEntities = DbEntitiesSingleton.Instance.GetDbEntities();

        public List<Role> GetRoles() {
            return dbEntities.Roles.ToList();
        }
    }
}
