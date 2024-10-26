using UnityEngine;

public class TimePowerUp : MonoBehaviour
{
    /// <summary>Amount Of Time Gained when collided</summary>
    [SerializeField] private float TimeGained;
    private void OnTriggerEnter(Collider other)
    {
        // Add time code goes here
        GetComponent<Collider>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
