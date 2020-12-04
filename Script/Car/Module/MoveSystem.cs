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

    AStarAgent m_astarAgent;
    public Vector3 Velocity { get { return m_velocity; } }
    public Vector3 SetVelocity { 
        set  {
            m_velocity = value * m_speed;
        } 
    }
    public float SetSpeed { set { m_speed *= value; } }
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
        m_astarAgent = GetComponent<AStarAgent>();
        m_statSystem = stat;
        m_gearSpeed = new float[6] { stat.GetStat.MaxSpeed * 0.25f, stat.GetStat.MaxSpeed * 0.45f, stat.GetStat.MaxSpeed * 0.65f, stat.GetStat.MaxSpeed * 0.8f, stat.GetStat.MaxSpeed * 0.9f, stat.GetStat.MaxSpeed*1.05f };
        return this;
    }
    public void NextFrame(float deltaTime)
    {
        ANode node = m_astarAgent.GetNodeToPosition(transform.position);
        float speed = 0;
        // 마찰력 계산 (기본 마찰 + 코너링마찰계수 * 회전세기)
        m_friction = (GameCoefficient.DefaultFriction + GameCoefficient.CornorFriction * Mathf.Abs(Handling) * m_statSystem.GetStat.Handling) * (1 - node.Friction);
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
        m_speed = speed * node.Speed;
        // 기어 상태
        if (m_speed >= m_gearSpeed[m_currGear])
            m_currGear = Mathf.Clamp(m_currGear + 1, 0, m_maxGear);
        else if(m_currGear != 0 && m_speed <= m_gearSpeed[m_currGear - 1])
            m_currGear = Mathf.Clamp(m_currGear - 1, 0, m_maxGear);

        // 핸들링
        if (m_speed != 0)
        {
            m_angle = transform.eulerAngles;
            m_angle.y += (m_moveState != EMoveState.Break ? GameCoefficient.DefaultHandling : GameCoefficient.BreakHandling)
                * Handling * deltaTime * m_statSystem.GetStat.Handling;
            transform.eulerAngles = m_angle;
        }

        // 가속
        m_velocity = (m_velocity * 4 + (transform.forward * m_speed)) * 0.2f;
        Vector3 nextPos = transform.position + m_velocity * deltaTime;
        // 이동 가능 체크
        if (!m_astarAgent.GetPossibleMoveToPosition(transform.position, ref nextPos))
        {
            m_velocity = m_velocity.normalized;
            m_speed = 1;
            return;
        }
        transform.position = nextPos;
        // 속도
        m_currSpeed = Vector3.Distance(Vector3.zero, m_velocity);
    }
}
