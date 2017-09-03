using Algoritmo.Core.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Helper
{
    public class BatchObjectiveHelper
    {
        public readonly BatchCandidate _candidate;
        public BatchObjectiveHelper(BatchCandidate Candidate)
        {
            _candidate = Candidate;
        }

        public TotalObjetivo CalculateTotalBatch()
        {
            var machineHelpers = GetMachineHelpers();
            var objetivos = new List<MaquinaObjetivo>();

            foreach(var helper in machineHelpers)
            {
                var temp = helper.CalculateObjective();
                objetivos.Add(temp);
            }

            var total = objetivos.Sum(o => o.Objetivo);

            var totalObjetivo = new TotalObjetivo()
            {
                Objetivo = total,
                MaquinaObjetivos = objetivos
            };

            return totalObjetivo;
        }

        private List<MachineObjectiveHelper> GetMachineHelpers()
        {
            var machineHelpers = new List<MachineObjectiveHelper>();
            foreach (var machine in _candidate.Machines)
            {
                var machineHelper = new MachineObjectiveHelper(machine.JobIds, machine.Id);
                machineHelpers.Add(machineHelper);
            }
            return machineHelpers;
        }
    }
}
