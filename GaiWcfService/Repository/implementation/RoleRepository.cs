using GaiWcfService.Repository.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GaiWcfService.Util.DbEntitiesSingleton;

namespace GaiWcfService.Repository.implementation {
    public class RoleRepository : IRoleRepository {
        public List<Role> GetRoles() {
            return dbEntities.Roles.ToList();
        }
    }
}
