namespace Visuals
{
    public class ColoredText : ColoredObject
    {
        protected override void Start() => GetComponent<TMPro.TMP_Text>().color = Color;
    }
}