using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Repository.Model
{
    public class TrabajoInput
    {
        public int Id { get; set; }
        public int TiempoProcMaquina1 { get; set; }
        public int TiempoProcMaquina2 { get; set; }
        public int PesoRetraso { get; set; }
        public int PesoAdelanto { get; set; }
        public int FechaEntrega { get; set; }
        public int FechaLanzamiento { get; set; }
        public int FechaLimite { get; set; }
    }
}
