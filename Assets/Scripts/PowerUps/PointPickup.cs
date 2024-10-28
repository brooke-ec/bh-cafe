using UnityEngine;

public class PointPickup : MonoBehaviour
{
    [SerializeField] private Vector2 max;
    [SerializeField] private float delay;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(
            Random.Range(-max.x, max.x),
            5,
            Random.Range(-max.y, max.y)
        ), ForceMode.Impulse);
    }

    private void Update()
    {
        delay -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (delay > 0) return;
        if (!other.CompareTag("Player")) return;
        God.instance.levelUIManager.GainDiamond(1);
        Destroy(gameObject);
    }
}
