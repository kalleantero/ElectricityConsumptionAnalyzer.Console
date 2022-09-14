using Azure.AI.AnomalyDetector.Models;

namespace ElectricityConsumptionAnalyzer.Console
{
    public interface IAnomalyDetectorService
    {
        Task<List<TimeSerieAnalysisResponse>> AnalyzeTimeSerie(IList<TimeSeriesPoint> timeSerie, TimeGranularity timeGranularity, ImputeMode imputeMode, int sensitivity);
    }
}
