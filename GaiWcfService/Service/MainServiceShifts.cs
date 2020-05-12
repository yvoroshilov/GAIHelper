using GaiWcfService.Callback;
using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GaiWcfService.Service {
    public partial class MainService {

        private IShiftRepository shiftRepository = new ShiftRepository();

        public void AddShift(ShiftDto shift) {
            shiftRepository.AddShift(Mapper.mapper.Map<Shift>(shift));
        }


        public int OpenShift(int responsibleId) {

            Shift shift = shiftRepository.GetOpenedShiftByResponsibleId(responsibleId);
            if (shift == null) {
                shift = new Shift();
                shift.responsible_id = responsibleId;
                shift.start = DateTime.Now;
                shiftRepository.AddShift(shift);
            }
            return shift.id;
        }

        public void CloseShift(int responsibleId) {
            Shift shift = shiftRepository.GetOpenedShiftByResponsibleId(responsibleId);
            shift.end = DateTime.Now;
            shiftRepository.EditShift(shift.id, shift);
            //ConnectedClientsSingleton.Instance.CloseConnection(employeeRepository.GetEmployee(shift.responsible_id).user_login);
            ConnectedClientsSingleton.Instance.CloseConnection(shift.Employee.user_login);
        }

        public ShiftDto GetCurrentShift(int resposibleId) {
            Shift shift = shiftRepository.GetOpenedShiftByResponsibleId(resposibleId);
            return Mapper.mapper.Map<ShiftDto>(shift);
        }

        public List<ShiftDto> GetAllShiftsByResponsibleId(int responsibleId) {
            return shiftRepository.GetAllShiftsByResponsibleId(responsibleId)
                .Select(val => Mapper.mapper.Map<ShiftDto>(val))
                .ToList();
        }

        public ShiftDto GetShiftById(int shiftId) {
            return Mapper.mapper.Map<ShiftDto>(shiftRepository.GetShift(shiftId));
        }
    }
}
