using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Dto {
    [DataContract]
    public class AdminDto {
        [DataMember]
        public int id { get; set; }
        
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string password { get; set; }
    }
}
