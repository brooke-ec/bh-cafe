using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    /// <summary>The different Power Ups that can be spawned</summary>
    [SerializeField] private GameObject[] PowerUps;
    /// <summary>The Percentage Chance(0-100) of an power Up spawning each second</summary>
    [SerializeField] private float SpawnRate;
    /// <summary>if already spawned this game</summary>
    private bool powerUpSpawned = false;
    /// <summary>1 second timer </summary>
    private float timer = 1f;

    // Update is called once per frame
    void Update()
    {
        if (SpawnRate == 0 || powerUpSpawned) return;
        if (timer <= 0)
        {
            float randomNumber = Random.value;

            if (randomNumber <= SpawnRate / 100)
            {
                Instantiate(PowerUps[Random.Range(0, PowerUps.Length)], transform);
                powerUpSpawned = true;
            }
            timer = 1f;
        }
        else timer -= Time.deltaTime;
    }
}
