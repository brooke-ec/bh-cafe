using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    [SerializeField] private References references;
    public IInteractable interactable;
    public IInteractable anchor;

    private RectTransform canvas;
    private RectTransform rect;
    private CanvasGroup group;
    private Image image;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>().transform as RectTransform;
        group = GetComponent<CanvasGroup>();
        rect = transform as RectTransform;
        image = GetComponent<Image>();

        rect.SetParent(canvas);
    }

    void Update()
    {
        bool shown = interactable != null;
        group.alpha = Mathf.Lerp(group.alpha, shown ? 1 : 0, Time.deltaTime * 20);

        anchor = interactable ?? anchor;
        if (anchor == null) return;

        references.description.text = anchor.GetText();
        references.button.SetActive(anchor.IsInteractable());
        image.sprite = anchor.IsInteractable() ? references.enabled : references.disabled;
        rect.anchoredPosition = Util.WorldToCanvasPosition(canvas, anchor.transform.position);
    }

    [Serializable]
    public struct References
    {
        public Sprite enabled;
        public Sprite disabled;
        public GameObject button;
        public TextMeshProUGUI description;
    }
}
