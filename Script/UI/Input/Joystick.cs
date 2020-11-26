using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class Joystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    // 이미지 설정에 대한 설명
    // 1. 백그라운드의 이미지를 왼쪽 하단으로 설정합니다.
    // 2. 백그라운드의 앵커를 0.5, 0.5 로 설정합니다.
    // 이렇게 설정한다면, 포지션값은 좌측이 -0.5로 시작해서 우측은 0.5로 끝나게 됩니다.

    PointerEventData m_eventData;
    Transform m_camera;
    private Image m_backgroundImg;
    private Image m_buttonImg;
    private Vector2 m_axis = Vector2.zero;

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
        SetJoystickButtonAnchor(Vector2.zero);
    }
    // 버튼 이미지의 앵커 값을 변경합니다.
    void SetJoystickButtonAnchor(Vector2 pos)
    {
        m_buttonImg.rectTransform.anchoredPosition = pos;
        m_axis = pos;
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
                // 백터의 길이값을 확인 했을때 1이 넘어갈 경우 단위백터로 설정합니다.
                if (pos.magnitude > 1.0f)
                    pos.Normalize();

                SetJoystickButtonAnchor(pos * 100);
                m_axis = pos;

                // m_axis.x;
            }
        }
    }
}