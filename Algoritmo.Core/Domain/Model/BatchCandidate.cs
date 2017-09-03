using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Domain.Model
{
    public class BatchCandidate
    {
        public int Id { get; set; }
        public List<MachineCandidate> Machines { get; set; }
    }
}
