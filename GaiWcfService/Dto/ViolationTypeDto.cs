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
        public int id { get; set; }

        [DataMember]
        public string title { get; set; }
        
        [DataMember]
        public double minPenalty { get; set; }

        [DataMember]
        public string description { get; set; }
    }
}
