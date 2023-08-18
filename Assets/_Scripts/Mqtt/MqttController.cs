using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MqttController : MonoBehaviour
{
    public string nameController = "Controller 1";
    //public string tagOfTheMQTTReceiver = "";
    public MqttReceiver _eventSender;
    [SerializeField]
    private TextMeshProUGUI _output = null;

    void Start()
    {
        //_eventSender = GameObject.FindGameObjectsWithTag(tagOfTheMQTTReceiver)[0].gameObject.GetComponent<MqttReceiver>();
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {
        //_output.text = newMsg;
        Debug.Log("Event Fired. The message, from Object " + nameController + " is = " + newMsg);
    }
}