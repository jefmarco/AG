using Algoritmo.Core.Repository;
using Algoritmo.Core.Repository.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Tests.Repository
{
    [TestFixture]
    public class CsvRepositoryTest
    {
        [Test]
        public void Test()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void Test2()
        {
            var csvHelper = new CsvRepository();
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void TestGetTrabajo()
        {
            var csvHelper = new CsvRepository();
            var trabajos = csvHelper.GetTrabajos();
            
            Assert.AreEqual(trabajos.Count, 6);
        }

        [Test]
        public void TestGetTrabajoMaquina1()
        {
            var csvHelper = new CsvRepository();
            var trabajos = csvHelper.GetTrabajosMaquina(1);

            Assert.AreEqual(trabajos.Count, 6);
        }

        [Test]
        public void StoreBatchResult()
        {
            var csvHelper = new CsvRepository();
            var batches = new List<BatchResultOutput>()
            {
                new BatchResultOutput()
                {
                    BatchId = 1,
                    JobIds = "1-2-3-4",
                    MachineId = 1,
                    MachineObjective = 50
                },
                new BatchResultOutput()
                {
                    BatchId = 1,
                    JobIds = "5-6-7",
                    MachineId = 2,
                    MachineObjective = 100
                }
            };

            csvHelper.WriteBatchResults(batches);

            Assert.Null(null);
        }
    }
}
