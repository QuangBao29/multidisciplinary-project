using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ARController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name = null;
    [SerializeField]
    private TextMeshProUGUI _light = null;
    [SerializeField]
    private TextMeshProUGUI _humid = null;
    [SerializeField]
    private TextMeshProUGUI _temp = null;

    private int _id;
    public void OnClickShowPanelInfo()
    {
        SceneManager.Instance.ChangeScene(Define.SceneName.Main.ToString(), new SceneManager.Param { viewName = Define.ViewName.PlantInfor, Id = _id });
    }
    public void OnClickWaterNowBtn()
    {
        Debug.LogError("water");
        StartCoroutine(OnWaterNow());
    }
    private IEnumerator OnWaterNow()
    {
        yield return ResourceManager.Instance.RequestWaterNow(_id);
    }
    public void OnSetPlantInfo(int id, string name, string light, string humid, string temp)
    {
        _id = id;
        _name.text = name;
        _light.text = light;
        _humid.text = humid;
        _temp.text = temp;
    }
}
