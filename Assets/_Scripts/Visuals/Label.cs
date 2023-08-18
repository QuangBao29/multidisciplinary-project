using TMPro;
using UnityEngine;


public class Label : MonoBehaviour
{
    public RectTransform RectTransform;
    public TMP_Text Text;

    public void SetX(float x)
    {
        RectTransform.anchoredPosition = new Vector2(x, RectTransform.anchoredPosition.y);
    }

    public void SetY(float y)
    {
        RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, y);
    }

    public void SetText(string text)
    {
        Text.text = text;
    }
}