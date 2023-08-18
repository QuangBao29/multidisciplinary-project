using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlantListItemView : MonoBehaviour
{
    public TextMeshProUGUI _nameText = null;
    [SerializeField]
    private Image _img = null;
    [Space]
    public PlantInformationStamp LightStamp = null;
    public PlantInformationStamp TempStamp = null;
    public PlantInformationStamp MoistureStamp = null;
    [Space]
    [SerializeField]
    private UIViewManager _uiViewManager = null;

    private int _plantID;

    public int PlantID
    {
        get => _plantID;
        set => _plantID = value;
    }

    public void SetPlantData(int id, string name, string imageID, double light, double temp, double moisture)
    {
        //PlantManager.Instance.PlantDataControllersByName[name].LightValueChanged += LightStamp.OnSetData;
        //PlantManager.Instance.PlantDataControllersByName[name].TemperatureValueChanged += TempStamp.OnSetData;
        //PlantManager.Instance.PlantDataControllersByName[name].MoistureValueChanged += MoistureStamp.OnSetData;
        _plantID = id;
        _nameText.text = name;
        _img.sprite = ResourceManager.Instance.GetImportImage(imageID);
        LightStamp.OnSetData(light.ToString());
        TempStamp.OnSetData(temp.ToString());
        MoistureStamp.OnSetData(moisture.ToString());
    }
    public void OnInstantiateData(int id, string name, string imageID)
    {
        _plantID = id;
        _nameText.text = name;
        _img.sprite = ResourceManager.Instance.GetImportImage(imageID);
    }
    public void OnSetName(string name)
    {
        _nameText.text = name;
    }
    
    public void OnClickShowPlantInfo()
    {
        PlantManager.Instance.CurrentPlantItem = this;
        _uiViewManager.OnShowPlantInfoPanel();
    }
}