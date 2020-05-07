using AutoMapper;
using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Util {
    public static class Mapper {
        public static IMapper mapper;
        static Mapper() {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<ViolationType, MainService.ViolationTypeDto>().ReverseMap();
            });
            mapper = config.CreateMapper();
        }
    }
}
