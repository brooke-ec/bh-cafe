using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject item;

    public void Interact()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player.IsHolding()) return;
        
        GameObject instance = Instantiate(item);
        player.Pickup(instance);
    }
}
