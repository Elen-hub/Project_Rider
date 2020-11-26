using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMoveState
{
    Standby,
    Accelerate,
    Break,
}

public class MoveSystem : MonoBehaviour
{
    float[] m_gearCoefficient = new float[6] { 1.3f, 1.15f, 1, 0.85f, 0.4f, 0.1f };
    float[] m_gearSpeed;
    int m_currGear = 0;
    const int m_maxGear = 5;
    [SerializeField] EMoveState m_moveState;
    StatSystem m_statSystem;
    public bool SetAccelerate {
        set {
            if (m_moveState != EMoveState.Break)
                m_moveState = value ? EMoveState.Accelerate : EMoveState.Standby;
        }
    }
    Vector3 m_angle;
    public float Handling { get; set; }
    public float Speed { get; set; }
    public float CurrGear { get { return m_currGear; } }
    public MoveSystem Init(ref StatSystem stat)
    {
        m_statSystem = stat;
        m_gearSpeed = new float[6] { stat.GetStat.MaxSpeed * 0.25f, stat.GetStat.MaxSpeed * 0.45f, stat.GetStat.MaxSpeed * 0.65f, stat.GetStat.MaxSpeed * 0.8f, stat.GetStat.MaxSpeed * 0.9f, stat.GetStat.MaxSpeed *0.1f };
        return this;
    }
    public void NextFrame(float deltaTime)
    {
        if (m_moveState == EMoveState.Accelerate)
        {
            float speed = Mathf.Clamp(Speed + m_statSystem.GetStat.AccelerateSpeed * deltaTime * m_gearCoefficient[m_currGear], 0, m_statSystem.GetStat.MaxSpeed);
            Speed = speed;
            if (Speed >= m_gearSpeed[m_currGear])
                m_currGear = Mathf.Clamp(m_currGear + 1, 0, m_maxGear);
        }
        else
        {
            float speed = Mathf.Clamp(Speed - m_statSystem.GetStat.AccelerateSpeed * deltaTime, 0, m_statSystem.GetStat.MaxSpeed);
            Speed = speed;
            if (Speed < m_gearSpeed[m_currGear])
                m_currGear = Mathf.Clamp(m_currGear - 1, 0, m_maxGear);
        }

        m_angle.y += Handling * deltaTime;
        transform.eulerAngles = m_angle;
        transform.Translate(transform.forward * Speed * deltaTime);
    }
}
