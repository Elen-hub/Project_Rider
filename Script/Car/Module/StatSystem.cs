using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stat
{
    public float AccelerateSpeed;
    public float MaxSpeed;
    public float Handling;
}
public class StatSystem : MonoBehaviour
{
    Stat m_baseStat;
    public Stat GetStat { get { return m_baseStat; } }
    public StatSystem Init(Stat baseStat)
    {
        m_baseStat = baseStat;
        return this;
    }
}
