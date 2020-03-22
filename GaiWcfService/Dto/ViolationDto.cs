using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Dto{
    [DataContract]
    public class ViolationDto {
        [DataMember]
        public int id;

        [DataMember]
        public int violationTypeId;

        [DataMember]
        public int violatorId;

        [DataMember]
        public string car_number;

        [DataMember]
        public DateTime date;

        [DataMember]
        public double penalty;

        [DataMember]
        public double locationN;

        [DataMember]
        public double locationE;

        [DataMember]
        public string description;
    }
}
