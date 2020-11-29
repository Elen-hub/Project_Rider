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
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        m_buttonImg.color = Color.grey;
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        m_buttonImg.color = Color.white;
    }
}
