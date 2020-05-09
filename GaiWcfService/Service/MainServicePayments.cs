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
    public partial class MainService {

        private IPaymentRepository paymentRepository = new PaymentRepository();

        public List<PaymentDto> GetPaymentsByPersonId(int personId) {
            return paymentRepository.GetPaymentsByPersonId(personId)
                .Select(val => Mapper.mapper.Map<PaymentDto>(val))
                .ToList();
        }

        public List<PaymentDto> GetLastNPayments(int n) {
            return paymentRepository.GetLastNPayments(n)
                .Select(val => Mapper.mapper.Map<PaymentDto>(val))
                .ToList();
        }
    }
}
