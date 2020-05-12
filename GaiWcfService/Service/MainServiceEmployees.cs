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

        public EmployeeDto GetEmployeeById(int id) {
            return Mapper.mapper.Map<EmployeeDto>(employeeRepository.GetEmployee(id));
        }

        public void EditEmployee(EmployeeDto employee) {
            employeeRepository.EditEmployee(Mapper.mapper.Map<Employee>(employee));
        }

        public List<EmployeeDto> SearchEmployees(EmployeeDto employee) {
            return employeeRepository.SearchEmployees(Mapper.mapper.Map<Employee>(employee))
                .Select(val => Mapper.mapper.Map<EmployeeDto>(val))
                .ToList();
        }

        public void DeleteEmployee(int id) {
            employeeRepository.DeleteEmployee(id);
        }

    }
}
