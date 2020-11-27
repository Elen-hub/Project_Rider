using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    public Camera Camera;
    public virtual BaseCamera Init()
    {
        Camera = GetComponentInChildren<Camera>();
        return this;
    }
}
