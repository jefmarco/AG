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
            var prevJobId = _jobIds.First().JobId;
            var nextJobId = prevJobId;
            var trabajosObjetivo = new List<TrabajoObjetivo>();

            foreach (var job in _jobIds)
            {
                nextJobId = job.JobId;
                var tempJob = job.CalculateObjective(valueObjectivo);
                var machineDelay = GetMachineDelay(prevJobId, nextJobId);
                var temp = tempJob.Objetivo + machineDelay;
                trabajosObjetivo.Add(tempJob);
                valueObjectivo = valueObjectivo + temp;
                prevJobId = nextJobId;
            }

            var maquinaObjetivo = new MaquinaObjetivo()
            {
                Id = _machineId,
                Objetivo = valueObjectivo,
                Trabajos = trabajosObjetivo
            };

            return maquinaObjetivo;
        }

        private int GetMachineDelay(int prevJobId, int nextJobId)
        {
            var trabajosMaquina = _csvRepository.GetTrabajosMaquina(_machineId);
            var trabajoMaquina = trabajosMaquina[prevJobId];
            var jobDelays = new List<int>() { trabajoMaquina.Trabajo1, trabajoMaquina.Trabajo2, trabajoMaquina.Trabajo3, trabajoMaquina.Trabajo4, trabajoMaquina.Trabajo5, trabajoMaquina.Trabajo6 }; //hack
            var jobDelay = jobDelays[nextJobId - 1];
            return jobDelay;
        }
    }
}
