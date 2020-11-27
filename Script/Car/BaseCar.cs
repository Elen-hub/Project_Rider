using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatSystem))]
[RequireComponent(typeof(MoveSystem))]
public class BaseCar : MonoBehaviour
{
    StatSystem m_module_StatSystem;
    public StatSystem GetStatSystem { get { return m_module_StatSystem; } }
    MoveSystem m_module_MoveSystem;
    public MoveSystem GetMoveSystem { get { return m_module_MoveSystem; } }
    public BaseCar Init()
    {
        Stat testStat = new Stat()
        {
            AccelerateSpeed = 30,
            MaxSpeed = 160,
            Handling = 1,
            BreakFriction = 50,
        };

        m_module_StatSystem = GetComponent<StatSystem>().Init(testStat);
        m_module_MoveSystem = GetComponent<MoveSystem>().Init(ref m_module_StatSystem);
        return this;
    }
    private void FixedUpdate()
    {
        m_module_MoveSystem.NextFrame(Time.fixedDeltaTime);
    }
}
