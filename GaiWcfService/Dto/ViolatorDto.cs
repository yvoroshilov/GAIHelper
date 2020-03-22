using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GaiWcfService.Dto {
    [DataContract]
    public class ViolatorDto {
        [DataMember]
        public int id;

        [DataMember]
        public string passportId;

        [DataMember]
        public string name;

        [DataMember]
        public string surname;

        [DataMember]
        public string patronymic;

        [DataMember]
        public double actual_penalty;

        [DataMember]
        public double paid_penalty;
    }
}