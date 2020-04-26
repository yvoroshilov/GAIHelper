using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System.Collections.Generic;
using System.Linq;

namespace GaiWcfService.Service {
    public partial class MainService : IAdminService, IUserService {

        private IViolationRepository violationRepository = new ViolationRepository();


        public void AddViolation(ViolationDto violation) {
            violationRepository.AddViolation(Mapper.mapper.Map<Violation>(violation));
        }

        public void EditViolation(int id, ViolationDto violation) {
            violationRepository.EditViolation(id, Mapper.mapper.Map<Violation>(violation));
        }

        public List<ViolationDto> GetAllViolations() {
            return violationRepository.GetAllViolations().Select(val => Mapper.mapper.Map<ViolationDto>(val)).ToList();
        }
        public void DeleteViolation(int id) {
            violationRepository.DeleteViolation(id);
        }

        public void AddViolationType(ViolationTypeDto violationType) {
            violationTypeRepository.AddViolationType(Mapper.mapper.Map<ViolationType>(violationType));
        }

    }
}
