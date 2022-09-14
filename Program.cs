using Azure.AI.AnomalyDetector.Models;
using ElectricityConsumptionAnalyzer.Console;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

var csvInputPath = configuration["Csv:InPath"];
var csvOutputPath = configuration["Csv:OutPath"];

Console.WriteLine("Hello, press any key to start anomaly detection.");

var anomalyDetectorService = new AnomalyDetectorService(configuration);

// creates time serie data from CSV file
var timeSerie = TimeSerieHelper.ReadCsv(csvInputPath);

// analyzes time serie data
var timeSerieAnalysis = await anomalyDetectorService.AnalyzeTimeSerie(timeSerie, TimeGranularity.Hourly, ImputeMode.Auto, 99);

// write results to CSV file
TimeSerieHelper.WriteCsv(csvOutputPath, timeSerieAnalysis);

Console.WriteLine("Anomaly detection completed.");
