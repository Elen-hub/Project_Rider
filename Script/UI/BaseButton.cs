using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public abstract class BaseButton: MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    protected Image m_buttonImg;
    public void Init()
    {
        m_buttonImg = GetComponent<Image>();
    }
    public abstract void OnPointerDown(PointerEventData eventData);
    public abstract void OnPointerUp(PointerEventData eventData);
}
