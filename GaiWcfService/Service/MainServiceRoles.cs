using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Service {
    public partial class MainService {
        private IRoleRepository roleRepository = new RoleRepository();

        public List<RoleDto> GetAllRoles() {
            return roleRepository.GetRoles()
                .Select(val => Mapper.mapper.Map<RoleDto>(val))
                .ToList();
        }
    }
}
