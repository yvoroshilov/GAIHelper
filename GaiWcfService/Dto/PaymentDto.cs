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
        public int id;

        [DataMember] 
        public int personId;

        [DataMember]
        public double amount;

        [DataMember]
        public DateTime date;
    }
}
