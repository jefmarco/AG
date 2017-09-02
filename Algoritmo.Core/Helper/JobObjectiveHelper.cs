using Algoritmo.Core.Domain.Model;
using Algoritmo.Core.Repository;
using Algoritmo.Core.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Helper
{
    public class JobObjectiveHelper
    {
        private readonly int _jobId;
        private readonly int _machineId;
        private readonly CsvRepository _csvRepository;

        public JobObjectiveHelper(int jobId, int machineId)
        {
            _jobId = jobId;
            _machineId = machineId;
            _csvRepository = new CsvRepository();
        }
        public TrabajoObjetivo CalculateObjective(int initialTime)
        {
            var job = _csvRepository
                .GetTrabajos()
                .ToList()
                .Where(t => t.Id == _jobId).First();

            var endTime = CalculateEndTime(initialTime, job);

            var offTime = CalculateOffTime(endTime, job);

            var isOverdue = CheckIsOverdue(endTime, job);

            var valueObjective = CalculateValueObjective(offTime, job);

            var trabajo = new TrabajoObjetivo()
            {
                Id = _jobId,
                IsOverdue = isOverdue,
                Objetivo = valueObjective
            };

            return trabajo;
        }

        private int CalculateValueObjective(int offTime, TrabajoInput job)
        {
            var valueObjective = 0;
            if (offTime < 0)
            {
                valueObjective = Math.Abs(offTime) * job.PesoRetraso;
            }
            else
            {
                valueObjective = Math.Abs(offTime) * job.PesoAdelanto;
            }
            return valueObjective;
        }

        private bool CheckIsOverdue(int endTime, TrabajoInput job)
        {
            var offTime = job.FechaLimite - endTime;
            return offTime < 0;
        }

        private int CalculateOffTime(int endTime, TrabajoInput job)
        {
            var offTime = job.FechaEntrega - endTime;
            return offTime;
        }

        private int CalculateEndTime(int initialTime, TrabajoInput job)
        {
            var processTimes = new List<int>() { job.TiempoProcMaquina1, job.TiempoProcMaquina2 }; //hack needs a proper db structure for jobs
            var processTimeJob = processTimes[_machineId-1]; //hack
            var endTime = initialTime + processTimeJob + job.FechaLanzamiento;
            return endTime;
        }
    }
}
