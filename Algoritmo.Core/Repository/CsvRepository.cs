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
        public const string InputFileName = "Input-1-Trabajos.csv";
        public const string InputTrabajoMaquina = "Input-1-TrabajoMaquina";
        public const string Csv = ".csv";
        private readonly string _inputPath;
        private readonly string _outputPath;

        public CsvRepository()
        {
            var x = Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent;
            
            _inputPath = x.FullName + @"\Resources\Input\";
            _outputPath = x.FullName + @"\Resources\Output\"; 
        }

        public List<TrabajoInput> GetTrabajos()
        {
            var inputFile = _inputPath + InputFileName;
            TextReader textReader = File.OpenText(inputFile);
            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<TrabajoInput>().ToList();
            return records;
        }

        public List<TrabajoMaquinaInput> GetTrabajosMaquina(int index)
        {
            var inputFile = $"{_inputPath}{InputTrabajoMaquina}-{index}{Csv}";
            TextReader textReader = File.OpenText(inputFile);
            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<TrabajoMaquinaInput>().ToList();
            return records;
        }

        public void WriteBatchResults(List<BatchResultOutput> outputs)
        {
            var dateFile = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            var outputFileName = _outputPath + "Output-" + dateFile + Csv;
            TextWriter textWrite = new StreamWriter(outputFileName);
            var csvWrite = new CsvWriter(textWrite);
            csvWrite.WriteRecords(outputs);
            textWrite.Flush();
            textWrite.Close();
        }
    }
}
