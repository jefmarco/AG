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
        public RandomAlgorithm()
        {
            _csvRepository = new CsvRepository();
        }

        public List<TotalObjetivo> CalculateAllObjectives(List<TrabajoInput> trabajos, List<TrabajoMaquinaInput> trabajoMaquinasCoste)
        {
            return null;
        }

        public TotalObjetivo CalculateMinObjective(List<TrabajoInput> trabajos, List<TrabajoMaquinaInput> trabajoMaquinasCoste)
        {
            return null;
        }
    }
}
