using UnityEngine;
//using static UnityEditor.Progress;

public class ItemSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private float cooldown;
    [SerializeField] private string label;

    private float timer;

   void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }

    void IInteractable.Interact(PlayerController player)
    {
        if (player.IsHolding()) return;
        GameObject instance = Instantiate(Util.PickRandom(items));
        player.Pickup(instance);
        timer = cooldown;
    }

    bool IInteractable.IsActive()
    {
        return timer <= 0;
    }

    string IInteractable.GetText()
    {
        return timer <= 0 ? "To Pickup" : $"{label} ({Mathf.CeilToInt(timer)})";
    }

    bool IInteractable.IsVisible()
    {
        return true;
    }

    Vector3 IInteractable.GetOffset()
    {
        return Vector3.zero;
    }
}
