using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;

namespace GaiWcfService.Service {
    public partial class MainService : IAdminService, IUserService {

        private IViolationTypeRepository violationTypeRepository = new ViolationTypeRepository();

        public void EditViolationType(int id, ViolationTypeDto violationType) {
            violationTypeRepository.EditViolationType(id, Mapper.mapper.Map<ViolationType>(violationType));
        }

        public void DeleteViolationType(int id) {
            violationTypeRepository.DeleteViolationType(id);
        }

    }
}
