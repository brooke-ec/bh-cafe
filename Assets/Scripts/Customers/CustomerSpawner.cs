using System.Linq;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public bool active = false;
    [SerializeField] private float spawnDelay = 10;
    [SerializeField] private GameObject[] customers;
    [SerializeField] private Transform exit;

    private float timer = 0;
    private TableController[] tables;

    public static void SetActive(bool state)
    {
        foreach (CustomerSpawner spawner in FindObjectsOfType<CustomerSpawner>())
        {
            spawner.active = state;
        }
    }

    void Start()
    {
        tables = FindObjectsOfType<TableController>();
    }

    void Update()
    {
        if (!active) return;
        if (timer < 0)
        {
            TableController[] options = tables.Where(t => t.IsEmpty()).ToArray();
            if (options.Length == 0) return;
            
            TableController table = Util.PickRandom(options);
            GameObject customer = Util.PickRandom(customers);

            CustomerController instance = Instantiate(customer, transform.position, transform.rotation).GetComponent<CustomerController>();
            table.customer = instance;
            instance.exit = exit;
            instance.table = table;
            timer = spawnDelay;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        
    }
}
