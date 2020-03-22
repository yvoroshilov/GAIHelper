using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class ViolatorRepository : IViolatorRepository {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public void AddViolator(Violator violator) {
            dbEntities.instance.Violators.Add(violator);
            dbEntities.instance.SaveChanges();
        }

        public void EditViolator(int id, Violator violator) {
            Violator oldViolatior = dbEntities.instance.Violators.Find(id);
            oldViolatior.name = violator.name;
            oldViolatior.actual_penalty = violator.actual_penalty;
            oldViolatior.paid_penalty = violator.paid_penalty;
            oldViolatior.passport_id = violator.passport_id;
            oldViolatior.patronymic = violator.patronymic;
            oldViolatior.surname = violator.surname;
            dbEntities.instance.SaveChanges();
        }

        public void DeleteViolator(int id) {
            Violator violator = dbEntities.instance.Violators.Find(id);
            dbEntities.instance.Violators.Remove(violator);
            dbEntities.instance.SaveChanges();
        }

        public Violator GetViolator(int id) {
            return dbEntities.instance.Violators.Find(id);
        }

        public HashSet<Violator> GetAll() {
            return dbEntities.instance.Violators.ToHashSet();
        }
    }
}
