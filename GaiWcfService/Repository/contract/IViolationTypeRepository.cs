using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    public interface IViolationTypeRepository {
        void AddViolationType(ViolationType violationType);

        void EditViolationType(int id, ViolationType violationType);

        void DeleteViolationType(int id);

        ViolationType GetViolationType(int id);

        HashSet<ViolationType> GetAll();
    }
}
