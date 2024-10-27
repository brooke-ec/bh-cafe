using UnityEngine;

public class PointPickup : MonoBehaviour
{
    [SerializeField] private Vector2 max;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(
            Random.Range(-max.x, max.x), 
            5,
            Random.Range(-max.y, max.y)
        );
        Debug.Log(GetComponent<Rigidbody>().velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Destroy(gameObject);
    }
}
