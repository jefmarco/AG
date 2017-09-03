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
        private readonly string _outputPath;

        public CsvRepository()
        {
            _inputPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var outputFile = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            _outputPath = outputFile;
        }

        public List<TrabajoInput> GetTrabajos()
        {
            TextReader textReader = File.OpenText(_inputPath);
            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<TrabajoInput>().ToList();
            return records;
        }

        public List<TrabajoMaquinaInput> GetTrabajosMaquina(int index)
        {
            TextReader textReader = File.OpenText(_inputPath);
            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<TrabajoMaquinaInput>().ToList();
            return records;
        }

        public void WriteBatchResults(List<BatchResultOutput> outputs)
        {
            TextWriter textWrite = new StreamWriter(_outputPath);
            var csvWrite = new CsvWriter(textWrite);
            csvWrite.WriteRecords(outputs);
        }
    }
}
