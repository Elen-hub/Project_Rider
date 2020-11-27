using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Input : MonoBehaviour
{
    Joystick m_joyStick;
    AccelerateButton m_accelerateButton;
    BreakButton m_breakButton;

    public HUD_Input Init()
    {
        m_joyStick = GetComponentInChildren<Joystick>().Init();
        m_accelerateButton = GetComponentInChildren<AccelerateButton>().Init();
        m_breakButton = GetComponentInChildren<BreakButton>().Init();
        return this;
    }
    public BaseCar SetTarget
    {
        set { 
            m_joyStick.SetInputCar = value;
            m_accelerateButton.SetInputCar = value;
            m_breakButton.SetInputCar = value;
        }
    }
}
