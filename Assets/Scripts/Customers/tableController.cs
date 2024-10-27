using UnityEngine;

public class TableController : MonoBehaviour
{
    public CustomerController customer;
    public PlayerController player;
    public Transform sitAnchor;
    public Transform chair;

    private Vector3 chairPos;

    void Start()
    {
        chairPos = chair.position;
        player = FindObjectOfType<PlayerController>();
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
