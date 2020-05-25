using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Util {
    public static class Utility {
        private static readonly IPersonRepository personRepository = new PersonRepository();
        private static readonly IViolationRepository violationRepository = new ViolationRepository();

        public static void CalculateViolationsPaid(int personId) {
            Person person = personRepository.GetPerson(personId);
            List<Violation> violations = violationRepository.GetAllViolations(person.id);
            double overallCost = violations.Aggregate(0.0, (a, b) => a + b.penalty);
            double paidPenalty = (double)person.paid_penalty;
            foreach (Violation item in violations) {
                if (SafeLessThan(paidPenalty - item.penalty, 0)) {
                    if (item.paid) {
                        item.paid = false;
                        violationRepository.EditViolation(item);
                    }
                } else {
                    paidPenalty -= item.penalty;
                    if (!item.paid) {
                        item.paid = true;
                        violationRepository.EditViolation(item);
                    }
                }
            }
            person.actual_penalty = (decimal)overallCost;
            if (person.paid_penalty > (decimal)overallCost) {
                person.paid_penalty = (decimal)overallCost;
            }
            personRepository.EditPerson(person);
        }

        public static bool SafeLessThan(double a, double b) {
            double epsilon = 0.01;
            double diff = Math.Abs(a - b);
            return (diff >= epsilon && a < b);
        }
    }
}
