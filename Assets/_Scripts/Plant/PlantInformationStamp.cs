using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantInformationStamp : MonoBehaviour
{
    public string DataType;
    [Space]
    [SerializeField] private TMP_Text _informationText;
    
    public void OnSetData(string data)
    {
        _informationText.text = data;
    }
    public string OnGetData()
    {
        return _informationText.text;
    }
    public void OnClickShowHistoryPanel()
    {

    }
}