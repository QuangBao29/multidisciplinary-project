using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIViewManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _topMenu = null;
    [SerializeField]
    private TMP_InputField _inputName = null;
    [Space]
    [Header("Loading related")]
    [SerializeField]
    private GameObject _waitingScene = null;
    [SerializeField]
    private GameObject _notiPanel = null;
    [SerializeField]
    private TextMeshProUGUI _noti = null;
    [Space]
    [Header("View")]
    [SerializeField]
    private PlantListView _plantListView = null;
    //[SerializeField]
    //private PlantHistory _plantHistory = null;
    [SerializeField]
    private PlantEditManager _plantEditManager = null;
    [SerializeField]
    private PlantInfoManager _plantInfoManager = null;

    public Dictionary<string, UIView> DictUIView = new Dictionary<string, UIView>();

    private List<string> _lstShownView;
    public List<string> LstShownView
    {
        get
        {
            if (_lstShownView == null) _lstShownView = new List<string>();
            return _lstShownView;
        }
        set => _lstShownView = value;
    }
    public string NewPlantName
    {
        get => _inputName.text;
        set => _inputName.text = value;
    }
    private void Awake()
    {
        Initialize();
    }
    private IEnumerator Start()
    {
        OnShowWaitingScene(true);
        _notiPanel.SetActive(false);
        yield return ResourceManager.Instance.RequestGetAllDataPlants();
        //yield return ResourceManager.Instance.RequestGetLatestData();
        foreach (var item in PlantManager.Instance.DctPlantData)
        {
            yield return ResourceManager.Instance.RequestGetLatestData(item.Key);
        }
        _plantEditManager.Init();
        _plantListView.Init();
        OnClickShowViewListPlant();
        var param = SceneManager.Instance.CurrentParam;
        if (param != null)
        {
            switch (param.viewName)
            {
                case Define.ViewName.PlantInfor:
                    //foreach (var item in _plantListView.DctPlantItems)
                    //{
                    //    if (item.Key == param.Id)
                    //    {
                    //        PlantManager.Instance.CurrentPlantItem = item.Value;
                    //        break;
                    //    }
                    //}
                    OnShowPlantInfoPanel();
                    break;

                case Define.ViewName.Account:
                    OnClickShowViewAccount();
                    break;

                default:
                    break;
            }
        }
        OnShowWaitingScene(false);
    }
    private void Initialize()
    {
        UIView[] viewController = GetComponentsInChildren<UIView>(true);
        foreach (var view in viewController)
        {
            view.Initialize(this);
            if (!DictUIView.ContainsKey(view.ViewName))
                DictUIView.Add(view.ViewName, view);
            else Debug.LogError("trung");
        }
        //Debug.LogError("count of dct: " + DictUIView.Count);
    }
    public void OnHideAllViews()
    {
        foreach (var item in DictUIView)
        {
            item.Value.gameObject.SetActive(false);
        }
    }
    public void OnShowWaitingScene(bool set)
    {
        _waitingScene.SetActive(set);
    }
    public void OnSetNotiPanel(bool status, string noti)
    {
        _noti.text = noti;
        _notiPanel.SetActive(status);
    }
    public void OnClickShowViewListPlant()
    {
        foreach (var pair in DictUIView)
        {
            if (pair.Key == Define.ViewName.ListPlant.ToString())
                pair.Value.gameObject.SetActive(true);
            else pair.Value.gameObject.SetActive(false);
        }
        _topMenu.SetActive(true);
        LstShownView.Add(Define.ViewName.ListPlant.ToString());
    }
    public void OnClickShowViewARMode()
    {
        SceneManager.Instance.ChangeScene(Define.SceneName.AR.ToString(), null);
    }
    public void OnClickShowViewAccount()
    {
        foreach (var pair in DictUIView)
        {
            if (pair.Key == Define.ViewName.Account.ToString())
                pair.Value.gameObject.SetActive(true);
            else pair.Value.gameObject.SetActive(false);
        }
        LstShownView.Add(Define.ViewName.Account.ToString());
    }
    public void OnClickShowViewHistory()
    {
        foreach (var pair in DictUIView)
        {
            if (pair.Key == Define.ViewName.History.ToString())
            {
                pair.Value.gameObject.SetActive(true);
            }
            else pair.Value.gameObject.SetActive(false);
        }
        //LstShownView.Add(Define.ViewName.History.ToString());
    }
    public void OnClickShowViewEdit()
    {
        foreach (var pair in DictUIView)
        {
            if (pair.Key == Define.ViewName.EditPlant.ToString())
            {
                pair.Value.gameObject.SetActive(true);
            }
            else pair.Value.gameObject.SetActive(false);
        }

        var curID = PlantManager.Instance.CurrentPlantItem.PlantID;
        _plantEditManager.OnSetData(PlantManager.Instance.DctPlantData[curID].PlantName);
        //LstShownView.Add(Define.ViewName.History.ToString());
    }
    
    public UIView GetUIViewWithViewName(string name)
    {
        foreach (var pair in DictUIView)
        {
            if (pair.Key == name)
                return pair.Value;
        }
        return null;
    }

    #region Plant Infor
    public void OnShowPlantInfoPanel()
    {
        OnClickShowViewPlantInfor();
        _topMenu.SetActive(false);
    }
    public void OnClickShowViewPlantInfor()
    {
        foreach (var pair in DictUIView)
        {
            if (pair.Key == Define.ViewName.PlantInfor.ToString())
                pair.Value.gameObject.SetActive(true);
            else pair.Value.gameObject.SetActive(false);
        }
        LstShownView.Add(Define.ViewName.PlantInfor.ToString());

        //goi plant infor manager lay data plant
        var curID = PlantManager.Instance.CurrentPlantItem.PlantID;
        var name = PlantManager.Instance.DctPlantData[curID].PlantName;
        var light = PlantManager.Instance.DctPlantData[curID].LightValue.ToString();
        var humid = PlantManager.Instance.DctPlantData[curID].MoistureValue.ToString();
        var temp = PlantManager.Instance.DctPlantData[curID].TemperatureValue.ToString();
        _plantInfoManager.OnSetPlantData(name, light, humid, temp);
    }
    public void OnClickBackToListPlant()
    {
        OnClickShowViewListPlant();
    }
    public void OnClickBtnInforStamp()
    {
        //lay infor cua cay dc target
        //lay = plantlistviewitem roi reference sang inforStamp
        //show panel history
    }
    public void HideAllViews()
    {
        foreach (var pair in DictUIView)
        {
            pair.Value.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Example Code
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (Input.mousePosition.x < Screen.width / 3)
    //        {
    //            Debug.LogError("run");
    //            // Take a screenshot and save it to Gallery/Photos
    //            StartCoroutine(TakeScreenshotAndSave());
    //            //PickImage(1024);
    //        }
    //        else
    //        {
    //            // Don't attempt to pick media from Gallery/Photos if
    //            // another media pick operation is already in progress
    //            if (NativeGallery.IsMediaPickerBusy())
    //                return;

    //            if (Input.mousePosition.x < Screen.width * 2 / 3)
    //            {
    //                // Pick a PNG image from Gallery/Photos
    //                // If the selected image's width and/or height is greater than 512px, down-scale the image
    //                //PickImage(512);
    //                PickImage(-1);
    //            }
    //            else
    //            {
    //                // Pick a video from Gallery/Photos
    //                PickImage(-1);
    //            }
    //        }
    //    }
    //}

    public IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "GalleryTest", "Image.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));

        Debug.Log("Permission result: " + permission);

        // To avoid memory leaks
        Destroy(ss);
    }

    public void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // Assign texture to a temporary quad and destroy it after 5 seconds
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                Material material = quad.GetComponent<Renderer>().material;
                if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                material.mainTexture = texture;

                //Destroy(quad, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                //Destroy(texture, 5f);
            }
        });

        Debug.Log("Permission result: " + permission);
    }

    public void PickVideo()
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Video path: " + path);
            if (path != null)
            {
                // Play the selected video
                Handheld.PlayFullScreenMovie("file://" + path);
            }
        }, "Select a video");

        Debug.Log("Permission result: " + permission);
    }

    // Example code doesn't use this function but it is here for reference
    public void PickImageOrVideo()
    {
        if (NativeGallery.CanSelectMultipleMediaTypesFromGallery())
        {
            NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
            {
                Debug.Log("Media path: " + path);
                if (path != null)
                {
                    // Determine if user has picked an image, video or neither of these
                    switch (NativeGallery.GetMediaTypeOfFile(path))
                    {
                        case NativeGallery.MediaType.Image: Debug.Log("Picked image"); break;
                        case NativeGallery.MediaType.Video: Debug.Log("Picked video"); break;
                        default: Debug.Log("Probably picked something else"); break;
                    }
                }
            }, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select an image or video");

            Debug.Log("Permission result: " + permission);
        }
    }

    public void OnGenerateButton()
    {
        foreach (var item in PlantManager.Instance.DctPlantData)
        {
            RecognizerParser.Save(item.Value.RecognizerCode, item.Key.ToString());
        }
    }
    #endregion
}
