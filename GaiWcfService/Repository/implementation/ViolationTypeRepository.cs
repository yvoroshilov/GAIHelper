using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GaiWcfService.Util.DbEntitiesSingleton;

namespace GaiWcfService.Repository.implementation {
    class ViolationTypeRepository : IViolationTypeRepository {
        public void AddViolationType(ViolationType violationType) {
            GAIDBEntities entities = dbEntities;
            entities.ViolationTypes.Add(violationType);
            entities.SaveChanges();
        }

        public void EditViolationType(int id, ViolationType violationType) {
            GAIDBEntities entities = dbEntities;
            ViolationType oldViolationType = entities.ViolationTypes.Find(id);
            oldViolationType.title = violationType.title;
            oldViolationType.min_penalty = violationType.min_penalty;
            oldViolationType.description = violationType.description;
            entities.SaveChanges();
        }

        public void DeleteViolationType(int id) {
            GAIDBEntities entities = dbEntities;
            ViolationType violationType = entities.ViolationTypes.Find(id);
            entities.ViolationTypes.Remove(violationType);
            entities.SaveChanges();
        }

        public ViolationType GetViolationType(int id) {
            return dbEntities.ViolationTypes.Find(id);
        }

        public List<ViolationType> GetAll() {
            return dbEntities.ViolationTypes
                .OrderBy(val => val.id)
                .ToList();
        }
    }
}
