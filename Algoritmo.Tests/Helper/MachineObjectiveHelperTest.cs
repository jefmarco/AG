using NUnit.Framework;
using Algoritmo.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Tests.Helper
{
    [TestFixture]
    class MachineObjectiveHelperTest
    {
        [Test]
        public void Test()
        {
            var machineId = 1;

         MachineObjectiveHelper m = new MachineObjectiveHelper(jobIds, machineId);
        }
        
    }
}
