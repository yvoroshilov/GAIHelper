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
            Violation added = violationRepository.AddViolation(Mapper.mapper.Map<Violation>(violation));
            Utility.CalculateViolationsPaid(added.person_id);
            return Mapper.mapper.Map<ViolationDto>(added);
        }

        public ViolationDto AddViolationFile(int violationId, byte[] file, string filename) {
            Violation violation = violationRepository.GetViolation(violationId);
            if (violation.doc_path != null) {
                RemoveViolationFile(violationId);
            }

            FileStream fs = File.Create($@"{Configuration.DocDir}\{violation.id}_{filename}");
            fs.Write(file, 0, file.Length);
            violation.doc_path = new FileInfo(fs.Name).Name;
            violationRepository.EditViolation(violation);
            fs.Close();
            return Mapper.mapper.Map<ViolationDto>(violation);
        }

        public byte[] GetViolationFile(int violationId) {
            Violation violation = violationRepository.GetViolation(violationId);
            return File.ReadAllBytes(Configuration.DocDir + @"\" + violation.doc_path);
        }

        public void RemoveViolationFile(int violationId) {
            Violation violation = violationRepository.GetViolation(violationId);
            File.Delete(Configuration.DocDir + @"\" + violation.doc_path);
            violation.doc_path = null;
        }

        public void EditViolation(ViolationDto violation) {
            Violation edited = violationRepository.GetViolation(violation.id);
            violationRepository.EditViolation(Mapper.mapper.Map<Violation>(violation));
            if (edited.person_id != violation.personId) {
                MyLogger.Instance.Write("NOT EQUALS");
                Utility.CalculateViolationsPaid(violation.personId);
            }
            Utility.CalculateViolationsPaid(edited.person_id);
        }

        public void DeleteViolation(int id) {
            Violation violation = violationRepository.GetViolation(id);
            violationRepository.DeleteViolation(id);
            Utility.CalculateViolationsPaid(violation.person_id);
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
