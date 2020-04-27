using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class PersonRepository : IPersonRepository {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public void AddPerson(Person person) {
            dbEntities.instance.Persons.Add(person);
            dbEntities.instance.SaveChanges();
        }

        public void EditPerson(int id, Person person) {
            Person oldViolatior = dbEntities.instance.Persons.Find(id);
            oldViolatior.name = person.name;
            oldViolatior.actual_penalty = person.actual_penalty;
            oldViolatior.paid_penalty = person.paid_penalty;
            oldViolatior.passport_id = person.passport_id;
            oldViolatior.patronymic = person.patronymic;
            oldViolatior.surname = person.surname;
            dbEntities.instance.SaveChanges();
        }

        public void DeletePerson(int id) {
            Person person = dbEntities.instance.Persons.Find(id);
            dbEntities.instance.Persons.Remove(person);
            dbEntities.instance.SaveChanges();
        }

        public Person GetPerson(int id) {
            return dbEntities.instance.Persons.Find(id);
        }

        public HashSet<Person> GetAll() {
            return dbEntities.instance.Persons.ToHashSet();
        }

        public Person GetPersonByLicense(string driverLicense) {
            return dbEntities.instance.Persons.Where(val => val.driver_license == driverLicense).FirstOrDefault();
        }
    }
}
