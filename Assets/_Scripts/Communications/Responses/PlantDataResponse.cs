using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Communications.Responses
{
    public class PlantDataResponse
    {
        [JsonProperty("PlantDataRange")] public PlantDataRange PlantDataRange { get; set; }
        [JsonProperty("PlantDataPoints")] public List<PlantDataPoint> PlantDataPoints { get; set; }
        [JsonProperty("PlantWaterPoints")] public List<PlantWaterPoint> PlantWaterPoints { get; set; }
    }

    public class PlantDataPoint
    {
        [JsonProperty("Timestamp")] public DateTime Timestamp { get; set; }
        [JsonProperty("LightValue")] public float LightValue { get; set; }
        [JsonProperty("TemperatureValue")] public float TemperatureValue { get; set; }
        [JsonProperty("MoistureValue")] public float MoistureValue { get; set; }
    }

    public class PlantWaterPoint
    {
        [JsonProperty("Timestamp")] public DateTime Timestamp { get; set; }
        [JsonProperty("IsManual")] public bool IsManual { get; set; }
    }

    public enum PlantDataRange
    {
        Latest,
        LastHour,
        Last24Hours,
    }
}