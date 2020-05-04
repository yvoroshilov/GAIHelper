﻿using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;

namespace GaiWcfService.Service {
    
    public partial class MainService {

        private IUserRepository userRepository = new UserRepository();

        public Message GetTest() {
            return OperationContext.Current.RequestContext.RequestMessage;
        }

        public void SetTest(int test) {
        }
        
        public void AddUser(UserDto user) {
            userRepository.AddUser(Mapper.mapper.Map<User>(user));
        }

        public void EditUser(string login, UserDto user) {
            userRepository.EditUser(login, Mapper.mapper.Map<User>(user));
        }

        public UserDto GetUser(string login) {
            return Mapper.mapper.Map<UserDto>(userRepository.GetUser(login));
        }

        public HashSet<UserDto> getAllUsers() {
            return userRepository.GetAll()
                .Select(val => Mapper.mapper.Map<UserDto>(val))
                .ToHashSet();
        }

        public string test() {
            return "TEST";
        }
    }
}