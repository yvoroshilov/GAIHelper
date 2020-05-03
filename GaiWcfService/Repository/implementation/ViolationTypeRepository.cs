using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    class ViolationTypeRepository : IViolationTypeRepository {
        private GAIDBEntities dbEntities = DbEntitiesSingleton.Instance.GetDbEntities();

        public void AddViolationType(ViolationType violationType) {
            dbEntities.ViolationTypes.Add(violationType);
            dbEntities.SaveChanges();
        }

        public void EditViolationType(int id, ViolationType violationType) {
            ViolationType oldViolationType = dbEntities.ViolationTypes.Find(id);
            oldViolationType.title = violationType.title;
            oldViolationType.min_penalty = violationType.min_penalty;
            oldViolationType.description = violationType.description;
            dbEntities.SaveChanges();
        }

        public void DeleteViolationType(int id) {
            ViolationType violationType = dbEntities.ViolationTypes.Find(id);
            dbEntities.ViolationTypes.Remove(violationType);
            dbEntities.SaveChanges();
        }

        public ViolationType GetViolationType(int id) {
            return dbEntities.ViolationTypes.Find(id);
        }

        public HashSet<ViolationType> GetAll() {
            return dbEntities.ViolationTypes.ToHashSet();
        }
    }
}
