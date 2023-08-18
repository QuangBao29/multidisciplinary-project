namespace Visuals
{
    public class ColoredImage : ColoredObject
    {
        protected override void Start() => GetComponent<UnityEngine.UI.Image>().color = Color;
    }
}