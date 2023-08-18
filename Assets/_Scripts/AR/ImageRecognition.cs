using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System.Linq;

public class ImageRecognition : MonoBehaviour
{
    private ARTrackedImageManager _arTrackedImageManager;
    [SerializeField]
    private GameObject _panel = null;
    [SerializeField]
    private ARController _controller = null;
    //[SerializeField]
    //private TextMeshProUGUI _text = null;
    //[SerializeField]
    //private TextMeshProUGUI _text2 = null;
    //[SerializeField]
    //private TextMeshProUGUI _text3 = null;
    //[SerializeField]
    //private TextMeshProUGUI _text4 = null;
    //[SerializeField]
    //private TextMeshProUGUI _text5 = null;

    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }
    private void Start()
    {
        _panel.SetActive(false);
    }
    public void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnShowPanel;
    }
    public void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnShowPanel;
    }

    public void OnShowPanel(ARTrackedImagesChangedEventArgs args)
    {
        _panel.SetActive(true);
        //foreach (var item in _arTrackedImageManager.trackables)
        //{
        // item.referenceImage.texture   
        //}
        //_text.text = str;
        //foreach (var item in args.added)
        //{
        //    _text2.text = "name " + item.referenceImage.name;
        //    _text3.text = "tracking state: " + item.trackingState.ToString();
        //}
        //foreach (var item in args.removed)
        //{
        //    _text5.text = item.referenceImage.name;
        //}
        foreach (ARTrackedImage trackedImg in args.added.Concat(args.updated))
        {
            if (trackedImg.trackingState == TrackingState.Tracking)
            {
                //trackedImg is tracked
                //_text4.text = "tracked name: " + trackedImg.referenceImage.name;
                var id = int.Parse(trackedImg.referenceImage.name);
                var name = PlantManager.Instance.DctPlantData[id].PlantName;
                var light = PlantManager.Instance.DctPlantData[id].LightValue;
                var humid = PlantManager.Instance.DctPlantData[id].MoistureValue;
                var temp = PlantManager.Instance.DctPlantData[id].TemperatureValue;
                _controller.OnSetPlantInfo(id, name, light.ToString(), humid.ToString(), temp.ToString());
            }
        }
    }
}
