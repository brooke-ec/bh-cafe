using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public float spawnTime = 10;
    public float running = 0;
    public GameObject Customer;
    public GameObject[] Tables;
    void Start()
    {
        Tables = GameObject.FindGameObjectsWithTag("Table");
    }

    // Update is called once per frame
    void Update()
    {
        if (running > spawnTime)
        {
            List<GameObject> tablesList = Tables.ToList();
            int index = Random.Range(0, tablesList.Count);
            GameObject table = tablesList[index];
            while(table.GetComponent<WayPoint>().empty == false)
            {
                tablesList.Remove(table);
                if (tablesList.Count ==0)
                {
                    return;
                }
                index = Random.Range(0, tablesList.Count);
                table = tablesList[index];
                
            }
            CustomerController NewCustomer = Instantiate(Customer,transform.position,transform.rotation).GetComponent<CustomerController>();
            NewCustomer.waypoints = table.GetComponent<WayPoint>().waypoints;
            table.GetComponent<WayPoint>().empty = false;
            running = 0;
        }
        else
        {
            running += Time.deltaTime;
        }
        
    }
}
