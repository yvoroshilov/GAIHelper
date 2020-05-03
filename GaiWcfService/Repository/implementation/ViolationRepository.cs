using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class ViolationRepository : IViolationRepository {
        private GAIDBEntities dbEntities = DbEntitiesSingleton.GetDbEntities().instance;

        public Violation AddViolation(Violation violation) {
            Violation added = dbEntities.Violations.Add(violation);
            dbEntities.SaveChanges();
            return added;
        }

        public void EditViolation(Violation newViolation) {
            Violation editedViolation = dbEntities.Violations.Find(newViolation.id);
            if (newViolation.violation_type_id == editedViolation.violation_type_id) {
                editedViolation.ViolationType = dbEntities.ViolationTypes.Find(editedViolation.violation_type_id);
            }
            if (newViolation.person_id == editedViolation.person_id) {
                editedViolation.Person = dbEntities.Persons.Find(editedViolation.person_id);
            }

            editedViolation.violation_type_id = newViolation.violation_type_id;
            editedViolation.person_id = newViolation.person_id;
            editedViolation.car_number = newViolation.car_number;
            editedViolation.date = newViolation.date;
            editedViolation.penalty = newViolation.penalty;
            editedViolation.location_n = newViolation.location_n;
            editedViolation.location_e = newViolation.location_e;
            editedViolation.address = newViolation.address;
            editedViolation.description = newViolation.description;

            dbEntities.SaveChanges();
        }

        public void DeleteViolation(int id) {
            Violation violation = dbEntities.Violations.Find(id);
            dbEntities.Violations.Remove(violation);
            dbEntities.SaveChanges();
        }

        public Violation GetViolation(int id) {
            return dbEntities.Violations.Find(id);
        }

        public List<Violation> GetAllViolations(int personId) {
            return dbEntities.Violations.Select(val => val)
                .Where(val => val.person_id == personId)
                .ToList();
        }
    }
}
