using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System.Collections.Generic;
using System.Linq;

namespace GaiWcfService.Service {
    public partial class MainService {

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

        public List<ViolationDto> SearchViolations(ViolationDto violation) {
            return violationRepository.SearchViolations(Mapper.mapper.Map<Violation>(violation))
                .Select(val => Mapper.mapper.Map<ViolationDto>(val))
                .ToList();
        }

        public void AddViolationType(ViolationTypeDto violationType) {
            violationTypeRepository.AddViolationType(Mapper.mapper.Map<ViolationType>(violationType));
        }

        public List<ViolationDto> GetAllViolationsByResponsibleId(int responsibleId) {
            Employee employee = employeeRepository.GetEmployee(responsibleId);
            List<ViolationDto> res = new List<ViolationDto>();
            foreach (var item in employee.Shifts) {
                res.AddRange(violationRepository.GetAllViolationsByShiftId(item.id)
                    .Select(val => Mapper.mapper.Map<ViolationDto>(val))
                    .ToList());
            }
            return res;
        }
    }
}
