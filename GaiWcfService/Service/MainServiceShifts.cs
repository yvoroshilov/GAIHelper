using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Service {
    public partial class MainService : IAdminService, IUserService {

        private IShiftRepository shiftRepository = new ShiftRepository();

        public int OpenShift(int responsibleId) {
            Shift shift = shiftRepository.GetOpenedShift(responsibleId);
            if (shift == null) {
                shift = new Shift();
                shift.responsible_id = responsibleId;
                shift.start = DateTime.Now;
                shiftRepository.AddShift(shift);
            }
            return shift.id;
        }

        public void CloseShift(int id) {
            
        }
    }
}
