using Azure;
using Azure.AI.AnomalyDetector;
using Azure.AI.AnomalyDetector.Models;
using Microsoft.Extensions.Configuration;

namespace ElectricityConsumptionAnalyzer.Console
{
    public class AnomalyDetectorService: IAnomalyDetectorService
    {
        private AnomalyDetectorClient _anomalyDetectorClient;
        public AnomalyDetectorService(IConfiguration configuration)
        {
            var cognitiveServiceEndpointUriString = configuration["AzureCognitiveServices:EndPoint"] ?? throw new ArgumentNullException("AzureCognitiveServices:EndPoint is missing");
            var apiKey = configuration["AzureCognitiveServices:Key"] ?? throw new ArgumentNullException("AzureCognitiveServices:Key is missing");
    
            var endpointUri = new Uri(cognitiveServiceEndpointUriString);
            var credential = new AzureKeyCredential(apiKey);
            
            //create client
            _anomalyDetectorClient = new AnomalyDetectorClient(endpointUri, credential);      
        }

        public async Task<List<TimeSerieAnalysisResponse>> AnalyzeTimeSerie(
            IList<TimeSeriesPoint> timeSerie, 
            TimeGranularity timeGranularity, 
            ImputeMode imputeMode, 
            int sensitivity)
        {
            var request = new DetectRequest(timeSerie)
            {
                Granularity = timeGranularity,
                ImputeMode = imputeMode,
                Sensitivity = sensitivity
            };

            var analysisResult =  await _anomalyDetectorClient.DetectEntireSeriesAsync(request).ConfigureAwait(false);

            return await MapData(request.Series, analysisResult);
        }

        /// <summary>
        /// Maps original time serie data and anomaly detection results to unified object
        /// </summary>
        /// <param name="timeSeriesRequest"></param>
        /// <param name="timeSeriesAnalysisResult"></param>
        /// <returns></returns>
        private async Task<List<TimeSerieAnalysisResponse>> MapData(
            IList<TimeSeriesPoint> timeSeriesRequest, 
            EntireDetectResponse timeSeriesAnalysisResult)
        {
            var analysis = new List<TimeSerieAnalysisResponse>();

            for (int i = 0; i < timeSeriesRequest.Count; ++i)
            {
                var data = new TimeSerieAnalysisResponse()
                {
                    Timestamp = timeSeriesRequest[i].Timestamp.Value,
                    Consumption = timeSeriesRequest[i].Value,
                    IsAnomaly = timeSeriesAnalysisResult.IsAnomaly[i]
                };

                if (timeSeriesAnalysisResult.IsAnomaly[i])
                {
                    data.AnomalyValue = timeSeriesRequest[i].Value;
                    data.Severity = timeSeriesAnalysisResult.Severity[i];
                    data.LowerMargins = timeSeriesAnalysisResult.LowerMargins[i];
                    data.UpperMargins = timeSeriesAnalysisResult.UpperMargins[i];
                    data.ExpectedValues = timeSeriesAnalysisResult.ExpectedValues[i];
                    data.IsNegativeAnomaly = timeSeriesAnalysisResult.IsNegativeAnomaly[i];
                    data.IsPositiveAnomaly = timeSeriesAnalysisResult.IsPositiveAnomaly[i];
                    data.Period = timeSeriesAnalysisResult.Period;
                }
                analysis.Add(data);
            }

            return analysis;
        }
    }
}
