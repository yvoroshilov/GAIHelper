using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    public interface IPersonRepository {
        void AddPerson(Person person);

        void EditPerson(Person person);

        void DeletePerson(int id);

        Person GetPerson(int id);

        Person GetPersonByLicense(string driverLicense);

        List<Person> SearchPersons(Person searchedPerson);

        HashSet<Person> GetAll();
    }
}
