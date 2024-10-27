using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUpInteract : MonoBehaviour
{
    public float speedTime = 5f;
    private float timeSinceSpeed = 0;
    private PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        if (timeSinceSpeed <= 0)
        {
            if (playerController == null) return;
            else
            {
                playerController.SpeedDown();
                playerController = null;
            }
        }
        else timeSinceSpeed -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController controller)) 
        {
            playerController = controller;
            playerController.SpeedUp();
            timeSinceSpeed = speedTime;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            AudioManager.instance.PlaySound(AudioManager.SoundEnum.pickUp);
        }
    }
}
