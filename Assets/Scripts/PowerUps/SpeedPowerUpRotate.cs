using UnityEngine;

public class SpeedPowerUpRotate : MonoBehaviour
{

    [SerializeField] float spinSpeed = 10f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }
    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
        transform.localPosition = new Vector3(transform.localPosition.x, startPosition.y+Mathf.Sin(Time.time)*0.5f, transform.localPosition.z);
    }
}
