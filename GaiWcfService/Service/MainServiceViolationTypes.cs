using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System.Collections.Generic;
using System.Linq;

namespace GaiWcfService.Service {
    public partial class MainService {

        private IViolationTypeRepository violationTypeRepository = new ViolationTypeRepository();

        public void EditViolationType(int id, ViolationTypeDto violationType) {
            violationTypeRepository.EditViolationType(id, Mapper.mapper.Map<ViolationType>(violationType));
        }

        public void DeleteViolationType(int id) {
            violationTypeRepository.DeleteViolationType(id);
        }

        public List<ViolationTypeDto> GetAllViolationTypes() {
            return violationTypeRepository.GetAll().Select(val => Mapper.mapper.Map<ViolationTypeDto>(val)).ToList();
        }
    }
}
