﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Dto {
    [DataContract]
    public class RoleDto {

        [DataMember]
        public string role;
    }
}
