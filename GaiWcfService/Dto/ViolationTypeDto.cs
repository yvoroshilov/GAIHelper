using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Dto {
    [DataContract]
    public class ViolationTypeDto {
        [DataMember]
        public string id;

        [DataMember]
        public string title;
        
        [DataMember]
        public double minPenalty;

        [DataMember]
        public double maxPenalty;

        [DataMember]
        public string description;

        [DataMember]
        public int paydayAfter;
    }
}
