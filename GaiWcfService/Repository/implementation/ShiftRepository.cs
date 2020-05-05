using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    class ShiftRepository : IShiftRepository {
        private GAIDBEntities dbEntities = DbEntitiesSingleton.Instance.GetDbEntities();

        public Shift AddShift(Shift shift) {
            Shift added = dbEntities.Shifts.Add(shift);
            dbEntities.SaveChanges();
            return added;
        }
        
        public void EditShift(int id, Shift shift) {
            Shift oldShift = dbEntities.Shifts.Find(id);
            oldShift.start = shift.start;
            oldShift.end = shift.end;
            oldShift.responsible_id = shift.responsible_id;
            dbEntities.SaveChanges();
        }

        public void DeleteShift(int id) {
            Shift shift = dbEntities.Shifts.Find(id);
            dbEntities.Shifts.Remove(shift);
            dbEntities.SaveChanges();
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
