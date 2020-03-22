using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Dto {
    [DataContract]
    public class ShiftDto {
        [DataMember]
        public int id;

        [DataMember]
        public int responsibleId;

        [DataMember]
        public DateTime start;
        
        [DataMember]
        public DateTime end;
    }
}
