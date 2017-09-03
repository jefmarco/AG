using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Domain.Model
{
    public class TotalObjetivo
    {
        public List<MaquinaObjetivo> MaquinaObjetivos { get; set; }
        public int Objetivo { get; set; }
    }
}
