using System;
using System.Collections;
using System.Collections.Generic;
using Communications.Requests;
using Communications.Responses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class ResourceManager : PersistentSingleton<ResourceManager>
{
    // @formatter:off
    
    //[Header("Data")]
    //public PlantDataController PlantDataController;

    public string bearerKey = "";
    [HideInInspector]
    public bool CanAdd = false;
    [HideInInspector]
    public bool CanLogin = false;
    [HideInInspector]
    public bool CanRemove = false;
    [HideInInspector]
    public bool CanChangeRules = false;
    [HideInInspector]
    public bool CanWater = false;
    [HideInInspector]
    public bool CanRegister = false;
    public IEnumerator RequestGetAllDataPlants()
    {
        yield return RequestCreator.SendRequest<GetPlantResponse>(
            endpoint: "dadn.azurewebsites.net/plantmanagement/get",
            requestType: RequestCreator.Type.GET,
            bearerKey: bearerKey,
            callback: (success, response) =>
            {
                if (success)
                {
                    PlantManager.Instance.DctPlantData.Clear();
                    foreach (var plantData in response.PlantInformations)
                    {
                        //Debug.LogError(string.Format("recog code: {0}", plantData.RecognizerCode));
                        if (!PlantManager.Instance.DctPlantData.ContainsKey(plantData.Id))
                        {
                            PlantDataController dataController = new PlantDataController();
                            dataController.Init(plantData.Id, plantData.Name, plantData.CreatedDate, plantData.RecognizerCode, 
                                plantData.WateringRuleRepeats, plantData.WateringRuleMetrics);
                            PlantManager.Instance.DctPlantData.Add(plantData.Id, dataController);
                        }
                    }
                }
                else
                {
                    Debug.LogError("failed to get all plants data");
                }
            });
    }
    public IEnumerator RequestLogin(string username, string password)
    {
        yield return RequestCreator.SendRequest<AuthenticationResponse>(
            endpoint: "dadn.azurewebsites.net/authentication/login",
            requestType: RequestCreator.Type.POST,
            bearerKey: "",
            objectToSend: new AuthenticationRequest() { Username = username, Password = password },
            callback: (success, response) =>
            {
                if (success)
                {
                    bearerKey = response.Token;
                    PlayerPrefs.SetString(Define.BearerKey, bearerKey);
                    CanLogin = true;
                    Debug.Log("Login successfully with bearer key: " + bearerKey);
                    SceneManager.Instance.ChangeScene(Define.SceneName.Main.ToString(), null);
                }
                else
                {
                    CanLogin = false;
                    Debug.Log("Login failed.");
                }
            });
    }

    public IEnumerator RequestRegister(string username, string password)
    {
        yield return RequestCreator.SendRequest<AuthenticationResponse>(
            endpoint: "dadn.azurewebsites.net/authentication/register",
            requestType: RequestCreator.Type.POST,
            bearerKey: "",
            objectToSend: new AuthenticationRequest() { Username = username, Password = password },
            callback: (success, response) =>
            {
                if (success)
                {
                    bearerKey = response.Token;
                    PlayerPrefs.SetString(Define.BearerKey, bearerKey);
                    CanRegister = true;
                    Debug.Log("Login successfully with bearer key: " + bearerKey);
                    SceneManager.Instance.ChangeScene(Define.SceneName.Main.ToString(), null);
                }
                else
                {
                    CanRegister = false;
                    Debug.Log("Login failed.");
                }
            });
    }

    public IEnumerator RequestAddNewPlant(string name)
    {
        yield return RequestCreator.SendRequest(
            endpoint: "dadn.azurewebsites.net/plantmanagement/add",
            requestType: RequestCreator.Type.POST,
            bearerKey: bearerKey,
            objectToSend: new AddPlantRequest() { Name = name },
            callback: success =>
            {
                if (success)
                {
                    CanAdd = true;
                    Debug.Log("Added a new plant.");
                    StartCoroutine(RequestGetAllDataPlants());
                }
                else
                {
                    Debug.Log("Adding plant failed.");
                    CanAdd = false;
                }
            });
    }
    public IEnumerator RequestDeletePlant(int id)
    {
        yield return RequestCreator.SendRequest(
            endpoint: "dadn.azurewebsites.net/plantmanagement/" + id + "/remove",
            requestType: RequestCreator.Type.GET,
            bearerKey: bearerKey,
            callback: success =>
            {
                if (success)
                {
                    Debug.Log("deleted a plant with id: " + id);
                    CanRemove = true;

                }
                else
                {
                    CanRemove = false;
                    Debug.Log("delete failed.");
                }
            });
    }
    public IEnumerator RequestGetLatestData(int id)
    {
        yield return RequestCreator.SendRequest<PlantDataResponse>(
            endpoint: "dadn.azurewebsites.net/plantdata/" + id + "/latest",
            requestType: RequestCreator.Type.GET,
            bearerKey: bearerKey,
            callback: (success, response) =>
            {
                if (success)
                {
                    foreach (var point in response.PlantDataPoints)
                    {
                        Debug.LogError(point.Timestamp + ": " + point.LightValue);
                        if (!PlantManager.Instance.DctPlantData.ContainsKey(id))
                        {
                            Debug.LogError("loi ko co id");
                        }
                        else
                        {
                            Debug.LogError("go with id: " + id);
                            PlantManager.Instance.DctPlantData[id].Timestamp = point.Timestamp;
                            PlantManager.Instance.DctPlantData[id].LightValue = Math.Round(point.LightValue, 1);
                            PlantManager.Instance.DctPlantData[id].TemperatureValue = Math.Round(point.TemperatureValue, 1);
                            PlantManager.Instance.DctPlantData[id].MoistureValue = Math.Round(point.MoistureValue, 1);

                        }
                    }
                }
                else
                {
                    Debug.Log("Failed to get plant's data.");
                }
            });
    }

    public IEnumerator RequestChangeWaterRules(int id, string rulesRepeat, string rulesMetric, string name)
    {
        yield return RequestCreator.SendRequest(
            endpoint: "dadn.azurewebsites.net/plantmanagement/" + id + "/edit",
            requestType: RequestCreator.Type.POST,
            bearerKey: bearerKey,
            objectToSend: new EditPlantRequest() { NewName = name, NewWateringRuleRepeats = rulesRepeat, NewWateringRuleMetrics = rulesMetric},
            callback: success =>
            {
                if (success)
                {
                    Debug.LogError("chang water rules: " + id + " " + rulesRepeat + " " + rulesMetric);
                    CanChangeRules = true;
                }
                else
                {
                    Debug.Log("Failed to change water rules.");
                    CanChangeRules = false;
                }
            }
            );
    }
    public IEnumerator RequestWaterNow(int id)
    {
        yield return RequestCreator.SendRequest(
            endpoint: "dadn.azurewebsites.net/plantmanagement/" + id + "/water",
            requestType: RequestCreator.Type.GET,
            bearerKey: bearerKey,
            callback: success =>
            {
                if (success)
                {
                    Debug.LogError("water now " + id);
                    CanWater = true;
                }
                else
                {
                    CanWater = false;
                    Debug.Log("Failed to water now.");
                }
            }
            );
    }

    public Sprite GetImportImage(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        var sprite = Resources.Load<Sprite>("Sprites/" + id);
        var ss = Resources.Load<Texture2D>(Application.persistentDataPath + "/" + id);
        //return Utility.ConvertToSprite(ss);
        return sprite;
    }

    public List<string> GetAllGalleryImagePaths()
    {
        List<string> results = new List<string>();
        HashSet<string> allowedExtesions = new HashSet<string>() { ".png", ".jpg", ".jpeg" };

        try
        {
            AndroidJavaClass mediaClass = new AndroidJavaClass("android.provider.MediaStore$Images$Media");

            // Set the tags for the data we want about each image.  This should really be done by calling; 
            //string dataTag = mediaClass.GetStatic<string>("DATA");
            // but I couldn't get that to work...

            const string dataTag = "_data";

            string[] projection = new string[] { dataTag };
            AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = player.GetStatic<AndroidJavaObject>("currentActivity");

            string[] urisToSearch = new string[] { "EXTERNAL_CONTENT_URI", "INTERNAL_CONTENT_URI" };
            foreach (string uriToSearch in urisToSearch)
            {
                AndroidJavaObject externalUri = mediaClass.GetStatic<AndroidJavaObject>(uriToSearch);
                AndroidJavaObject finder = currentActivity.Call<AndroidJavaObject>("managedQuery", externalUri, projection, null, null, null);
                bool foundOne = finder.Call<bool>("moveToFirst");
                while (foundOne)
                {
                    int dataIndex = finder.Call<int>("getColumnIndex", dataTag);
                    string data = finder.Call<string>("getString", dataIndex);
                    if (allowedExtesions.Contains(Path.GetExtension(data).ToLower()))
                    {
                        string path = @"file:///" + data;
                        results.Add(path);
                    }

                    foundOne = finder.Call<bool>("moveToNext");
                }
            }
        }
        catch (System.Exception e)
        {
            // do something with error...
            Debug.LogError("loi lay anh");
        }

        return results;
    }
}