using GaiWcfService.Dto;
using System.Collections.Generic;
using System.ServiceModel;

namespace GaiWcfService.Callback {
    public interface ICallbackService {
        
        [OperationContract]
        void SendPenaltyExpired(List<PersonDto> persons);
    }
}