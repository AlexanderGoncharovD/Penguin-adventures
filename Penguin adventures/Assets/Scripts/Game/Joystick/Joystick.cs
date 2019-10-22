using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Vector2 direction { get { return new Vector2(input.x, input.y); } }
    public float horizontal { get { return input.x; } }
    public float vertical { get { return input.y; } }

    private Vector2 input = Vector2.zero;

    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    private Canvas canvas;
    private Camera cam;

    protected virtual void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMax = center;
        handle.anchorMin = center;
        handle.pivot = center;
        handle.anchoredPosition = center;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        /*background.gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.140625f);
        handle.gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.140625f);*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        HandleInput(input.magnitude, input.normalized);
        handle.anchoredPosition = input * radius;
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalized)
    {
        if (magnitude > 0)
        {
            if (magnitude > 1)
                input = normalized;
        }
        else
            input = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
       /* background.gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
        handle.gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);*/
    }
}
