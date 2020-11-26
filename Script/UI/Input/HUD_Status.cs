using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class HUD_Status : MonoBehaviour
{
    BaseCar m_character;
    Text m_speedText;
    StringBuilder m_speedString;
    Text m_gearText;
    StringBuilder m_gearString;

    public BaseCar SetTarget {
        set  {
            m_character = value;
        }
    }
    public HUD_Status Init()
    {
        m_speedText = transform.Find("SpeedText").GetComponent<Text>();
        m_gearText = transform.Find("GearText").GetComponent<Text>();
        m_speedString = new StringBuilder(" km/h", 8);
        m_gearString = new StringBuilder("기어: ", 5);
        return this;
    }
    private void LateUpdate()
    {
        m_speedText.text = (m_character.GetMoveSystem.Speed * 50).ToString("000") + m_speedString;
        m_gearText.text = m_gearString + m_character.GetMoveSystem.CurrGear.ToString();
            // (m_character.GetMoveSystem.Speed*50).ToString("F0");
    }
}
