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
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<Employee, EmployeeDto>()
                    .ForMember("certificateId", expr => expr.MapFrom("certificate_id"))
                    .ForMember("hireDate", expr => expr.MapFrom("hire_date"))
                    .ReverseMap();
                cfg.CreateMap<Payment, PaymentDto>()
                    .ForMember("personId", expr => expr.MapFrom("person_id"))
                    .ForMember("isPaid", expr => expr.MapFrom("is_paid"))
                    .ReverseMap();
                cfg.CreateMap<Shift, ShiftDto>()
                    .ForMember("responsibleId", expr => expr.MapFrom("responsible_id"))
                    .ReverseMap();
                cfg.CreateMap<Violation, ViolationDto>()
                    .ForMember("violationTypeId", expr => expr.MapFrom("violation_type_id"))
                    .ForMember("personId", expr => expr.MapFrom("person_id"))
                    .ForMember("carNumber", expr => expr.MapFrom("car_number"))
                    .ForMember("protocolId", expr => expr.MapFrom("protocol_id"))
                    .ForMember("locationN", expr => expr.MapFrom("location_n"))
                    .ForMember("locationE", expr => expr.MapFrom("location_e"))
                    .ForMember("shiftId", expr => expr.MapFrom("shift_id"))
                    .ReverseMap();
                cfg.CreateMap<ViolationType, ViolationTypeDto>()
                    .ForMember("minPenalty", expr => expr.MapFrom("min_penalty"))
                    .ForMember("maxPenalty", expr => expr.MapFrom("max_penalty"))
                    .ReverseMap();
                cfg.CreateMap<Person, PersonDto>()
                    .ForMember("passportId", expr => expr.MapFrom("passport_id"))
                    .ForMember("driverLicense", expr => expr.MapFrom("driver_license"))
                    .ForMember("actualPenalty", expr => expr.MapFrom("actual_penalty"))
                    .ForMember("paidPenalty", expr => expr.MapFrom("paid_penalty"))
                    .ReverseMap();
            });
            mapper = config.CreateMapper();
        }
    }
}
