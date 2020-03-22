using GaiWcfService.Dto;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Service {
    // TODO: make exception for every case
    public class UserService : IUserService {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public void AddPayment(PaymentDto payment) {
            dbEntities.instance.Payments.Add(Mapper.mapper.Map<Payment>(payment));
        }

        public void AddViolation(ViolationDto violation) {
            throw new NotImplementedException();
        }

        public void AddViolator(ViolatorDto violator) {
            throw new NotImplementedException();
        }

        public void DeletePayment(int id) {
            throw new NotImplementedException();
        }

        public void DeleteViolation(int id) {
            throw new NotImplementedException();
        }

        public void DeleteViolator(int id) {
            throw new NotImplementedException();
        }

        public void EditPayment(int id, PaymentDto payment) {
            throw new NotImplementedException();
        }

        public void EditViolation(int id, ViolationDto violation) {
            throw new NotImplementedException();
        }

        public void EditViolator(int id, ViolatorDto violator) {
            throw new NotImplementedException();
        }
    }
}
