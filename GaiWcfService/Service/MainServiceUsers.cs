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

        public byte[] GetTest() {
            byte[] arr = new byte[5000000];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = 255;
            }
            return arr;
        }

        public void SetTest(int test) {
            ViolationDto viol = GetAllViolations(1).Last();
            viol.personId = 12;
            EditViolation(viol);
        }
        
        public void AddUser(UserDto user) {
            userRepository.AddUser(Mapper.mapper.Map<User>(user));
        }

        public void EditUser(UserDto user) {
            userRepository.EditUser(Mapper.mapper.Map<User>(user));
        }

        public UserDto GetUser(string login) {
            return Mapper.mapper.Map<UserDto>(userRepository.GetUser(login));
        }

        public HashSet<UserDto> getAllUsers() {
            return userRepository.GetAll()
                .Select(val => Mapper.mapper.Map<UserDto>(val))
                .ToHashSet();
        }

        public List<UserDto> SearchUsers(UserDto searchedUser) {
            return userRepository.SearchUser(Mapper.mapper.Map<User>(searchedUser))
                .Select(val => Mapper.mapper.Map<UserDto>(val))
                .ToList();
        }

        public void DeleteUser(string login) {
            userRepository.DeleteUser(login);
        }

        public string test() {
            return "TEST";
        }
    }
}
