using UnityEngine;
using UnityEngine.Events;

public class Interactable: MonoBehaviour
{
    [HideInInspector] public bool active = false;
    public CanvasGroup marker;
    public UnityEvent callback;

    public void Interact()
    {
        callback.Invoke();
    }

    private void Update()
    {
        marker.alpha = Mathf.Lerp(marker.alpha, active ? 1 : 0, Time.deltaTime * 20);
    }
}