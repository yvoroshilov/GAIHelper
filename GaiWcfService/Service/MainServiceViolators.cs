﻿using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;

namespace GaiWcfService.Service {
    public partial class MainService : IAdminService, IUserService {

        private IPersonRepository personRepository = new PersonRepository();

        public void AddPerson(PersonDto person) { 
            personRepository.AddPerson(Mapper.mapper.Map<Person>(person));
        }

        public void EditPerson(int id, PersonDto person) {
            personRepository.EditPerson(id, Mapper.mapper.Map<Person>(person));
        }

        public void DeletePerson(int id) {
            personRepository.DeletePerson(id);
        }

    }
}
