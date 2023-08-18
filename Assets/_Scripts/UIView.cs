using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIView : MonoBehaviour
{
    public string ViewName;
    private UIViewManager _viewManager;
    public UIViewManager UIViewManager => _viewManager;
    public void Initialize(UIViewManager uiviewmanager)
    {
        _viewManager = uiviewmanager;
    }

}
