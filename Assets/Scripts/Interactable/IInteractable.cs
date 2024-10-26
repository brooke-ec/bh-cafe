using UnityEngine;

public interface IInteractable
{
    Transform transform { get; }
    void Interact(PlayerController player);
    bool IsInteractable();
    string GetText();
}
