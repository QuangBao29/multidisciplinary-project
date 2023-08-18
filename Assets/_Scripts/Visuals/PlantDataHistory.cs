using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Communications.Responses;
using Shapes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Visuals;


public class PlantDataHistory : MonoBehaviour
{
    public enum Type
    {
        Light,
        Temperature,
        Moisture,
    }

    [SerializeField] private Canvas _canvas;

    [SerializeField] private TMP_Text _title;
    [SerializeField] private Button _lastHourButton;
    [SerializeField] private Button _last24HoursButton;
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _lightSprite;
    [SerializeField] private Sprite _temperatureSprite;
    [SerializeField] private Sprite _moistureSprite;
    [Space]
    [SerializeField] private Polyline _dataLine;
    [SerializeField] private Line _waterLine;
    [SerializeField] private RectTransform _graphPanel;
    [SerializeField] private List<Label> _xLabels;
    [SerializeField] private List<Label> _yLabels;
    [Space]
    [SerializeField]
    private UIViewManager _uiViewManager = null;

    private int _currentPlantId;
    private string _currentPlantName;
    private Type _currentType;
    private PlantDataRange _currentDataRange;
    private List<GameObject> _instantiatedWaterLines = new();
    private Color _currentPrimaryColor;

    private void Start()
    {
        _canvas.worldCamera = Camera.main;
        _lastHourButton.onClick.AddListener(SwitchToLastHour);
        _last24HoursButton.onClick.AddListener(SwitchToLast24Hours);

        Deactivate();
    }

    private void SwitchToLastHour()
    {
        var colorBlock = _lastHourButton.colors;
        colorBlock.normalColor = colorBlock.selectedColor = VisualManager.Instance.InactiveButtonColor;

        _last24HoursButton.colors = colorBlock;
        _last24HoursButton.GetComponentInChildren<TMP_Text>().color = VisualManager.Instance.InactiveTextColor;

        colorBlock.normalColor = colorBlock.selectedColor = _currentPrimaryColor;
        _lastHourButton.colors = colorBlock;
        _lastHourButton.GetComponentInChildren<TMP_Text>().color = VisualManager.Instance.BlackColor;

        Show(PlantDataRange.LastHour);
    }

    private void SwitchToLast24Hours()
    {
        var colorBlock = _last24HoursButton.colors;
        colorBlock.normalColor = colorBlock.selectedColor = VisualManager.Instance.InactiveButtonColor;

        _lastHourButton.colors = colorBlock;
        _lastHourButton.GetComponentInChildren<TMP_Text>().color = VisualManager.Instance.InactiveTextColor;

        colorBlock.normalColor = colorBlock.selectedColor = _currentPrimaryColor;
        _last24HoursButton.colors = colorBlock;
        _last24HoursButton.GetComponentInChildren<TMP_Text>().color = VisualManager.Instance.BlackColor;

        Show(PlantDataRange.Last24Hours);
    }

    public void Activate(int plantId, string plantName, Type type)
    {
        gameObject.SetActive(true);
        
        _currentPlantId = plantId;
        _currentPlantName = plantName;
        _currentType = type;

        switch (_currentType)
        {
            case Type.Light:
                _background.color = VisualManager.Instance.LightSecondaryColor;
                _currentPrimaryColor = VisualManager.Instance.LightPrimaryColor;
                _icon.sprite = _lightSprite;
                break;
            case Type.Temperature:
                _background.color = VisualManager.Instance.TemperatureSecondaryColor;
                _currentPrimaryColor = VisualManager.Instance.TemperaturePrimaryColor;
                _icon.sprite = _temperatureSprite;
                break;
            case Type.Moisture:
                _background.color = VisualManager.Instance.HumiditySecondaryColor;
                _currentPrimaryColor = VisualManager.Instance.HumidityPrimaryColor;
                _icon.sprite = _moistureSprite;
                break;
        }

        SwitchToLastHour();
    }

    public void Deactivate()
    {
        foreach (var waterLine in _instantiatedWaterLines) Destroy(waterLine);
        _instantiatedWaterLines = new();
        gameObject.SetActive(false);
    }

    private void Show(PlantDataRange plantDataRange)
    {
        if (plantDataRange == PlantDataRange.Latest)
        {
            Debug.LogError("You are trying to show a graph with a single data point.");
            return;
        }

        StartCoroutine(SendRequest(_currentPlantId,
            plantDataRange,
            (success, plantDataResponse) =>
            {
                if (!success)
                {
                    Debug.LogError("Failed to request for plant data.");
                    return;
                }

                foreach (var waterLine in _instantiatedWaterLines) Destroy(waterLine);
                _instantiatedWaterLines = new();

                _currentDataRange = plantDataRange;

                string titleText = _currentPlantName + "'s ";
                switch (_currentType)
                {
                    case Type.Light:
                        titleText += "lighting";
                        break;
                    case Type.Temperature:
                        titleText += "temperature";
                        break;
                    case Type.Moisture:
                        titleText += "moisture";
                        break;
                }

                titleText += " history";

                _title.text = titleText;

                float timeRange = plantDataRange == PlantDataRange.LastHour ? 1 : 24;
                var startTimestamp = DateTime.UtcNow.AddHours(-timeRange);

                var plantDataPoints = plantDataResponse.PlantDataPoints;
                List<(DateTime timestamp, float value)> filteredDataPoints = new();
                foreach (var dataPoint in plantDataPoints)
                {
                    if (dataPoint.Timestamp < startTimestamp) continue;
                    switch (_currentType)
                    {
                        case Type.Light:
                            filteredDataPoints.Add((dataPoint.Timestamp, dataPoint.LightValue));
                            break;
                        case Type.Temperature:
                            filteredDataPoints.Add((dataPoint.Timestamp, dataPoint.TemperatureValue));
                            break;
                        case Type.Moisture:
                            filteredDataPoints.Add((dataPoint.Timestamp, dataPoint.MoistureValue));
                            break;
                    }
                }

                var plantWaterPoints = plantDataResponse.PlantWaterPoints;
                List<PlantWaterPoint> filteredWaterPoints = new();
                foreach (var waterPoint in plantWaterPoints)
                {
                    if (waterPoint.Timestamp < startTimestamp) continue;
                    filteredWaterPoints.Add(waterPoint);
                }

                DrawGraphBase(startTimestamp, DateTime.UtcNow);
                DrawDataGraph(filteredDataPoints, startTimestamp, DateTime.UtcNow);
                DrawWaterGraph(filteredWaterPoints, startTimestamp, DateTime.UtcNow);
            }));
    }

    private IEnumerator SendRequest(int plantId, PlantDataRange plantDataRange, Action<bool, PlantDataResponse> callback)
    {
        string type = plantDataRange == PlantDataRange.LastHour ? "lasthour" : "last24hours";
        yield return RequestCreator.SendRequest<PlantDataResponse>(
            endpoint: "dadn.azurewebsites.net/plantdata/" + plantId + "/" + type,
            requestType: RequestCreator.Type.GET,
            bearerKey:
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2ODI1OTc2OTEsImV4cCI6MTk5ODIxNjg5MSwiaWF0IjoxNjgyNTk3NjkxfQ.GwwtpyjNFV8FPN53Fm1ySjocORZYQW1EZ04ydBRTBEY",
            callback: callback);
    }

    private void DrawGraphBase(DateTime startTimestamp, DateTime endTimestamp)
    {
        DateTime currentTimestamp = startTimestamp;
        List<DateTime> rawTimestamps = new List<DateTime>() {currentTimestamp};
        for (int i = 0; i < 7; i++)
        {
            if (_currentDataRange == PlantDataRange.LastHour)
                currentTimestamp = currentTimestamp.AddMinutes(10);
            else
                currentTimestamp = currentTimestamp.AddHours(4);

            rawTimestamps.Add(currentTimestamp);
        }

        rawTimestamps.Add(endTimestamp);

        var normalizedTimestamps = NormalizeData(rawTimestamps, startTimestamp, DateTime.UtcNow);

        for (int i = 0; i < 7; i++)
        {
            float x = Mathf.Lerp(0, _graphPanel.rect.size.x, normalizedTimestamps[i]);
            _xLabels[i].SetX(x);

            if (_currentDataRange == PlantDataRange.LastHour)
                _xLabels[i].SetText(rawTimestamps[i].ToLocalTime().ToString("h:mmtt"));
            else
                _xLabels[i].SetText(rawTimestamps[i].ToLocalTime().ToString("htt"));
        }

        int minValue = 0;
        int maxValue = 100;
        List<float> rawData = new();
        for (int i = 0; i <= 100; i += 20) rawData.Add(i);
        var normalizedData = NormalizeData(rawData, minValue, maxValue);
        for (int i = 0; i < 6; i++)
        {
            float y = Mathf.Lerp(0, _graphPanel.rect.size.y, normalizedData[i]);
            _yLabels[i].SetY(y);

            string text = ((int)rawData[i]).ToString();

            switch (_currentType)
            {
                case Type.Light:
                    text += "lx";
                    break;
                case Type.Temperature:
                    text += "°C";
                    break;
                case Type.Moisture:
                    text += "%";
                    break;
            }

            _yLabels[i].SetText(text);
        }
    }

    private void DrawDataGraph(List<(DateTime timestamp, float value)> dataPoints, DateTime startTimestamp, DateTime endTimestamp)
    {
        List<Vector2> dataLinePoints = new();

        var timestamps = dataPoints.Select(point => point.timestamp).ToList();
        var normalizedTimestamps = NormalizeData(timestamps, startTimestamp, endTimestamp);

        var dataValues = dataPoints.Select(point => point.value).ToList();
        var minDataValue = 0;
        var maxDataValue = 100;
        var normalizedDataValue = NormalizeData(dataValues, minDataValue, maxDataValue);

        for (var i = 0; i < dataPoints.Count; i++)
        {
            float x = Mathf.Lerp(_graphPanel.rect.min.x, _graphPanel.rect.max.x, normalizedTimestamps[i]);
            float y = Mathf.Lerp(_graphPanel.rect.min.y, _graphPanel.rect.max.y, normalizedDataValue[i]);
            dataLinePoints.Add(new Vector2(x, y));
        }

        _dataLine.Color = _currentPrimaryColor;
        _dataLine.SetPoints(dataLinePoints);
    }

    private void DrawWaterGraph(List<PlantWaterPoint> waterPoints, DateTime startTimestamp, DateTime endTimestamp)
    {
        var timestamps = waterPoints.Select(point => point.Timestamp).ToList();
        var normalizedTimestamps = NormalizeData(timestamps, startTimestamp, endTimestamp);

        for (var i = 0; i < waterPoints.Count; i++)
        {
            var line = Instantiate(_waterLine, _waterLine.transform.parent);
            var x = Mathf.Lerp(_graphPanel.rect.min.x, _graphPanel.rect.max.x, normalizedTimestamps[i]);
            line.Start = new Vector2(x, _graphPanel.rect.min.y);
            line.End = new Vector2(x, _graphPanel.rect.max.y);
            if (!waterPoints[i].IsManual) line.Dashed = true;
            line.Color = _currentPrimaryColor;
            _instantiatedWaterLines.Add(line.gameObject);
        }
    }

    private List<float> NormalizeData(IEnumerable<float> rawData, float min, float max)
    {
        return rawData.Select(data => Mathf.InverseLerp(min, max, data)).ToList();
    }

    private List<float> NormalizeData(IEnumerable<DateTime> rawData, DateTime min, DateTime max)
    {
        return rawData.Select(data => (float)((data - min) / (max - min))).ToList();
    }

    public void OnClickBackBtn()
    {
        _uiViewManager.OnClickShowViewPlantInfor();
        Deactivate();
    }
}