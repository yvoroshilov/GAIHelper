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

        private IPaymentRepository paymentRepository = new PaymentRepository();

        public void AddPayment(PaymentDto payment) {
            paymentRepository.AddPayment(Mapper.mapper.Map<Payment>(payment));
        }

        public void EditPayment(int id, PaymentDto payment) {
            paymentRepository.EditPayment(id, Mapper.mapper.Map<Payment>(payment));
        }

        public void DeletePayment(int id) {
            paymentRepository.DeletePayment(id);
        }

        public void AddShift(ShiftDto shift) {
            shiftRepository.AddShift(Mapper.mapper.Map<Shift>(shift));
        }
    }
}
