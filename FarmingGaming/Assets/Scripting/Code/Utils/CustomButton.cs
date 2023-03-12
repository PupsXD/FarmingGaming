using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] UnityEvent _onClick = new UnityEvent();
    [SerializeField] UnityEvent _onPointerDown = new UnityEvent();
    [SerializeField] UnityEvent _onPointerUp = new UnityEvent();

    public UnityEvent Click
    {
        get { return _onClick; }
    }
    public UnityEvent PointerDown
    {
        get { return _onPointerDown; }
    }
    public UnityEvent PointerUp
    {
        get { return _onPointerUp; }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _onClick.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _onPointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _onPointerUp.Invoke();
    }
}
