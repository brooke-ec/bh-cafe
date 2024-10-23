using UnityEngine;

public class Marker : MonoBehaviour
{
    public Transform anchor;
    public Vector3 offset;
    public bool perspective = false;

    private RectTransform rect;
    private RectTransform canvas;
    private Vector3 size;

    void Start()
    {
        rect = transform as RectTransform;
        canvas = GetComponentInParent<Canvas>().transform as RectTransform;
        size = rect.localScale;
    }

    void Update()
    {
        Vector3 position = anchor.position + offset;
        rect.anchoredPosition = WorldToCanvasPosition(canvas, Camera.main, position);
        
        if (perspective)
        {
            float distance = (position - Camera.main.transform.position).magnitude;
            rect.localScale = size * 1 / distance * 7;
        }
    }

    static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
    {
        Vector3 result = camera.WorldToScreenPoint(position);

        if (result.z < 0) return Vector2.negativeInfinity;

        result.x -= canvas.sizeDelta.x * canvas.pivot.x;
        result.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return result;
    }
}
