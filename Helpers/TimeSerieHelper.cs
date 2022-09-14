using Azure.AI.AnomalyDetector.Models;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace ElectricityConsumptionAnalyzer.Console
{
    public class TimeSerieHelper
    {
        public static IList<TimeSeriesPoint> ReadCsv(string csvFilePath)
        {
            return File.ReadAllLines(csvFilePath, Encoding.UTF8)
                .Where(e => e.Trim().Length != 0)
                .Select(e => e.Split(';'))
                .Where(e => e.Length == 2)
                .Select(e => CreateTimeSerie(e)).ToList();
        }

        static TimeSeriesPoint CreateTimeSerie(string[] e)
        {
            var timestampString = e[0];
            var consumption = e[1];
            return new TimeSeriesPoint(float.Parse(e[1])) { Timestamp = DateTime.Parse(timestampString) };
        }

        public static void WriteCsv(string csvFilePath, List<TimeSerieAnalysisResponse> timeSeriesAnalysisResult)
        {
            using (var writer = new StreamWriter(csvFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(timeSeriesAnalysisResult);
            }
        }
    }
}
