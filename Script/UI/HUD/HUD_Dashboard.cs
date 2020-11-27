using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class HUD_Dashboard : MonoBehaviour
{
    BaseCar m_character;
    Text m_speedText;
    StringBuilder m_speedString;
    Text m_gearText;
    StringBuilder m_gearString;
    Transform m_speedoMetor;

    public BaseCar SetTarget {
        set  {
            m_character = value;
        }
    }
    public HUD_Dashboard Init()
    {
        m_speedText = transform.Find("SpeedText").GetComponent<Text>();
        m_gearText = transform.Find("GearText").GetComponent<Text>();
        m_speedString = new StringBuilder(" km/h", 8);
        m_gearString = new StringBuilder("기어: ", 5);
        m_speedoMetor = transform.Find("SpeedoMeter").Find("Needle");
        return this;
    }
    private void LateUpdate()
    {
        float speed = m_character.GetMoveSystem.GetSpeed;
        m_speedText.text = speed.ToString("000") + m_speedString;
        m_gearText.text = m_gearString + (m_character.GetMoveSystem.CurrGear+1).ToString();
        m_speedoMetor.eulerAngles = -Vector3.forward * speed;
    }
}
