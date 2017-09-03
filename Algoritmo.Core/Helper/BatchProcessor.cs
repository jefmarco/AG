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
    public class BatchProcessor
    {
        private readonly CsvRepository _csvRepository;
        public BatchProcessor()
        {
            _csvRepository = new CsvRepository();
        }

        public void Process(List<BatchCandidate> candidates)
        {
            var outputs = new List<BatchResultOutput>();
            var candidateHelpers = candidates.Select(c => new BatchObjectiveHelper(c)).ToList();
            foreach(var helper in candidateHelpers)
            {
                var temp = helper.CalculateTotalBatch();
                AddResultToOutput(temp, outputs, helper._candidate);
            }

            _csvRepository.WriteBatchResults(outputs);
        }

        private void AddResultToOutput(TotalObjetivo total, List<BatchResultOutput> outputs, BatchCandidate candidate)
        {
            foreach (var machine in total.MaquinaObjetivos)
            {
                var temp = new BatchResultOutput()
                {
                    BatchId = candidate.Id,
                    JobIds = machine.Trabajos.Select(t => "Id:" + t.Id + ".").ToString(),
                    MachineId = machine.Id,
                    MachineObjective = machine.Objetivo
                };

                outputs.Add(temp);
            }
        }
    }
}
