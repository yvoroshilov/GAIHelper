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
    class ShiftRepository : IShiftRepository {
        public Shift AddShift(Shift shift) {
            GAIDBEntities entities = dbEntities;
            Shift added = entities.Shifts.Add(shift);
            entities.SaveChanges();
            return added;
        }
        
        public void EditShift(int id, Shift shift) {
            GAIDBEntities entities = dbEntities;
            Shift oldShift = entities.Shifts.Find(id);
            oldShift.start = shift.start;
            oldShift.end = shift.end;
            oldShift.responsible_id = shift.responsible_id;
            entities.SaveChanges();
        }

        public void DeleteShift(int id) {
            GAIDBEntities entities = dbEntities;
            Shift shift = entities.Shifts.Find(id);
            entities.Shifts.Remove(shift);
            entities.SaveChanges();
        }

        public Shift GetShift(int id) {
            return dbEntities.Shifts.Find(id);
        }

        public Shift GetOpenedShiftByResponsibleId(int responsibleId) {
            return dbEntities.Shifts
                .Where(val => val.responsible_id == responsibleId && val.end == null)
                .SingleOrDefault();
        }

        public HashSet<Shift> GetAll() {
            return dbEntities.Shifts.ToHashSet();
        }

        public List<Shift> GetAllShiftsByResponsibleId(int responsibleId) {
            return dbEntities.Shifts
                .Where(val => val.responsible_id == responsibleId)
                .ToList();
        }
    }
}
