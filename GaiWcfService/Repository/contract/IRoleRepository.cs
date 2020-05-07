using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    public interface IRoleRepository {

        List<Role> GetRoles();
    }
}
