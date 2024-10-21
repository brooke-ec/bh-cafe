using UnityEngine;

public class Marker : MonoBehaviour
{
    public Transform anchor;
    public Vector3 offset;

    protected RectTransform rect;
    protected RectTransform canvas;

    void Start()
    {
        rect = transform as RectTransform;
        canvas = GetComponentInParent<Canvas>().transform as RectTransform;
    }

    void Update()
    {
        Vector3 position = anchor.position + offset;
        rect.anchoredPosition = Util.WorldToCanvasPosition(canvas, Camera.main, position);
    }
}
