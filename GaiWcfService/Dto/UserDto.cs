using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Dto {
    [DataContract]
    public class UserDto {

        [DataMember]
        public int id;

        [DataMember]
        public string login;

        [DataMember]
        public string password;

        [DataMember]
        public string role;
    }
}
