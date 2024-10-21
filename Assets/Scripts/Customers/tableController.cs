using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableController : MonoBehaviour
{
    // Start is called before the first frame update

    public bool arrived;
    public Transform chair;
    private Vector3 offset;
    private CustomerController customer;


    void Start()
    {
        arrived = false;
        offset = chair.position+ new Vector3(0, 0, 0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void whenArrive(CustomerController controller)
    {
        customer = controller;
        chair.position = offset;
        arrived = true;
        controller.transform.position = chair.position+new Vector3(0,1f,0);
        controller.transform.eulerAngles = new Vector3(0, 180, 0);
    }
}
