using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public static class RequestCreator
{
    public enum Type
    {
        GET,
        POST,
    }

    private static UnityWebRequest ConstructWebRequest(string endpoint, Type requestType, string bearerKey,
        object objectToSend = null)
    {
        var requestTypeString = requestType switch
        {
            Type.GET => "GET",
            Type.POST => "POST",
        };

        UnityWebRequest webRequest = new UnityWebRequest("https://" + endpoint, requestTypeString);

        if (objectToSend != null)
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JsonConvert.SerializeObject(objectToSend));
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        }

        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        if (bearerKey != "") webRequest.SetRequestHeader("Authorization", "Bearer " + bearerKey);

        return webRequest;
    }

    public static IEnumerator SendRequest(string endpoint, Type requestType, Action<bool> callback, string bearerKey,
        object objectToSend = null)
    {
        var webRequest = ConstructWebRequest(endpoint, requestType, bearerKey, objectToSend);

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed request: " + webRequest.url + "\nMessage: " + webRequest.error);
            callback?.Invoke(false);
            yield break;
        }

        callback?.Invoke(true);
        webRequest.Dispose();
    }

    public static IEnumerator SendRequest<T>(string endpoint, Type requestType, Action<bool, T> callback,
        string bearerKey, object objectToSend = null) where T : new()
    {
        var webRequest = ConstructWebRequest(endpoint, requestType, bearerKey, objectToSend);

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed request: " + webRequest.url + "\nMessage: " + webRequest.error);
            callback?.Invoke(false, new T());
            yield break;
        }
        Debug.LogError(webRequest.downloadHandler.text);
        var receivedObject = JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
        callback?.Invoke(true, receivedObject);
        webRequest.Dispose();
    }
}