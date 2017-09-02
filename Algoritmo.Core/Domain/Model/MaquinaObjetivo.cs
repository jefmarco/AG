using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Domain.Model
{
    public class MaquinaObjetivo
    {
        public string Name { get; set; }
        public List<int> Trabajos { get; set; }
        public int Objetivo { get; set; }
    }
}
