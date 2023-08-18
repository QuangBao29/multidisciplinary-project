#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneHelperEditor : MonoBehaviour
{
    [MenuItem("DADN/StartGame _F5", false, 100)]
    static public void OpenLoginAndStart()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Login.unity");
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
    // ============================================================

    [MenuItem("DADN/SceneEditor/Scene_Login", false, 4)]
    static public void OpenSceneLogin()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Login.unity");
    }

    [MenuItem("DADN/SceneEditor/Scene_Main", false, 4)]
    static public void OpenSceneMain()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
    }
    [MenuItem("DADN/SceneEditor/Scene_AR", false, 4)]
    static public void OpenSceneAR()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/AR.unity");
    }
}
#endif
