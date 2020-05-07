using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System.Collections.Generic;
using System.Linq;

namespace GaiWcfService.Service {
    public partial class MainService {

        private IPersonRepository personRepository = new PersonRepository();

        public void AddPerson(PersonDto person) { 
            personRepository.AddPerson(Mapper.mapper.Map<Person>(person));
        }

        public void EditPerson(PersonDto person) {
            personRepository.EditPerson(Mapper.mapper.Map<Person>(person));
        }

        public void DeletePerson(int id) {
            personRepository.DeletePerson(id);
        }

        public List<PersonDto> SearchPersons(PersonDto person) {
            return personRepository.SearchPersons(Mapper.mapper.Map<Person>(person))
                .Select(val => Mapper.mapper.Map<PersonDto>(val))
                .ToList();
        }

        public PersonDto GetPersonByDriverLicense(string driverLicense) {
            return Mapper.mapper.Map<PersonDto>(personRepository.GetPersonByLicense(driverLicense));
        }

        public PersonDto GetPerson(int personId) {
            return Mapper.mapper.Map<PersonDto>(personRepository.GetPerson(personId));
        }

        public List<PaymentDto> GetPayments(int personId) {
            return personRepository.GetPerson(personId).Payments
                .Select(val => Mapper.mapper.Map<PaymentDto>(val))
                .ToList();
        }
    }
}
