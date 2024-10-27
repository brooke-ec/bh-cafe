using UnityEngine;

public interface IInteractable
{
    Transform transform { get; }
    void Interact(PlayerController player);
    bool IsVisible();
    bool IsActive();
    string GetText();
    Vector3 GetOffset();
}
