using Algoritmo.Core.Domain.Model;
using Algoritmo.Core.Repository;
using Algoritmo.Core.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core
{
    public class RandomAlgorithm
    {
        private readonly CsvRepository _csvRepository;
        public const int TotalMachines = 2;
        public RandomAlgorithm()
        {
            _csvRepository = new CsvRepository();
        }
        

        public List<BatchCandidate> GenerateRandomBatches(int number)
        {
            var batches = new List<BatchCandidate>();
            for (int i = 0; i<number; i++)
            {
                var temp = CreateBatch(i);
                batches.Add(temp);
            }
            return batches;
        }

        private BatchCandidate CreateBatch(int i)
        {
            var maquina1 = new MachineCandidate() { Id = 1, JobIds = new List<int>() };
            var maquina2 = new MachineCandidate() { Id = 2, JobIds = new List<int>() };
            var trabajos = _csvRepository.GetTrabajos();

            var random = new Random();

            foreach (var trabajo in trabajos)
            {
                var randomNumber = random.Next(0, 10);
                if (randomNumber < 5)
                {
                    maquina1.JobIds.Add(trabajo.Id);
                }
                else
                {
                    maquina2.JobIds.Add(trabajo.Id);
                }
            }

            var batch = new BatchCandidate()
            {
                Id = i,
                Machines = new List<MachineCandidate>() { maquina1, maquina2 }
            };

            return batch;
        }
        
    }
}
