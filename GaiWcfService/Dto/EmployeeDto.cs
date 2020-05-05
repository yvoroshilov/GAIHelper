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
        public int certificateId { get; set; }

        [DataMember] 
        public string name { get; set; }

        [DataMember] 
        public string surname { get; set; }

        [DataMember] 
        public string patronymic { get; set; }

        [DataMember]
        public DateTime hireDate { get; set; }

        [DataMember] 
        public string userLogin { get; set; }
    }
}
