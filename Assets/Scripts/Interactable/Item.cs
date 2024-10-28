using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private OrderType type = null;

    public new OrderType GetType() { return type; }
}
