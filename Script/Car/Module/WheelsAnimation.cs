using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsAnimation : MonoBehaviour
{
    // 0 Front, 1 Back
    Wheel[,] m_wheels = new Wheel[2,2];
    public Wheel GetWheel(int front = 0, int left = 0)
    {
        return m_wheels[front, left];
    }
    public WheelsAnimation Init()
    {
        Wheel[] wheels = GetComponentsInChildren<Wheel>();
        m_wheels[0, 0] = wheels[0].Init();
        m_wheels[0, 1] = wheels[1].Init();
        m_wheels[1, 0] = wheels[2].Init();
        m_wheels[1, 1] = wheels[3].Init();
        return this;
    }
    public void RotateToHandling(float handling, int front = 0)
    {
        if(front == 0)
        {
            m_wheels[0, 0].RotateToHandling(handling);
            m_wheels[0, 1].RotateToHandling(handling);
        }
        else
        {
            m_wheels[1, 0].RotateToHandling(handling);
            m_wheels[1, 1].RotateToHandling(handling);
        }
    }
    public void RotateToSpeed(float speed)
    {
        m_wheels[0, 0].RotateToSpeed(speed);
        m_wheels[0, 1].RotateToSpeed(speed);
        m_wheels[1, 0].RotateToSpeed(speed);
        m_wheels[1, 1].RotateToSpeed(speed);
    }
}
