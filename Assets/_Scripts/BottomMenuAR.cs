using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomMenuAR : MonoBehaviour
{
    public void OnClickShowViewListPlant()
    {
        SceneManager.Instance.ChangeScene(Define.SceneName.Main.ToString(), new SceneManager.Param { viewName = Define.ViewName.ListPlant, Id = 0 });
    }
    public void OnClickShowViewAccount()
    {
        SceneManager.Instance.ChangeScene(Define.SceneName.Main.ToString(), new SceneManager.Param { viewName = Define.ViewName.Account, Id = 0 });
    }
}
