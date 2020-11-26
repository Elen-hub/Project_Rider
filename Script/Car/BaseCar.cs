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
            AccelerateSpeed = 0.5f,
            MaxSpeed = 2.5f,
            Handling = 0.1f
        };

        m_module_StatSystem = GetComponent<StatSystem>().Init(testStat);
        m_module_MoveSystem = GetComponent<MoveSystem>().Init(ref m_module_StatSystem);
        return this;
    }
    private void Update()
    {
        m_module_MoveSystem.NextFrame(Time.deltaTime);
    }
}
