using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    class ShiftRepository : IShiftRepository {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public void AddShift(Shift shift) {
            dbEntities.instance.Shifts.Add(shift);
            dbEntities.instance.SaveChanges();
        }
        
        public void EditShift(int id, Shift shift) {
            Shift oldShift = dbEntities.instance.Shifts.Find(id);
            oldShift.start = shift.start;
            oldShift.end = shift.end;
            oldShift.responible_id = shift.responible_id;
            dbEntities.instance.SaveChanges();
        }

        public void DeleteShift(int id) {
            Shift shift = dbEntities.instance.Shifts.Find(id);
            dbEntities.instance.Shifts.Remove(shift);
            dbEntities.instance.SaveChanges();
        }

        public Shift GetRepository(int id) {
            return dbEntities.instance.Shifts.Find(id);
        }

        public HashSet<Shift> GetAll() {
            return dbEntities.instance.Shifts.ToHashSet();
        }
    }
}
