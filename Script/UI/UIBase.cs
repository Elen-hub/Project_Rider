using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected const float m_distanceFactor = 1;
    public virtual UIBase Init()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = m_distanceFactor;
        return this;
    }
    public abstract void Open();
    public abstract void Close();
}
