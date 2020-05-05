using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    public interface IViolationRepository {
        Violation AddViolation(Violation violation);

        void EditViolation(Violation violation);

        void DeleteViolation(int id);

        Violation GetViolation(int id);

        List<Violation> SearchViolations(Violation searchedViolation);

        List<Violation> GetAllViolations(int personId);

        List<Violation> GetAllViolationsByShiftId(int shiftId);
    }
}
