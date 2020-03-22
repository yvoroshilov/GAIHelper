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
            dbEntities.instance.Entry(oldViolation).CurrentValues.SetValues(violation);
        }

        public void DeleteViolation(int id) {
            Violation violation = dbEntities.instance.Violations.Find(id);
            dbEntities.instance.Violations.Remove(violation);
            dbEntities.instance.SaveChanges();
        }

    }
}
