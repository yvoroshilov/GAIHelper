using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Dto {
    [DataContract]
    public class EmployeeDto {
        [DataMember] 
        public int certificateId;

        [DataMember] 
        public string name;

        [DataMember] 
        public string surname;

        [DataMember] 
        public string patronymic;

        [DataMember]
        public DateTime hireDate;

        [DataMember] 
        public string userLogin;
    }
}
