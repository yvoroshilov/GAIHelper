using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class PersonRepository : IPersonRepository {
        private GAIDBEntities dbEntities = DbEntitiesSingleton.Instance.GetDbEntities();

        public Person AddPerson(Person person) {
            Person added = dbEntities.Persons.Add(person);
            dbEntities.SaveChanges();
            return added;
        }

        public void EditPerson(Person person) {
            Person oldViolatior = dbEntities.Persons.Find(person.id);
            oldViolatior.name = person.name;
            oldViolatior.actual_penalty = person.actual_penalty;
            oldViolatior.paid_penalty = person.paid_penalty;
            oldViolatior.passport_id = person.passport_id;
            oldViolatior.patronymic = person.patronymic;
            oldViolatior.surname = person.surname;
            oldViolatior.birthday = person.birthday;
            oldViolatior.driver_license = person.driver_license;
            dbEntities.SaveChanges();
        }

        public void DeletePerson(int id) {
            Person person = dbEntities.Persons.Find(id);
            dbEntities.Persons.Remove(person);
            dbEntities.SaveChanges();
        }

        public Person GetPerson(int id) {
            return dbEntities.Persons.Find(id);
        }

        public HashSet<Person> GetAll() {
            return dbEntities.Persons.ToHashSet();
        }

        public Person GetPersonByLicense(string driverLicense) {
            return dbEntities.Persons.Where(val => val.driver_license == driverLicense).FirstOrDefault();
        }

        public List<Person> SearchPersons(Person searchedPerson) {
            return dbEntities.Persons.Where(val =>
                (searchedPerson.id == default || val.id == searchedPerson.id) &&
                (searchedPerson.passport_id == default || val.passport_id == searchedPerson.passport_id) &&
                (searchedPerson.driver_license == default || val.driver_license == searchedPerson.driver_license) &&
                (searchedPerson.name == default || val.name.Contains(searchedPerson.name)) &&
                (searchedPerson.surname == default || val.surname.Contains(searchedPerson.surname)) &&
                (searchedPerson.patronymic == default || val.patronymic.Contains(searchedPerson.patronymic)) &&
                (searchedPerson.birthday == default || val.birthday == searchedPerson.birthday) &&
                (searchedPerson.actual_penalty == default || val.actual_penalty == searchedPerson.actual_penalty) &&
                (searchedPerson.paid_penalty == default || val.paid_penalty == searchedPerson.paid_penalty))
                .ToList();
        }
    }
}
