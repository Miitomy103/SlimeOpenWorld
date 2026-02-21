using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEvent : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public Action onPointerDown;
    public Action onPointerUp;
    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke();
    }
}
