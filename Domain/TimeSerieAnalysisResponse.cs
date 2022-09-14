
namespace ElectricityConsumptionAnalyzer.Console
{
    public class TimeSerieAnalysisResponse
    {
        public DateTimeOffset Timestamp { get; set; }
        public bool IsAnomaly { get; set; }
        public float AnomalyValue { get; set; }

        public float Consumption { get; set; }
        public float Severity { get; set; }
        public float LowerMargins { get; set; }
        public float UpperMargins { get; set; }
        public float ExpectedValues { get; set; }
        public bool IsNegativeAnomaly { get; set; }
        public bool IsPositiveAnomaly { get; set; }
        public int Period { get; set; }

        public override string ToString()
        {
            return String.Format(
                $"Timestamp: {Timestamp}, " +
                $"Consumption: {Consumption}, " +
                $"IsAnomaly: {IsAnomaly}, " +
                $"IsNegativeAnomaly: {IsNegativeAnomaly}, " +
                $"IsPositiveAnomaly: {IsPositiveAnomaly}, " +
                $"Severity: {Severity}, " +
                $"LowerMargins: {LowerMargins}, " +
                $"UpperMargins={UpperMargins}, " +
                $"ExpectedValues: {ExpectedValues}");
        }
    }

}
