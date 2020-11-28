using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    LayerMask m_layerMask;
    float m_horizonAxis;
    Vector3 m_rotation;
    const float m_radius = 0.5f;
    const float m_pi = 3.14f;
    float m_sign = 1;
    public void RotateToSpeed(float speed)
    {
        m_rotation.z -= m_sign * speed / (m_pi * m_radius * 2);
        transform.localEulerAngles = m_rotation;
    }
    public void RotateToHandling(float handling)
    {
        m_rotation.y = m_horizonAxis + handling * 45;
    }
    public bool IsGround()
    {
        return Physics.CheckSphere(transform.position, m_radius, m_layerMask);
    }
    public Wheel Init()
    {
        m_rotation = transform.localEulerAngles;
        m_horizonAxis = m_rotation.y;
        if (m_horizonAxis < 180)
            m_sign = -1;
        return this;
    }
}
