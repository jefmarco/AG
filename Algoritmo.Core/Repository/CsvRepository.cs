using Algoritmo.Core.Repository.Model;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Repository
{
    public class CsvRepository
    {
        private readonly string _inputPath;

        public CsvRepository()
        {
            _inputPath = System.AppDomain.CurrentDomain.BaseDirectory;
        }

        public List<TrabajoInput> GetTrabajos()
        {
            TextReader textReader = File.OpenText(_inputPath);
            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<TrabajoInput>().ToList();
            return records;
        }

        public List<TrabajoMaquinaInput> GetTrabajosMaquina()
        {
            TextReader textReader = File.OpenText(_inputPath);
            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<TrabajoMaquinaInput>().ToList();
            return records;
        }
    }
}
