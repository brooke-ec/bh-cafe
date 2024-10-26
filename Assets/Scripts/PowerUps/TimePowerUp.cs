using UnityEngine;

public class TimePowerUp : MonoBehaviour
{
    /// <summary>Amount Of Time Gained when collided</summary>
    [SerializeField] private float TimeGained;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            God.instance.levelUIManager.AddTime(TimeGained);
            GetComponent<Collider>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            AudioManager.instance.PlaySound(AudioManager.SoundEnum.pickUp);
        }
    }
}
