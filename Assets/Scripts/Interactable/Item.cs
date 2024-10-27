using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Type type = Type.Other;

    public new Type GetType() { return type; }

    public enum Type
    {
        Other,
        Coffee,
        Food,
    }
}
