using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    [SerializeField] private References references;
    public IInteractable interactable;
    public IInteractable anchor;

    private Canvas canvas;
    private RectTransform rect;
    private CanvasGroup group;
    private Image image;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        group = GetComponent<CanvasGroup>();
        rect = transform as RectTransform;
        image = GetComponent<Image>();

        rect.SetParent(canvas.transform);
    }

    void Update()
    {
        bool shown = interactable != null;
        group.alpha = Mathf.Lerp(group.alpha, shown ? 1 : 0, Time.deltaTime * 20);

        anchor = interactable ?? anchor;
        if (anchor == null) return;

        try
        {
            references.description.text = anchor.GetText();
            references.button.SetActive(anchor.IsActive());
            image.sprite = anchor.IsActive() ? references.enabled : references.disabled;
            rect.anchoredPosition = Util.WorldToCanvasPosition(canvas, anchor.transform.position + anchor.GetOffset());
        } catch (MissingReferenceException) { anchor = null; }
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
