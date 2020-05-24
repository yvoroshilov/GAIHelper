using GaiWcfService.Dto;
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

        public void AddUser(UserDto user) {
            userRepository.AddUser(Mapper.mapper.Map<User>(user));
        }

        public void EditUser(UserDto user) {
            userRepository.EditUser(Mapper.mapper.Map<User>(user));
        }

        public UserDto GetUser(string login) {
            return Mapper.mapper.Map<UserDto>(userRepository.GetUser(login));
        }

        public List<UserDto> SearchUsers(UserDto searchedUser) {
            return userRepository.SearchUser(Mapper.mapper.Map<User>(searchedUser))
                .Select(val => Mapper.mapper.Map<UserDto>(val))
                .ToList();
        }

        public void DeleteUser(string login) {
            userRepository.DeleteUser(login);
        }
    }
}
