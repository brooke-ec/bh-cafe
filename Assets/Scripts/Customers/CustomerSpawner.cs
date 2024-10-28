using System.Linq;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private float spawnDelay = 10;
    [SerializeField] private GameObject[] customers;

    private float timer = 0;
    private TableController[] tables;

    void Start()
    {
        tables = FindObjectsOfType<TableController>();
    }

    void Update()
    {
        if (timer < 0)
        {
            TableController[] options = tables.Where(t => t.IsEmpty()).ToArray();
            if (options.Length == 0) return;
            
            TableController table = Util.PickRandom(options);
            GameObject customer = Util.PickRandom(customers);

            CustomerController instance = Instantiate(customer, transform.position, transform.rotation).GetComponent<CustomerController>();
            table.customer = instance;
            instance.exit = transform;
            instance.table = table;
            timer = spawnDelay;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        
    }
}
