using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Repository.Model
{
    public class BatchResultOutput
    {
        public int BatchId { get; set; }
        public int MachineId { get; set; }
        public string JobIds { get; set; }
        public int MachineObjective { get; set; }
    }
}
