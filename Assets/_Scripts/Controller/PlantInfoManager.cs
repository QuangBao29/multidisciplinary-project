using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlantInfoManager : MonoBehaviour
{
    [SerializeField] private UIViewManager _uiViewManager = null;
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private TextMeshProUGUI _light = null;
    [SerializeField] private TextMeshProUGUI _humid = null;
    [SerializeField] private TextMeshProUGUI _temp = null;

    [SerializeField] private PlantDataHistory _plantDataHistory;

    public void OnClickShowHistoryTemp()
    {
        _plantDataHistory.Activate(PlantManager.Instance.CurrentPlantItem.PlantID,
            PlantManager.Instance.CurrentPlantItem._nameText.text,
            PlantDataHistory.Type.Temperature);
        _uiViewManager.OnHideAllViews();
    }

    public void OnClickShowHistoryLight()
    {
        _plantDataHistory.Activate(PlantManager.Instance.CurrentPlantItem.PlantID,
            PlantManager.Instance.CurrentPlantItem._nameText.text,
            PlantDataHistory.Type.Light);
        _uiViewManager.OnHideAllViews();
    }

    public void OnClickShowHistoryHumid()
    {
        _plantDataHistory.Activate(PlantManager.Instance.CurrentPlantItem.PlantID,
            PlantManager.Instance.CurrentPlantItem._nameText.text,
            PlantDataHistory.Type.Moisture);
        _uiViewManager.OnHideAllViews();
    }

    public void OnClickBackBtn()
    {
        _uiViewManager.OnClickBackToListPlant();
        _plantDataHistory.Deactivate();
    }

    public void OnClickWaterNow()
    {
        StartCoroutine(OnWaterNow());
    }

    private IEnumerator OnWaterNow()
    {
        _uiViewManager.OnShowWaitingScene(true);
        yield return ResourceManager.Instance.RequestWaterNow(PlantManager.Instance.CurrentPlantItem.PlantID);
        if (!ResourceManager.Instance.CanWater)
        {
            _uiViewManager.OnSetNotiPanel(true, "Cannot Water Now");
            yield return new WaitForSeconds(2);
            _uiViewManager.OnShowWaitingScene(false);
            _uiViewManager.OnSetNotiPanel(false, "");
        }
        else

        {
            _uiViewManager.OnShowWaitingScene(false);
            _uiViewManager.OnSetNotiPanel(true, "Watered");
            yield return new WaitForSeconds(1);
            _uiViewManager.OnSetNotiPanel(false, "");
        }
    }

    public void OnClickEdit()
    {
        _uiViewManager.OnClickShowViewEdit();
    }

    public void OnSetPlantData(string name, string light, string humid, string temp)
    {
        _name.text = name;
        _temp.text = temp + "<size=60px>°C";
        _light.text = light + "<size=60px>lx";
        _humid.text = humid + "<size=60px>%";
    }
}