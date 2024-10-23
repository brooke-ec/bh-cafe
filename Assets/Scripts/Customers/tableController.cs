using UnityEngine;

public class TableController : MonoBehaviour
{
    public CustomerController customer;
    [SerializeField] private Transform chair;

    private Vector3 offset;
    private bool arrived = false;


    void Start()
    {
        offset = chair.position+ new Vector3(0, 0, 0.75f);
    }

    public void Arrive()
    {
        arrived = true;
        chair.position = offset;
        customer.transform.position = chair.position + new Vector3(0,1f,0);
        customer.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public bool IsEmpty()
    {
        return customer == null;
    }
}
