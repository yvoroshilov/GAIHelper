using System.ServiceModel;

namespace GaiWcfService.Callback {
    public interface ICallbackService {
        
        [OperationContract]
        string Test(string str);
    }
}