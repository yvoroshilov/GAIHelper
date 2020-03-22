using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GaiWcfService.Service {
    public class AdminService : IAdminService {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        private IAdminRepository adminRepository = new AdminRepository();
        
        private IEmployeeRepository employeeRepository = new EmployeeRepository();

        private IPaymentRepository paymentRepository = new PaymentRepository();

        private IShiftRepository shiftRepository = new ShiftRepository();

        private IViolationRepository violationRepository = new ViolationRepository();

        private IViolationTypeRepository violationTypeRepository = new ViolationTypeRepository();

        private IViolatorRepository violatorRepository = new ViolatorRepository();
        
        public string Test() {
            Admin admin = new Admin() { id = 1, password = "pass", username = "username"};
            AdminDto adminDto = Util.Mapper.mapper.Map<AdminDto>(admin);

            return $"{adminDto.id} -- {adminDto.username} -- {adminDto.password}";
        }

        public void AddAdimn(AdminDto admin) {
            adminRepository.AddAdimn(Mapper.mapper.Map<Admin>(admin));
        }

        public void EditAdmin(int id, AdminDto admin) {
            adminRepository.EditAdmin(id, Mapper.mapper.Map<Admin>(admin));
        }

        public HashSet<AdminDto> getAllAdmins() {
            HashSet<Admin> set = adminRepository.GetAll();
            HashSet<AdminDto> dtoSet = new HashSet<AdminDto>();
            foreach (var item in set) {
                dtoSet.Add(Mapper.mapper.Map<AdminDto>(item));
            }
            return dtoSet;
        }

        public void AddEmployee(EmployeeDto employee) {
            employeeRepository.AddEmployee(Mapper.mapper.Map<Employee>(employee));
        }

        public void EditEmployee(int id, EmployeeDto employee) {
            employeeRepository.EditEmployee(id, Mapper.mapper.Map<Employee>(employee));
        }

        public void DeleteEmployee(int id) {
            employeeRepository.DeleteEmployee(id);
        }

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

        public void EditShift(int id, ShiftDto shift) {
            shiftRepository.EditShift(id, Mapper.mapper.Map<Shift>(shift));
        }

        public void DeleteShift(int id) {
            shiftRepository.DeleteShift(id);
        }

        public void AddViolation(ViolationDto violation) {
            violationRepository.AddViolation(Mapper.mapper.Map<Violation>(violation));
        }

        public void EditViolation(int id, ViolationDto violation) {
            violationRepository.EditViolation(id, Mapper.mapper.Map<Violation>(violation));
        }

        public void DeleteViolation(int id) {
            violationRepository.DeleteViolation(id);
        }

        public void AddViolationType(ViolationTypeDto violationType) {
            violationTypeRepository.AddViolationType(Mapper.mapper.Map<ViolationType>(violationType));
        }

        public void EditViolationType(int id, ViolationTypeDto violationType) {
            violationTypeRepository.EditViolationType(id, Mapper.mapper.Map<ViolationType>(violationType));
        }

        public void DeleteViolationType(int id) {
            violationTypeRepository.DeleteViolationType(id);
        }

        public void AddViolator(ViolatorDto violator) {
            violatorRepository.AddViolator(Mapper.mapper.Map<Violator>(violator));
        }

        public void EditViolator(int id, ViolatorDto violator) {
            violatorRepository.EditViolator(id, Mapper.mapper.Map<Violator>(violator));
        }

        public void DeleteViolator(int id) {
            violatorRepository.DeleteViolator(id);
        }
    }
}
