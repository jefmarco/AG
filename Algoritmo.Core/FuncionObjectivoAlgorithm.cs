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
    public class FuncionObjectivoAlgorithm
    {
        private readonly CsvRepository _csvRepository;
        public FuncionObjectivoAlgorithm()
        {
            _csvRepository = new CsvRepository();
        }

        public TotalObjetivo CalculateAllOutputs(List<TrabajoInput> trabajos, List<TrabajoMaquinaInput> trabajoMaquinasCoste)
        {
            return null;
        }
    }
}
