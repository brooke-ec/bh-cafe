using UnityEngine;

public class SpeedPowerUpRotate : MonoBehaviour
{

    [SerializeField] float spinSpeed = 10f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        // Just added another axis to this
        transform.Rotate(
            0f,
            spinSpeed*Time.deltaTime, 
            0f
        );
        transform.position = new Vector3(transform.position.x, startPosition.y+Mathf.Sin(Time.time)*0.5f, transform.position.z);
    }
}
