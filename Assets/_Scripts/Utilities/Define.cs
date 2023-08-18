using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public static string TOPIC_LIGHT = "light";
    public static string TOPIC_TEMPERATURE = "temperature";
    public static string TOPIC_MOISTURE = "moisture";

    public static string BearerKey = "BearerKey";
    public enum SceneName
    {
        Login = 0,
        Main = 1,
        AR = 2,
    }
    public enum ApplicationState
    {
        Login,
        Main,
    }

    public enum LoginState
    {
        Waiting,
        Proceeding,
        Failed,
        Success,
    }
    public enum ViewName
    {
        PlantInfor = 0,
        ListPlant = 1,
        ARMode = 2,
        Account = 3,
        NewPlant = 4,
        History = 5,
        EditPlant = 6,
    }
    public enum Message
    {
        ShowPanelInfo = 0,
    }
    public enum HistoryType
    {
        Light,
        Temperature,
        Humidity,
    }
    public enum RulesType
    {
        Repeat = 0,
        Metric = 1
    }
    public enum LoginType
    {
        Login = 0,
        Register = 1
    }
    public enum PlantNameReferenceLib
    {
        plant1 = 0,
        plant2 = 1
    }
}
