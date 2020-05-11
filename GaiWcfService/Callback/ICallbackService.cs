using GaiWcfService.Dto;
using System.Collections.Generic;
using System.ServiceModel;

namespace GaiWcfService.Callback {
    public interface ICallbackService {
        
        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void SendPenaltyExpired(List<PersonDto> persons);
    }
}