using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECameraType
{
    Main,
    UI,
    World,
}
public class CameraMng : TSingleton<CameraMng>
{
    Dictionary<ECameraType, BaseCamera> m_cameraDic = new Dictionary<ECameraType, BaseCamera>(3);
    public void Init()
    {
        m_cameraDic.Add(ECameraType.Main, Camera.main.GetComponent<BaseCamera>());
    }
    public T GetCamera<T>(ECameraType type) where T : BaseCamera
    {
        return m_cameraDic[type] as T;
    }
    public BaseCamera GetCamera(ECameraType type)
    {
        return m_cameraDic[type];
    }
}
