using UnityEngine;

public class SpeedPowerUpRotate : MonoBehaviour
{
    void Update()
    {
        // Just added another axis to this
        transform.rotation = Quaternion.Euler(
            Camera.main.transform.rotation.eulerAngles.x, 
            Camera.main.transform.rotation.eulerAngles.y, 
            0f
        );
    }
}
