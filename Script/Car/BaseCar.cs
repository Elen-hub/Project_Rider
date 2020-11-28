using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WheelsAnimation))]
[RequireComponent(typeof(StatSystem))]
[RequireComponent(typeof(MoveSystem))]
public class BaseCar : MonoBehaviour
{
    StatSystem m_module_StatSystem;
    public StatSystem GetStatSystem { get { return m_module_StatSystem; } }
    MoveSystem m_module_MoveSystem;
    public MoveSystem GetMoveSystem { get { return m_module_MoveSystem; } }
    WheelsAnimation m_wheelAnimation;
    public BaseCar Init(Stat stat)
    {
        m_wheelAnimation = GetComponent<WheelsAnimation>().Init();
        m_module_StatSystem = GetComponent<StatSystem>().Init(stat);
        m_module_MoveSystem = GetComponent<MoveSystem>().Init(ref m_module_StatSystem);
        return this;
    }
    private void FixedUpdate()
    {
        m_module_MoveSystem.NextFrame(Time.fixedDeltaTime);
        m_wheelAnimation.RotateToSpeed(m_module_MoveSystem.GetSpeed);
        m_wheelAnimation.RotateToHandling(m_module_MoveSystem.Handling, 0);
    }
}
