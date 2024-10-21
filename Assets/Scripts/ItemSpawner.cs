using UnityEngine;

public class ItemSpawner : Interactable
{
    [SerializeField] private GameObject item;

    override public void Interact(PlayerController player)
    {
        Debug.Log("I am the Blahaj of the abyss!");
    }
}
