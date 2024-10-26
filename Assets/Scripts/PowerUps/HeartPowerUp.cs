using UnityEngine;

public class HeartPowerUp : MonoBehaviour
{
    /// <summary>Amount Of Lives Gained when collided</summary>
    [SerializeField] private int LivesGained;

    private void OnTriggerEnter(Collider other)
    {
        God.instance.levelUIManager.GainHeart();
        GetComponent<Collider>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
   
