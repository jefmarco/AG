using Algoritmo.Core.Domain.Model;
using Algoritmo.Core.Helper;
using Algoritmo.Core.Repository;
using Algoritmo.Core.Repository.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Tests.Helper
{
    [TestFixture]
    public class JobObjectiveHelperTest
    {
        [Test]
        public void Test()

        {
            var machineId = 1;
            var jobId = 1;
            JobObjectiveHelper joboh = new JobObjectiveHelper(jobId, machineId);
            
                Assert.AreEqual(jobId, machineId);
             
        }
                 
    }


            
}
    

