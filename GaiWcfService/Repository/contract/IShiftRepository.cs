using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    public interface IShiftRepository {
        Shift AddShift(Shift shift);

        void EditShift(int id, Shift shift);

        void DeleteShift(int id);

        Shift GetShift(int shiftId);

        Shift GetOpenedShiftByResponsibleId(int responsibleId);

        HashSet<Shift> GetAll();
    }
}
