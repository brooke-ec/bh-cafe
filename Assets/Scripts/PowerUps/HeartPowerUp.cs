using UnityEngine;

public class HeartPowerUp : MonoBehaviour
{
    /// <summary>Amount Of Lives Gained when collided</summary>
    [SerializeField] private int LivesGained;

    private void OnTriggerEnter(Collider other)
    {
        // increase heart code goes here
        GetComponent<Collider>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
   
