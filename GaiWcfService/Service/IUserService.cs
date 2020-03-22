using GaiWcfService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Service {
    public interface IUserService {
        void AddPayment(PaymentDto payment);

        void EditPayment (int id, PaymentDto payment);

        void DeletePayment(int id);
        
        void AddViolation(ViolationDto violation);

        void EditViolation (int id, ViolationDto violation);

        void DeleteViolation(int id);

        void AddViolator(ViolatorDto violator);

        void EditViolator(int id, ViolatorDto violator);

        void DeleteViolator(int id);
    }
}
