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
                cfg.CreateMap<Admin, MainService.AdminDto>().ReverseMap();
                cfg.CreateMap<Employee, MainService.EmployeeDto>().ReverseMap();
                //cfg.CreateMap<Payment, MainService.PaymentDto>().ReverseMap();
                cfg.CreateMap<Shift, MainService.ShiftDto>();
                cfg.CreateMap<Violation, MainService.ViolationDto>().ReverseMap();
                cfg.CreateMap<ViolationType, MainService.ViolationTypeDto>().ReverseMap();
                cfg.CreateMap<Person, MainService.PersonDto>().ReverseMap();
            });
            mapper = config.CreateMapper();
        }
    }
}
