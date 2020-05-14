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
                cfg.CreateMap<User, UserDto>()
                    .ReverseMap()
                    .ForMember("Role1", expr => expr.Ignore());
                cfg.CreateMap<Employee, EmployeeDto>()
                    .ForMember("certificateId", expr => expr.MapFrom("certificate_id"))
                    .ForMember("hireDate", expr => expr.MapFrom("hire_date"))
                    .ForMember("userLogin", expr => expr.MapFrom("user_login"))
                    .ReverseMap()
                    .ForMember("User", expr => expr.Ignore());
                cfg.CreateMap<Payment, PaymentDto>()
                    .ForMember("personId", expr => expr.MapFrom("person_id"))
                    .ReverseMap()
                    .ForMember("Person", expr => expr.Ignore());
                cfg.CreateMap<Shift, ShiftDto>()
                    .ForMember("responsibleId", expr => expr.MapFrom("responsible_id"))
                    .ReverseMap()
                    .ForMember("Employee", expr => expr.Ignore());
                cfg.CreateMap<Violation, ViolationDto>()
                    .ForMember("violationTypeId", expr => expr.MapFrom("violation_type_id"))
                    .ForMember("personId", expr => expr.MapFrom("person_id"))
                    .ForMember("carNumber", expr => expr.MapFrom("car_number"))
                    .ForMember("protocolId", expr => expr.MapFrom("protocol_id"))
                    .ForMember("shiftId", expr => expr.MapFrom("shift_id"))
                    .ForMember("docPath", expr => expr.MapFrom("doc_path"))
                    .ReverseMap()
                    .ForMember("Person", expr => expr.Ignore())
                    .ForMember("Shift", expr => expr.Ignore())
                    .ForMember("ViolationType", expr => expr.Ignore());
                cfg.CreateMap<ViolationType, ViolationTypeDto>()
                    .ForMember("minPenalty", expr => expr.MapFrom("min_penalty"))
                    .ForMember("maxPenalty", expr => expr.MapFrom("max_penalty"))
                    .ForMember("paydayAfter", expr => expr.MapFrom("payday_after"))
                    .ReverseMap();
                cfg.CreateMap<Person, PersonDto>()
                    .ForMember("passportId", expr => expr.MapFrom("passport_id"))
                    .ForMember("driverLicense", expr => expr.MapFrom("driver_license"))
                    .ForMember("actualPenalty", expr => expr.MapFrom("actual_penalty"))
                    .ForMember("paidPenalty", expr => expr.MapFrom("paid_penalty"))
                    .ForMember("photoPath", expr => expr.MapFrom("photo_path"))
                    .ReverseMap();
                cfg.CreateMap<Role, RoleDto>()
                    .ForMember("role", expr => expr.MapFrom("role1"))
                    .ReverseMap();
            });
            mapper = config.CreateMapper();
        }

    }
}
