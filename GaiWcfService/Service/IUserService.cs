using GaiWcfService.Callback;
using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Service {

    [ServiceContract]
    public interface IUserService {
        [OperationContract]
        List<ViolationDto> GetAllViolations(int personId);

        [OperationContract]
        ViolationDto AddViolation(ViolationDto violation);

        [OperationContract]
        void EditViolation(ViolationDto violation);

        [OperationContract]
        void DeleteViolation(int id);

        [OperationContract]
        List<ViolationTypeDto> GetAllViolationTypes();

        [OperationContract]
        PersonDto GetPersonByDriverLicense(string driverLicense);

        [OperationContract]
        ViolationDto AddViolationFile(int violationId, byte[] file, string filename);

        [OperationContract]
        byte[] GetViolationFile(int violationId);

        [OperationContract]
        void RemoveViolationFile(int violationId);
    }
}
