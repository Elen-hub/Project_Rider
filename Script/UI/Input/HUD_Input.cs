using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Input : MonoBehaviour
{
    Joystick m_joyStick;
    Accelerate m_accelerate;

    public HUD_Input Init()
    {
        m_joyStick = GetComponentInChildren<Joystick>().Init();
        m_accelerate = GetComponentInChildren<Accelerate>().Init();
        return this;
    }
    public BaseCar SetTarget
    {
        set { 
            m_joyStick.SetInputCar = value; 
            m_accelerate.SetInputCar = value;
        }
    }
}
