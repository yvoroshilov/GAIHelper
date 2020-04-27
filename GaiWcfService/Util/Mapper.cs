using AutoMapper;
using GaiWcfService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Util {
    public static class Mapper {
        public static IMapper mapper;
        static Mapper() {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Admin, AdminDto>().ReverseMap();
                cfg.CreateMap<Employee, EmployeeDto>().ReverseMap();
                cfg.CreateMap<Payment, PaymentDto>().ReverseMap();
                cfg.CreateMap<Shift, ShiftDto>();
                cfg.CreateMap<Violation, ViolationDto>().ReverseMap();
                cfg.CreateMap<ViolationType, ViolationTypeDto>().ReverseMap();
                cfg.CreateMap<Person, PersonDto>().ReverseMap();
            });
            mapper = config.CreateMapper();
        }
    }
}
