using Algoritmo.Core.Domain.Model;
using Algoritmo.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Helper
{
    public class MachineObjectiveHelper
    {
        private readonly List<JobObjectiveHelper> _jobIds;
        private readonly int _machineId;
        private readonly CsvRepository _csvRepository;
        public MachineObjectiveHelper(List<int> jobIds, int machineId)
        {
            _jobIds = jobIds.Select(j => new JobObjectiveHelper(j, machineId)).ToList();
            _machineId = machineId;
            _csvRepository = new CsvRepository();
        }

        public MaquinaObjetivo CalculateObjective()
        {
            var valueObjectivo = 0;
            var machineDelay = 0;
            var prevJobId = 0;
            var nextJobId = 0;

            foreach (var job in _jobIds)
            {
                var tempJob = job.CalculateObjective(valueObjectivo);
                var temp = tempJob.Objetivo + machineDelay;
                valueObjectivo = temp;
                machineDelay = GetMachineDelay(tempJob.Id);
            }
        }

        private int GetMachineDelay(int jobId)
        {
            var trabajosMaquina = _csvRepository.GetTrabajosMaquina(_machineId);
            var trabajoMaquina = trabajosMaquina[jobId];
            var jobDelays = new List<int>() { trabajoMaquina.Trabajo1, trabajoMaquina.Trabajo2, trabajoMaquina.Trabajo3, trabajoMaquina.Trabajo4, trabajoMaquina.Trabajo5, trabajoMaquina.Trabajo6 }; //hack
            var jobDelay = jobDelays[jobId - 1];
            return jobDelay;
        }
    }
}
