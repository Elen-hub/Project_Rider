using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class Joystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    PointerEventData m_eventData;
    Transform m_camera;
    private Image m_backgroundImg;
    private Image m_buttonImg;
    private float m_axis = 0;

    BaseCar m_character;
    public BaseCar SetInputCar { set { m_character = value; } }
    bool m_isDown;
    public Joystick Init()
    {
        m_backgroundImg = GetComponent<Image>();
        m_buttonImg = transform.Find("Pivot").GetComponent<Image>();
        return this;
    }
    public void Enabled()
    {
        gameObject.SetActive(true);
    }

    public void Disabled()
    {
        m_isDown = false;
        SetJoystickButtonAnchor(Vector2.zero);
        gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        m_isDown = true;
        m_eventData = eventData;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        m_isDown = false;
        m_axis = 0;
        m_character.GetMoveSystem.Handling = 0;
        SetJoystickButtonAnchor(Vector2.zero);
    }
    void SetJoystickButtonAnchor(Vector2 pos)
    {
        m_buttonImg.rectTransform.anchoredPosition = pos;
        m_axis = pos.x;
    }

    void Update()
    {
        if (m_isDown)
        {
            Vector2 pos;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_backgroundImg.rectTransform,
                   m_eventData.position,
                   m_eventData.pressEventCamera,
                   out pos))
            {
                pos.x = (pos.x / m_backgroundImg.rectTransform.sizeDelta.x) * 2;
                pos.y = 0;

                if (pos.magnitude > 1)
                    pos.Normalize();

                // 핸들링 지연효과
                // pos.x = Mathf.Clamp(pos.x, m_axis - Time.deltaTime*10, m_axis + Time.deltaTime*10);
                SetJoystickButtonAnchor(pos * 150);
                m_axis = pos.x;

                m_character.GetMoveSystem.Handling = m_axis;
            }
        }
    }
}