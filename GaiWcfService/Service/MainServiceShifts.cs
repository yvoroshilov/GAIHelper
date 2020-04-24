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

        public void EditShift(int id, ShiftDto shift) {
            shiftRepository.EditShift(id, Mapper.mapper.Map<Shift>(shift));
        }

        public void DeleteShift(int id) {
            shiftRepository.DeleteShift(id);
        }
    }
}
