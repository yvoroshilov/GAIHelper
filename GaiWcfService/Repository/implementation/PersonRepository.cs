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

        public void AddPerson(Person person) {
            dbEntities.Persons.Add(person);
            dbEntities.SaveChanges();
        }

        public void EditPerson(int id, Person person) {
            Person oldViolatior = dbEntities.Persons.Find(id);
            oldViolatior.name = person.name;
            oldViolatior.actual_penalty = person.actual_penalty;
            oldViolatior.paid_penalty = person.paid_penalty;
            oldViolatior.passport_id = person.passport_id;
            oldViolatior.patronymic = person.patronymic;
            oldViolatior.surname = person.surname;
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
    }
}
