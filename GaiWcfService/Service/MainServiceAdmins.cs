using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GaiWcfService.Service {
    public partial class MainService : IAdminService, IUserService {

        private IAdminRepository adminRepository = new AdminRepository();
        
        public string Test() {
            Admin admin = new Admin() { id = 1, password = "pass", username = "username"};
            AdminDto adminDto = Util.Mapper.mapper.Map<AdminDto>(admin);

            return $"{adminDto.id} -- {adminDto.username} -- {adminDto.password}";
        }

        public void AddAdimn(AdminDto admin) {
            adminRepository.AddAdimn(Mapper.mapper.Map<Admin>(admin));
        }

        public void EditAdmin(int id, AdminDto admin) {
            adminRepository.EditAdmin(id, Mapper.mapper.Map<Admin>(admin));
        }

        public HashSet<AdminDto> getAllAdmins() {
            /*
            HashSet<Admin> set = adminRepository.GetAll();
            HashSet<AdminDto> dtoSet = new HashSet<AdminDto>();
            foreach (var item in set) {
                dtoSet.Add(Mapper.mapper.Map<AdminDto>(item));
            }
            return dtoSet;
            */
            HashSet<AdminDto> set = new HashSet<AdminDto>();
            var entities = new GAIDBEntities();
            set.Add(Mapper.mapper.Map<AdminDto>(entities.Admins.First()));
            return set;
        }

        public string test() {
            return "TEST";
        }
    }
}
