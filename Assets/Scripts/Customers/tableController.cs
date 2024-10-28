using TMPro;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public CustomerController customer;
    public Transform sitAnchor;
    public int tableNumber;
    public Transform chair;

    private Vector3 chairPos;

    void Start()
    {
        chairPos = chair.position;
        foreach (var t in GetComponentsInChildren<TextMeshPro>()) t.text = tableNumber.ToString();
    }

    void Update()
    {
        if (customer == null) return;

        if (customer.IsSitting()) chair.position = new Vector3(
            sitAnchor.position.x, 
            chairPos.y,
            sitAnchor.position.z
        );
        else chair.position = chairPos;
    }

    public bool IsEmpty()
    {
        return customer == null;
    }
}
