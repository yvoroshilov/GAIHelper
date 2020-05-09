using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System.Collections.Generic;
using System.Linq;

namespace GaiWcfService.Service {
    public partial class MainService {

        private IPersonRepository personRepository = new PersonRepository();

        public PersonDto AddPerson(PersonDto person) { 
            return Mapper.mapper.Map<PersonDto>(personRepository.AddPerson(Mapper.mapper.Map<Person>(person)));
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

        public List<PersonDto> SearchPersonsByPaidPenalty(PersonDto searchedPerson, double minPaidPenalty, double maxPaidPenalty) {
            List<PersonDto> partialResult = SearchPersons(searchedPerson);
            return partialResult.Where(val => val.paidPenalty >= minPaidPenalty && val.paidPenalty <= maxPaidPenalty)
                .ToList();
        }

        public List<PersonDto> SearchPersonsByActualPenalty(PersonDto searchedPerson, double minActualPenalty, double maxActualPenalty) {
            List<PersonDto> partialResult = SearchPersons(searchedPerson);
            return partialResult.Where(val => val.actualPenalty >= minActualPenalty && val.actualPenalty <= maxActualPenalty)
                .ToList();
        }

        public PersonDto GetPersonByDriverLicense(string driverLicense) {
            return Mapper.mapper.Map<PersonDto>(personRepository.GetPersonByLicense(driverLicense));
        }

        public PersonDto GetPerson(int personId) {
            return Mapper.mapper.Map<PersonDto>(personRepository.GetPerson(personId));
        }
    }
}
