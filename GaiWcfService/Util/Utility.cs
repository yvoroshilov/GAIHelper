﻿using GaiWcfService.Repository.contract;
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
            double overallCost = person.Violations.Aggregate(0.0, (a, b) => a + b.penalty);
            double paidPenalty = (double)person.paid_penalty;
            foreach (Violation item in person.Violations) {
                if (paidPenalty - item.penalty < 0) {
                    break;
                }
                paidPenalty -= item.penalty;
                item.paid = true;
                violationRepository.EditViolation(item);
            }
            person.actual_penalty = (decimal)overallCost;
            if (person.paid_penalty > (decimal)overallCost) {
                person.paid_penalty = (decimal)overallCost;
            }
            personRepository.EditPerson(person);
        }
    }
}