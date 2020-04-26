using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class ViolationRepository : IViolationRepository {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public void AddViolation(Violation violation) {
            dbEntities.instance.Violations.Add(violation);
            dbEntities.instance.SaveChanges();
        }

        public void EditViolation(int id, Violation violation) {
            Violation oldViolation = dbEntities.instance.Violations.Find(id);
            oldViolation.date = violation.date;
            oldViolation.car_number = violation.car_number;
            oldViolation.description = violation.description;
            oldViolation.location_n = violation.location_n;
            oldViolation.location_e = violation.location_e;
            oldViolation.penalty = violation.penalty;
            oldViolation.violator_id = violation.violator_id;
            dbEntities.instance.SaveChanges();
        }

        public void DeleteViolation(int id) {
            Violation violation = dbEntities.instance.Violations.Find(id);
            dbEntities.instance.Violations.Remove(violation);
            dbEntities.instance.SaveChanges();
        }

        public Violation GetViolation(int id) {
            return dbEntities.instance.Violations.Find(id);
        }

        public List<Violation> GetAllViolations() {
            return dbEntities.instance.Violations.ToList();
        }

        public HashSet<Violation> GetAll() {
            return dbEntities.instance.Violations.ToHashSet();
        }
    }
}
