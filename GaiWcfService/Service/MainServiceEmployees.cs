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
        private readonly IEmployeeRepository employeeRepository = new EmployeeRepository();

        public void AddEmployee(EmployeeDto employee) {
            employeeRepository.AddEmployee(Mapper.mapper.Map<Employee>(employee));
        }

        public EmployeeDto GetEmployeeByUserLogin(string login) {
            EmployeeDto empDto = Mapper.mapper.Map<EmployeeDto>(employeeRepository.GetEmployeeByUserLogin(login));
            return empDto;
        }

        public void EditEmployee(int id, EmployeeDto employee) {
            employeeRepository.EditEmployee(id, Mapper.mapper.Map<Employee>(employee));
        }

        public void DeleteEmployee(int id) {
            employeeRepository.DeleteEmployee(id);
        }

    }
}
