using System;
using System.Collections;
using Communications.Requests;
using Communications.Responses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class TestApi : MonoBehaviour
{
    public string bearerKey = "";
    private IEnumerator Start()
    {
        

        // Vi du 1 Login
        yield return RequestCreator.SendRequest<AuthenticationResponse>(
            endpoint: "dadn.azurewebsites.net/authentication/login",
            requestType: RequestCreator.Type.POST,
            bearerKey: "",
            objectToSend: new AuthenticationRequest() { Username = "Chuan", Password = "chuan123" },
            callback: (success, response) =>
            {
                if (success)
                {
                    bearerKey = response.Token;
                    Debug.Log("Login successfully with bearer key: " + bearerKey);
                }
                else
                {
                    Debug.Log("Login failed.");
                }
            });

        // Vd new plant
        yield return RequestCreator.SendRequest(endpoint: "dadn.azurewebsites.net/plantmanagement/add",
            requestType: RequestCreator.Type.POST,
            bearerKey: bearerKey,
            objectToSend: new AddPlantRequest() { Name = "Cây cute" },
            callback: success =>
            {
                if (success) Debug.Log("Added a new plant.");
                else Debug.Log("Adding plant failed.");
            });

        // Vi du 3 get data
        yield return RequestCreator.SendRequest<PlantDataResponse>(
            endpoint: "dadn.azurewebsites.net/plantdata/1/latest",
            requestType: RequestCreator.Type.GET,
            bearerKey: bearerKey,
            callback: (success, response) =>
            {
                if (success)
                {
                    foreach (var point in response.PlantDataPoints)
                    {
                        Debug.Log(point.Timestamp + ": " + point.LightValue);
                    }
                }
                else
                {
                    Debug.Log("Failed to get plant's data.");
                }
            });
    }
}