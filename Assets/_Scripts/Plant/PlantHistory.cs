using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Visuals;
using Random = UnityEngine.Random;

public class PlantHistory : Singleton<PlantHistory>
{
    public Action<Define.HistoryType> HistoryTypeChanged;

    public Define.HistoryType HistoryType;

    [SerializeField] private Button hourlyButton;
    [SerializeField] private Button dailyButton;
    [SerializeField] private Image background;
    [SerializeField] private Image icon;

    [SerializeField] private List<BarUnit> BarUnits;
    [SerializeField] private Sprite lightSprite;
    [SerializeField] private Sprite temperatureSprite;
    [SerializeField] private Sprite humiditySprite;
    [Space]
    [SerializeField]
    private UIViewManager _uiViewManager = null;
    public void Init()
    {
        foreach (var barUnit in BarUnits)
        {
            barUnit.Init();
            barUnit.SetPercentage(Random.value);
            HistoryTypeChanged += barUnit.HistoryChangedHandler;
        }

        HistoryTypeChanged += HistoryTypeChangedHandler;

        HistoryTypeChanged?.Invoke(HistoryType);
    }

    private void OnDestroy()
    {
        foreach (var barUnit in BarUnits)
        {
            HistoryTypeChanged -= barUnit.HistoryChangedHandler;
        }
    }
    public void OnChangeHistoryType(Define.HistoryType type)
    {
        HistoryType = type;
        HistoryTypeChanged?.Invoke(HistoryType);
    }
    private void HistoryTypeChangedHandler(Define.HistoryType historyType)
    {
        //Debug.LogError("chay cai nay");
        switch (historyType)
        {
            case Define.HistoryType.Light:
                background.color = VisualManager.Instance.GetColor(ColorType.LightSecondary);
                icon.sprite = lightSprite;
                break;
            case Define.HistoryType.Temperature:
                background.color = VisualManager.Instance.GetColor(ColorType.TemperatureSecondary);
                icon.sprite = temperatureSprite;
                break;
            case Define.HistoryType.Humidity:
                background.color = VisualManager.Instance.GetColor(ColorType.HumiditySecondary);
                icon.sprite = humiditySprite;
                break;
        }
    }
    public void OnClickBackBtn()
    {
        //back to view plant info, truyen them param la data cua cay hien tai
        _uiViewManager.OnClickShowViewPlantInfor();
    }
}