using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Visuals;

public class BarUnit : MonoBehaviour
{
    [SerializeField] private RectTransform frontImageRectTransform;
    [SerializeField] private RectTransform backImageRectTransform;
    private Image frontImage;
    private Image backImage;

    [SerializeField] private TMP_Text _valueText;
    
    private float _minData;
    private float _maxData;
    
    public void Init()
    {
        frontImage = frontImageRectTransform.GetComponent<Image>();
        backImage = backImageRectTransform.GetComponent<Image>();
    }

    public void SetPercentage(float percentage)
    {
        frontImageRectTransform.sizeDelta = new Vector2(frontImageRectTransform.sizeDelta.x,
            backImageRectTransform.rect.height * percentage);
        _valueText.text = percentage.ToString("F2");
    }

    public void SetValue(float value)
    {
        
    }

    public void SetNewLimits(float min, float max)
    {
        _minData = min;
        _maxData = max;
    }

    public void HistoryChangedHandler(Define.HistoryType historyType)
    {
        switch (historyType)
        {
            case Define.HistoryType.Light:
                frontImage.color = VisualManager.Instance.GetColor(ColorType.LightPrimary);
                backImage.color = VisualManager.Instance.GetColor(ColorType.LightSecondary);
                break;
            case Define.HistoryType.Temperature:
                frontImage.color = VisualManager.Instance.GetColor(ColorType.TemperaturePrimary);
                backImage.color = VisualManager.Instance.GetColor(ColorType.TemperatureSecondary);
                break;
            case Define.HistoryType.Humidity:
                frontImage.color = VisualManager.Instance.GetColor(ColorType.HumidityPrimary);
                backImage.color = VisualManager.Instance.GetColor(ColorType.HumiditySecondary);
                break;
        }
    }
}