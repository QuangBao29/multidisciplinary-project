using UnityEngine;
using Visuals;

public abstract class ColoredObject : MonoBehaviour
{
    [SerializeField] private ColorType colorType;
    protected Color Color => VisualManager.Instance.GetColor(colorType);
    protected abstract void Start();
}