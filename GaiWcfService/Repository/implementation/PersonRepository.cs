﻿using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GaiWcfService.Util.DbEntitiesSingleton;

namespace GaiWcfService.Repository.implementation {
    public class PersonRepository : IPersonRepository {
        public Person AddPerson(Person person) {
            GAIDBEntities entities = dbEntities;
            Person added = entities.Persons.Add(person);
            entities.SaveChanges();
            return added;
        }

        public void EditPerson(Person person) {
            GAIDBEntities entities = dbEntities;
            Person oldPerson = entities.Persons.Find(person.id);
            oldPerson.name = person.name;
            oldPerson.actual_penalty = person.actual_penalty;
            oldPerson.paid_penalty = person.paid_penalty;
            oldPerson.passport_id = person.passport_id;
            oldPerson.patronymic = person.patronymic;
            oldPerson.surname = person.surname;
            oldPerson.birthday = person.birthday;
            oldPerson.driver_license = person.driver_license;
            oldPerson.email = person.email;
            oldPerson.photo = person.photo;
            entities.SaveChanges();
        }

        public void DeletePerson(int id) {
            GAIDBEntities entities = dbEntities;
            Person person = entities.Persons.Find(id);
            entities.Persons.Remove(person);
            entities.SaveChanges();
        }

        public Person GetPerson(int id) {
            return dbEntities.Persons.Find(id);
        }

        public Person GetPersonByLicense(string driverLicense) {
            return dbEntities.Persons.Where(val => val.driver_license == driverLicense).FirstOrDefault();
        }

        public List<Person> SearchPersons(Person searchedPerson) {
            return dbEntities.Persons.Where(val =>
                (searchedPerson.id == default || val.id == searchedPerson.id) &&
                (searchedPerson.passport_id == default || val.passport_id == searchedPerson.passport_id) &&
                (searchedPerson.driver_license == default || val.driver_license == searchedPerson.driver_license) &&
                (searchedPerson.name == default || val.name.ToLower().Contains(searchedPerson.name.ToLower())) &&
                (searchedPerson.surname == default || val.surname.ToLower().Contains(searchedPerson.surname.ToLower())) &&
                (searchedPerson.patronymic == default || val.patronymic.ToLower().Contains(searchedPerson.patronymic.ToLower())) &&
                (searchedPerson.birthday == default || val.birthday == searchedPerson.birthday) &&
                (searchedPerson.actual_penalty == default || val.actual_penalty == searchedPerson.actual_penalty) &&
                (searchedPerson.paid_penalty == default || val.paid_penalty == searchedPerson.paid_penalty))
                .ToList();
        }

        public List<Person> GetExpiredDebtors() {
            return dbEntities.Violations
                .Where(val => !val.paid && 
                    val.Person.driver_license != "NO_LIC" &&
                    DbFunctions.DiffDays(val.date, DateTime.Now) >= val.ViolationType.payday_after)
                .GroupBy(val => val.Person.id)
                .Select(val => val.FirstOrDefault().Person)
                .ToList();

        }

        public Person GetPersonByPassportId(string passportId) {
            return dbEntities.Persons
                .Where(val => val.passport_id == passportId)
                .SingleOrDefault();
        }

        public Person GetPersonByDriverLicense(string driverLicense) {
            return dbEntities.Persons
                .Where(val => val.driver_license == driverLicense)
                .SingleOrDefault();
        }
    }
}
