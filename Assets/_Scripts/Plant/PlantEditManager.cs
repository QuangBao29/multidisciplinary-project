using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlantEditManager : MonoBehaviour
{
    [Header("Plant Name")]
    [SerializeField]
    private TMP_InputField _name = null;
    [SerializeField]
    private Image _imgInput = null;
    [Header("Change Waterring Rules")]
    [SerializeField]
    private GameObject _panelChangeRules = null;
    [SerializeField]
    private TMP_InputField _newRules = null;
    [Header("Remove")]
    [SerializeField]
    private GameObject _panelRemove = null;
    [Space]
    [SerializeField]
    private Image _imgPlant = null;
    [Space]
    [SerializeField]
    private UIViewManager _uiViewManager = null;
    [SerializeField]
    private PlantListView _plantListView = null;
    [Space]
    [SerializeField]
    private ARTrackedImageManager _ARManager = null;
    [SerializeField]
    private XRReferenceImageLibrary _lib = null;


    private Texture2D _texture = null;

    private Define.RulesType type;
    public void Init()
    {
        _imgInput.enabled = false;
        _name.interactable = false;
    }
    public void OnClickRenameBtn()
    {
        _imgInput.enabled = true;
        _name.interactable = true;
    }
    public void OnUnActiveInputName()
    {
        _name.interactable = false;
        _imgInput.enabled = false;
        StartCoroutine(OnRequestChangeName());
    }
    private IEnumerator OnRequestChangeName()
    {
        _uiViewManager.OnShowWaitingScene(true);
        var curID = PlantManager.Instance.CurrentPlantItem.PlantID;
        var curMetric = PlantManager.Instance.DctPlantData[curID].WateringRuleMetrics;
        var curRepeat = PlantManager.Instance.DctPlantData[curID].WateringRuleRepeats;
        yield return ResourceManager.Instance.RequestChangeWaterRules(curID, curRepeat, curMetric, _name.text);
        if (!ResourceManager.Instance.CanChangeRules)
        {
            _uiViewManager.OnSetNotiPanel(true, "Cannot change name");
            yield return new WaitForSeconds(2);
            _uiViewManager.OnShowWaitingScene(false);
            _uiViewManager.OnSetNotiPanel(false, "");
        }
        else
        {
            _uiViewManager.OnShowWaitingScene(false);
            _uiViewManager.OnSetNotiPanel(true, "Name changed.");
            yield return new WaitForSeconds(1);
            _uiViewManager.OnSetNotiPanel(false, "");
            _plantListView.DctPlantItems[curID].OnSetName(_name.text);
            PlantManager.Instance.DctPlantData[curID].PlantName = _name.text;
        }
    }
    public void OnClickSaveRecog()
    {
        var myLib = _ARManager.CreateRuntimeLibrary(_lib);
        var myTexture = myLib[0].texture;
        SaveImage(myTexture);
    }
    public void OnClickRetakeBtn()
    {
        //access to gallery and select picture
        PickImage();
    }

    public void OnClickChangeRulesRepeatBtn()
    {
        _panelChangeRules.SetActive(true);
        type = Define.RulesType.Repeat;
        var curID = PlantManager.Instance.CurrentPlantItem.PlantID;
        _newRules.text = PlantManager.Instance.DctPlantData[curID].WateringRuleRepeats;
    }

    public void OnClickChangeRulesMetricBtn()
    {
        _panelChangeRules.SetActive(true);
        type = Define.RulesType.Metric;
        var curID = PlantManager.Instance.CurrentPlantItem.PlantID;
        _newRules.text = PlantManager.Instance.DctPlantData[curID].WateringRuleMetrics;
    }

    public void OnClickConfirmChangeRulesBtn()
    {
        StartCoroutine(OnClickSendRequestChangeRules());
    }
    public void OnClickCancelChangeRulesBtn()
    {
        _panelChangeRules.SetActive(false);
    }

    private IEnumerator OnClickSendRequestChangeRules()
    {
        int curID = PlantManager.Instance.CurrentPlantItem.PlantID;
        Debug.LogError("cur Id: " + curID);
        _uiViewManager.OnShowWaitingScene(true);
        if (type == Define.RulesType.Repeat)
        {
            var curMetric = PlantManager.Instance.DctPlantData[curID].WateringRuleMetrics;
            yield return ResourceManager.Instance.RequestChangeWaterRules(curID, _newRules.text, curMetric,
                PlantManager.Instance.DctPlantData[curID].PlantName);
        }
        else
        {
            var curRepeat = PlantManager.Instance.DctPlantData[curID].WateringRuleRepeats;
            yield return ResourceManager.Instance.RequestChangeWaterRules(curID, curRepeat, _newRules.text,
            PlantManager.Instance.DctPlantData[curID].PlantName);
        }
        if (!ResourceManager.Instance.CanChangeRules)
        {
            _uiViewManager.OnSetNotiPanel(true, "Cannot change rules.");
            yield return new WaitForSeconds(2);
            _uiViewManager.OnShowWaitingScene(false);
            _uiViewManager.OnSetNotiPanel(false, "");
            _panelChangeRules.SetActive(false);
        }
        else
        {
            _uiViewManager.OnShowWaitingScene(false);
            _panelChangeRules.SetActive(false);
            _uiViewManager.OnSetNotiPanel(true, "Rules changed.");
            yield return new WaitForSeconds(1);
            _uiViewManager.OnSetNotiPanel(false, "");
        }
    }

    public void OnClickRemoveBtn()
    {
        _panelRemove.SetActive(true);
    }

    public void OnClickConfirmRemoveBtn()
    {
        StartCoroutine(OnClickSendRequestRemovePlant(PlantManager.Instance.CurrentPlantItem.PlantID));
    }

    private IEnumerator OnClickSendRequestRemovePlant(int id)
    {
        _uiViewManager.OnShowWaitingScene(true);
        yield return ResourceManager.Instance.RequestDeletePlant(id);
        if (!ResourceManager.Instance.CanRemove)
        {
            _uiViewManager.OnSetNotiPanel(true, "Cannot remove plant.");
            yield return new WaitForSeconds(2);
            _uiViewManager.OnShowWaitingScene(false);
            _panelRemove.SetActive(false);
            _uiViewManager.OnSetNotiPanel(false, "");
        }
        else
        {
            Destroy(_plantListView.DctPlantItems[id].gameObject);
            Debug.LogError("removed: " + PlantManager.Instance.DctPlantData.Remove(id));
            Debug.LogError("removed: " + _plantListView.DctPlantItems.Remove(id));
            _panelRemove.SetActive(false);
            _uiViewManager.OnSetNotiPanel(true, "Plant removed.");
            Debug.LogError("check 1");
            yield return new WaitForSeconds(1);
            _uiViewManager.OnClickShowViewListPlant();
            Debug.LogError("check 2");
            _uiViewManager.OnSetNotiPanel(false, "");
            _uiViewManager.OnShowWaitingScene(false);
        }
    }

    public void OnClickCancelRemoveBtn()
    {
        _panelRemove.SetActive(false);
    }

    public void OnSetData(string name)
    {
        _name.text = name;
    }

    public void PickImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                _texture = texture;
                _imgPlant.sprite = Utility.ConvertToSprite(texture);
            }
        });

        Debug.Log("Permission result: " + permission);
    }
    public void SaveImage(Texture2D texture)
    {
        //Debug.LogError("save image");
        var curID = PlantManager.Instance.CurrentPlantItem.PlantID;
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(texture, "PlantGallery", curID + ".png", (success, path) => Debug.Log("Media save result: " + success + " " + path));

        Debug.LogError("Permission result: " + permission);
    }
    
}
