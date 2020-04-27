using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    [ServiceContract]
    public interface IPersonRepository {
        [OperationContract]
        void AddPerson(Person person);

        [OperationContract]
        void EditPerson(int id, Person person);

        [OperationContract]
        void DeletePerson(int id);

        [OperationContract]
        Person GetPerson(int id);

        [OperationContract]
        HashSet<Person> GetAll();
    }
}
