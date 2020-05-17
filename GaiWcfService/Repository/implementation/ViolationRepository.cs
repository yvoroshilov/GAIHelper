using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GaiWcfService.Util.DbEntitiesSingleton;

namespace GaiWcfService.Repository.implementation {
    public class ViolationRepository : IViolationRepository {
        public Violation AddViolation(Violation violation) {
            GAIDBEntities entities = dbEntities;
            Violation added = entities.Violations.Add(violation);
            entities.SaveChanges();
            return added;
        }

        public void EditViolation(Violation newViolation) {
            GAIDBEntities entities = dbEntities;
            Violation editedViolation = entities.Violations.Find(newViolation.id);

            editedViolation.violation_type_id = newViolation.violation_type_id;
            editedViolation.person_id = newViolation.person_id;
            editedViolation.car_number = newViolation.car_number;
            editedViolation.date = newViolation.date;
            editedViolation.penalty = newViolation.penalty;
            editedViolation.latitude = newViolation.latitude;
            editedViolation.longitude = newViolation.longitude;
            editedViolation.address = newViolation.address;
            editedViolation.description = newViolation.description;
            editedViolation.paid = newViolation.paid;
            editedViolation.doc_path = newViolation.doc_path;

            entities.SaveChanges();
        }

        public void DeleteViolation(int id) {
            GAIDBEntities entities = dbEntities;
            Violation violation = entities.Violations.Find(id);
            entities.Violations.Remove(violation);
            entities.SaveChanges();
        }

        public Violation GetViolation(int id) {
            return dbEntities.Violations.Find(id);
        }

        public List<Violation> GetAllViolations(int personId) {
            return dbEntities.Violations.Select(val => val)
                .Where(val => val.person_id == personId)
                .OrderBy(val => val.date)
                .ToList();
        }

        public List<Violation> SearchViolations(Violation searchedViolation) {
            return dbEntities.Violations.Where(val =>
                (searchedViolation.violation_type_id == default || val.violation_type_id.Equals(searchedViolation.violation_type_id)) &&
                (searchedViolation.shift_id == default || val.shift_id == searchedViolation.shift_id) &&
                (searchedViolation.car_number == default || val.car_number.ToLower().Contains(searchedViolation.car_number.ToLower())) &&
                (searchedViolation.protocol_id == default || val.protocol_id == searchedViolation.protocol_id) &&
                (searchedViolation.date == default || val.date == searchedViolation.date) &&
                (searchedViolation.penalty == default || val.penalty == searchedViolation.penalty) &&
                (searchedViolation.longitude == default || val.longitude == searchedViolation.longitude) &&
                (searchedViolation.latitude == default || val.latitude == searchedViolation.latitude) &&
                (searchedViolation.address == default || val.address.ToLower().Contains(searchedViolation.address.ToLower())) &&
                (searchedViolation.description == default || val.description.ToLower().Contains(searchedViolation.description.ToLower())))
                .OrderBy(val => val.date)
                .ToList();
        }

        public List<Violation> GetAllViolationsByShiftId(int shiftId) {
            return dbEntities.Violations
                .Where(val => val.shift_id == shiftId)
                .OrderBy(val => val.date)
                .ToList();
        }
    }
}
