using UnityEngine;

public class TableController : MonoBehaviour
{
    [SerializeField] private Marker marker;
    public CustomerController customer;
    public PlayerController player;
    public Transform sitAnchor;
    public Transform chair;

    private Vector3 chairPos;

    void Start()
    {
        chairPos = chair.position;
        marker.transform.SetParent(FindObjectOfType<Canvas>().transform);
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (customer == null) return;

        if (marker.gameObject.activeSelf != customer.IsSitting())
            marker.gameObject.SetActive(customer.IsSitting());

        if (customer.IsSitting()) chair.position = new Vector3(
            sitAnchor.position.x, 
            chairPos.y,
            sitAnchor.position.z
        );
        else chair.position = chairPos;
    }

    public void Serve()
    {
        if (customer == null || !customer.IsSitting()) return;
        if (player.heldItem == null) return;
        player.ClearHeld();
        customer.Leave();
    }

    public bool IsEmpty()
    {
        return customer == null;
    }
}
