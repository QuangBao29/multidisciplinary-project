using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantListView : MonoBehaviour
{
    [SerializeField] private PlantListItemView _prefabPlantItem = null;
    [SerializeField] private GameObject _content = null;
    [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup = null;
    [SerializeField] private UIViewManager _uiViewManager = null;

    private Dictionary<int, PlantListItemView> _dctPlantItems = new Dictionary<int, PlantListItemView>();

    public Dictionary<int, PlantListItemView> DctPlantItems
    {
        get => _dctPlantItems;
        set => _dctPlantItems = value;
    }

    public void Init()
    {
        _prefabPlantItem.gameObject.SetActive(false);
        //Debug.LogError("check dct data count: " + PlantManager.Instance.DctPlantData.Count);
        //Debug.LogError("check dct item count: " + DctPlantItems.Count);
        StartCoroutine(OnInit());
        _uiViewManager.OnClickShowViewListPlant();
        _uiViewManager.NewPlantName = "";
        _uiViewManager.OnShowWaitingScene(false);
    }
    private IEnumerator OnInit()
    {
        int id = 0;
        if (SceneManager.Instance.CurrentParam != null)
            if (SceneManager.Instance.CurrentParam.Id != 0)
                id = SceneManager.Instance.CurrentParam.Id;
        foreach (var plant in PlantManager.Instance.DctPlantData)
        {
            //Debug.LogError("plant id: " + plant.Key);
            var item = Utility.InstantiateObject<PlantListItemView>(_prefabPlantItem, _content.transform);
            item.SetPlantData(plant.Key,
                plant.Value.PlantName,
                "Plant Image",
                plant.Value.LightValue,
                plant.Value.TemperatureValue,
                plant.Value.MoistureValue);
            if (!DctPlantItems.ContainsKey(plant.Key))
                DctPlantItems.Add(plant.Key, item);
            else Debug.LogError("trung id");
            if (id != 0 && plant.Key == id)
                PlantManager.Instance.CurrentPlantItem = item;
            item.gameObject.SetActive(true);
        }
        
        _horizontalLayoutGroup.padding.left =
            (int)(1280f / 2f - _prefabPlantItem.GetComponent<RectTransform>().rect.width / 2);
        yield return new WaitForSeconds(2);
    }

    public void OnClickButtonAddNewPlant()
    {
        _uiViewManager.GetUIViewWithViewName(Define.ViewName.NewPlant.ToString()).gameObject.SetActive(true);
        _uiViewManager.OnShowWaitingScene(false);
    }

    public void OnClickCancelAddPlant()
    {
        _uiViewManager.OnClickShowViewListPlant();
        _uiViewManager.NewPlantName = "";
    }

    public void OnClickSpawnPlantItems()
    {
        //gui request tao cay moi va lay data ve, sau do generate data controller
        var newName = Utility.RemoveSpacesFromHeadTail(_uiViewManager.NewPlantName);
        StartCoroutine(OnSpawnItem(_uiViewManager.NewPlantName));
        //for (var i = 1; i < _content.transform.childCount; i++)
        //{
        //    Destroy(_content.transform.GetChild(i).gameObject);
        //}
        foreach (var item in DctPlantItems)
        {
            Destroy(item.Value.gameObject);
        }
        DctPlantItems.Clear();

        _uiViewManager.OnClickShowViewListPlant();
        _uiViewManager.NewPlantName = "";
    }

    public IEnumerator OnSpawnItem(string name)
    {
        _uiViewManager.OnShowWaitingScene(true);
        yield return ResourceManager.Instance.RequestAddNewPlant(name);
        //xong cai nay se co dctPlantData moi
        if (!ResourceManager.Instance.CanAdd)
        {
            _uiViewManager.OnSetNotiPanel(true, "Cannot Add New Plant.");
            yield return new WaitForSeconds(2);
            _uiViewManager.OnSetNotiPanel(false, "");
        }
        else
        {
            _uiViewManager.OnSetNotiPanel(true, "New Plant Added.");
            yield return new WaitForSeconds(1);
            _uiViewManager.OnSetNotiPanel(false, "");
        }

        foreach (var item in PlantManager.Instance.DctPlantData)
        {
            yield return ResourceManager.Instance.RequestGetLatestData(item.Key);
        }

        Init();
    }
}