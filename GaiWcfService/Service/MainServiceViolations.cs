using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.IO;
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
            Person person = personRepository.GetPerson(violation.personId);
            person.actual_penalty += (decimal)violation.penalty;
            personRepository.EditPerson(person);
            return Mapper.mapper.Map<ViolationDto>(
                violationRepository.AddViolation(Mapper.mapper.Map<Violation>(violation)));
        }

        public ViolationDto AddViolationFile(int violationId, byte[] file, string filename) {
            Violation violation = violationRepository.GetViolation(violationId);
            if (violation.doc_path != null) {
                RemoveViolationFile(violationId);
            }

            FileStream fs = File.Create($@"{Configuration.FileDir}\{violation.id}_{filename}");
            fs.Write(file, 0, file.Length);
            violation.doc_path = fs.Name;
            fs.Close();
            return Mapper.mapper.Map<ViolationDto>(violation);
        }

        public byte[] GetViolationFile(int violationId) {
            Violation violation = violationRepository.GetViolation(violationId);
            return File.ReadAllBytes(violation.doc_path);
        }

        public void RemoveViolationFile(int violationId) {
            Violation violation = violationRepository.GetViolation(violationId);
            File.Delete(violation.doc_path);
            violation.doc_path = null;
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

        public List<ViolationDto> SearchViolationsPenaltyRange(ViolationDto searchedViolation, double penaltyMin, double penaltyMax) {
            List<ViolationDto> partialResult = SearchViolations(searchedViolation);
            return partialResult.Where(val => val.penalty >= penaltyMin && val.penalty <= penaltyMax)
                .ToList();
        }

        public List<ViolationDto> SearchViolationsDateRange(ViolationDto searchedViolation, DateTime start, DateTime end) {
            List<ViolationDto> partialResult = SearchViolations(searchedViolation);
            return partialResult
                .Where(val => 
                    ((val.date.Date.CompareTo(start.Date) + val.date.Date.CompareTo(end.Date)) == 0) ||
                    (val.date.Date.CompareTo(start.Date) == 0) ||
                    (val.date.Date.CompareTo(end.Date) == 0))
                .ToList();
        }
    }
}
