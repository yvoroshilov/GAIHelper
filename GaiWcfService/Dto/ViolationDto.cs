﻿using System;
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
        public string violationTypeId;

        [DataMember]
        public int personId;

        [DataMember]
        public string carNumber;

        [DataMember]
        public string protocolId;

        [DataMember]
        public DateTime date;

        [DataMember]
        public double penalty;

        [DataMember]
        public double? latitude;

        [DataMember]
        public double? longitude;

        [DataMember]
        public string address;

        [DataMember]
        public string description;

        [DataMember]
        public int shiftId;

        [DataMember]
        public bool paid;

        [DataMember]
        public string docPath;
    }
}
