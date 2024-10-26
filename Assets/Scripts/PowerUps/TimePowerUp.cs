using UnityEngine;

public class TimePowerUp : MonoBehaviour
{
    /// <summary>Amount Of Time Gained when collided</summary>
    [SerializeField] private float TimeGained;
    private void OnTriggerEnter(Collider other)
    {
        God.instance.levelUIManager.AddTime(TimeGained);
        GetComponent<Collider>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
