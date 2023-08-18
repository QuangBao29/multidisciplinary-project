using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    // Start is called before the first frame update
    public void OnClickLogOut()
    {
        SceneManager.Instance.ChangeScene(Define.SceneName.Login.ToString(), null);
    }
}
