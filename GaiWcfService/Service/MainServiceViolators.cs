using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;

namespace GaiWcfService.Service {
    public partial class MainService : IAdminService, IUserService {

        private IViolatorRepository violatorRepository = new ViolatorRepository();

        public void AddViolator(ViolatorDto violator) { 
            violatorRepository.AddViolator(Mapper.mapper.Map<Violator>(violator));
        }

        public void EditViolator(int id, ViolatorDto violator) {
            violatorRepository.EditViolator(id, Mapper.mapper.Map<Violator>(violator));
        }

        public void DeleteViolator(int id) {
            violatorRepository.DeleteViolator(id);
        }

    }
}
