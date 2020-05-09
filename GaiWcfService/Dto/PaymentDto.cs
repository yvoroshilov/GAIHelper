using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Dto {
    [DataContract]
    public class PaymentDto {
        [DataMember]
        public int id { get; set; }

        [DataMember] 
        public int personId { get; set; }

        [DataMember]
        public double amount { get; set; }

        [DataMember]
        public DateTime date { get; set; }
    }
}
