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
    #region GearVar
    float[] m_gearCoefficient = new float[6] { 1, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f };
    float[] m_gearSpeed;
    int m_currGear = 0;
    const int m_maxGear = 5;
    #endregion

    [SerializeField] EMoveState m_moveState;
    StatSystem m_statSystem;

    float m_friction;
    float m_speed;
    float m_currSpeed;
    Vector3 m_velocity;
    Vector3 m_angle;

    public Vector3 Velociry { get { return m_velocity; } set { m_velocity = value; } }
    public float SetSpeed { set { m_speed = value; } }
    // 5는 속도계수
    public float GetSpeed { get { return m_currSpeed; } }
    public bool SetAccelerate {
        set {
            if (m_moveState != EMoveState.Break)
                m_moveState = value ? EMoveState.Accelerate : EMoveState.Standby;
        }
    }
    public bool SetBreak{
        set {
            m_moveState = value ? EMoveState.Break : EMoveState.Standby;
        }
    }
    public float Handling { get; set; }
    public float CurrGear { get { return m_currGear; } }

    public MoveSystem Init(ref StatSystem stat)
    {
        m_statSystem = stat;
        m_gearSpeed = new float[6] { stat.GetStat.MaxSpeed * 0.25f, stat.GetStat.MaxSpeed * 0.45f, stat.GetStat.MaxSpeed * 0.65f, stat.GetStat.MaxSpeed * 0.8f, stat.GetStat.MaxSpeed * 0.9f, stat.GetStat.MaxSpeed*1.05f };
        return this;
    }
    public void NextFrame(float deltaTime)
    {
        float speed = 0;
        float frictionCoe = Mathf.Clamp01((0.25f * m_statSystem.GetStat.MaxSpeed + m_speed) / m_statSystem.GetStat.MaxSpeed);
        // 마찰력 계산 (기본 마찰 + 코너링마찰계수 * 회전세기)
        m_friction = (GameCoefficient.DefaultFriction + GameCoefficient.CornorFriction * Mathf.Abs(Handling)) * frictionCoe;
        switch (m_moveState)
        {
            case EMoveState.Accelerate:
                speed = Mathf.Clamp(m_speed + (m_statSystem.GetStat.AccelerateSpeed * m_gearCoefficient[m_currGear] - m_friction) * deltaTime, 0, m_statSystem.GetStat.MaxSpeed);
                break;
            case EMoveState.Standby:
                speed = Mathf.Clamp(m_speed - m_friction * deltaTime, 0, m_statSystem.GetStat.MaxSpeed);
                break;
            case EMoveState.Break:
                speed = Mathf.Clamp(m_speed - (m_friction + m_statSystem.GetStat.BreakFriction) * deltaTime, 0, m_statSystem.GetStat.MaxSpeed);
                break;
        }
        m_speed = speed;

        // 기어 상태
        if (m_speed >= m_gearSpeed[m_currGear])
            m_currGear = Mathf.Clamp(m_currGear + 1, 0, m_maxGear);
        else if(m_currGear != 0 && m_speed <= m_gearSpeed[m_currGear - 1])
            m_currGear = Mathf.Clamp(m_currGear - 1, 0, m_maxGear);

        if (m_speed != 0)
        {
            // 핸들링
            m_angle = transform.eulerAngles;
            m_angle.y += (m_moveState != EMoveState.Break ? GameCoefficient.DefaultHandling : GameCoefficient.BreakHandling)
                * Handling * deltaTime * m_statSystem.GetStat.Handling * frictionCoe;

            transform.eulerAngles = m_angle;
        }

        // 가속
        m_velocity = (m_velocity + (transform.forward * m_speed * deltaTime)) * 0.5f;
        transform.position += m_velocity;
        // 속도 (이전거리와 현제거리 * 프레임)
        m_currSpeed = Vector3.Distance(Vector3.zero, m_velocity) * 1 / deltaTime;
    }
}
