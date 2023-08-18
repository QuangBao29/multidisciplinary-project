using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : PersistentSingleton<SceneManager>
{
    public class Param
    {
        public Define.ViewName viewName;
        public int Id;
    }
    private string _currentScene;
    private Param _currentParam;

    public Param CurrentParam
    {
        get => _currentParam;
        set => CurrentParam = value;
    }
    //public UnityAction ShowPanelInfor;
    //public UnityAction ShowListPlant;
    //public UnityAction ShowAccount;
    //public UnityAction Init;

    protected override void Awake()
    {
        base.Awake();
    }
    public string GetCurrentScene()
    {
        return _currentScene;
    }

    public void ChangeScene(string scene, Param param)
    {
        _currentScene = scene;
        _currentParam = param;
        Process(scene);
    }
    private void Process(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }    

}