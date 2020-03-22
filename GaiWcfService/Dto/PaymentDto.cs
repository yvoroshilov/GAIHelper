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
        public int violatorId { get; set; }

        [DataMember]
        public DateTime payday { get; set; }

        [DataMember]
        public Boolean isPaid { get; set; }
    }
}
