using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    class ViolationTypeRepository : IViolationTypeRepository {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public void AddViolationType(ViolationType violationType) {
            dbEntities.instance.ViolationTypes.Add(violationType);
            dbEntities.instance.SaveChanges();
        }

        public void EditViolationType(int id, ViolationType violationType) {
            ViolationType oldViolationType = dbEntities.instance.ViolationTypes.Find(id);
            oldViolationType.title = violationType.title;
            oldViolationType.min_penalty = violationType.min_penalty;
            oldViolationType.description = violationType.description;
            dbEntities.instance.SaveChanges();
        }

        public void DeleteViolationType(int id) {
            ViolationType violationType = dbEntities.instance.ViolationTypes.Find(id);
            dbEntities.instance.ViolationTypes.Remove(violationType);
            dbEntities.instance.SaveChanges();
        }

        public ViolationType GetViolationType(int id) {
            return dbEntities.instance.ViolationTypes.Find(id);
        }

        public HashSet<ViolationType> GetAll() {
            return dbEntities.instance.ViolationTypes.ToHashSet();
        }
    }
}
