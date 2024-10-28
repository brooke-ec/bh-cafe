using UnityEngine;

[CreateAssetMenu(fileName = "Untitled Order Type", menuName = "ScriptableObjects/OrderType")]
public class OrderType : ScriptableObject
{
    public string label;
    public int points;
    public Sprite sprite;
}