using Algoritmo.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Tests.RandomAlgorithmTest
{
    [TestFixture]
    public class RandomAlgorithmtest
    {
        [Test]
        public void Test()
        {
            RandomAlgorithm p = new RandomAlgorithm();
            var batches = p.GenerateRandomBatches(5);

            Assert.AreEqual(batches.Count, 5);
        }








    }
}
