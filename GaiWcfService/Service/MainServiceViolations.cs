using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System.Collections.Generic;
using System.Linq;

namespace GaiWcfService.Service {
    public partial class MainService : IAdminService, IUserService {

        private IViolationRepository violationRepository = new ViolationRepository();

        public List<ViolationDto> GetAllViolations(int personId) {
            return violationRepository.GetAllViolations(personId)
                .Select(val => Mapper.mapper.Map<ViolationDto>(val))
                .ToList();
        }

        public ViolationDto AddViolation(ViolationDto violation) {
            return Mapper.mapper.Map<ViolationDto>(
                violationRepository.AddViolation(Mapper.mapper.Map<Violation>(violation)));
        }

        public void EditViolation(ViolationDto violation) {
            violationRepository.EditViolation(Mapper.mapper.Map<Violation>(violation));
        }

        public void DeleteViolation(int id) {
            violationRepository.DeleteViolation(id);
        }

        public void AddViolationType(ViolationTypeDto violationType) {
            violationTypeRepository.AddViolationType(Mapper.mapper.Map<ViolationType>(violationType));
        }

    }
}
