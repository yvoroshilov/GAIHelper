using GaiWcfService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GaiWcfService.Service {
    public class AdminService : IAdminService {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public string Test() {
            Admin admin = new Admin() { id = 1, password = "pass", username = "username"};
            AdminDto adminDto = Util.Mapper.mapper.Map<AdminDto>(admin);

            return $"{adminDto.id} -- {adminDto.username} -- {adminDto.password}";
        }
        public void AddEmployee(EmployeeDto employee) {
            throw new NotImplementedException();
        }

        public void AddPayment(PaymentDto payment) {
            throw new NotImplementedException();
        }

        public void AddShift(ShiftDto shift) {
            throw new NotImplementedException();
        }

        public void AddViolation(ViolationDto violation) {
            throw new NotImplementedException();
        }

        public void AddViolationType(ViolationTypeDto violationType) {
            throw new NotImplementedException();
        }

        public void AddViolator(ViolatorDto violator) {
            throw new NotImplementedException();
        }

        public void DeleteEmployee(int id) {
            throw new NotImplementedException();
        }

        public void DeletePayment(int id) {
            throw new NotImplementedException();
        }

        public void DeleteShift(int id) {
            throw new NotImplementedException();
        }

        public void DeleteViolation(int id) {
            throw new NotImplementedException();
        }

        public void DeleteViolationType(int id) {
            throw new NotImplementedException();
        }

        public void DeleteViolator(int id) {
            throw new NotImplementedException();
        }

        public void EditEmployee(int id, Employee employee) {
            throw new NotImplementedException();
        }

        public void EditPayment(int id, PaymentDto payment) {
            throw new NotImplementedException();
        }

        public void EditShift(int id, ShiftDto shift) {
            throw new NotImplementedException();
        }

        public void EditViolation(int id, ViolationDto violation) {
            throw new NotImplementedException();
        }

        public void EditViolationType(int id, ViolationTypeDto violationType) {
            throw new NotImplementedException();
        }

        public void EditViolator(int id, ViolatorDto violator) {
            throw new NotImplementedException();
        }
    }
}
