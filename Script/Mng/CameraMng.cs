using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECameraType
{
    Player,
    UI,
    World,
}
public class CameraMng : TSingleton<CameraMng>
{
    Dictionary<ECameraType, BaseCamera> m_cameraDic = new Dictionary<ECameraType, BaseCamera>(3);
    public void Init()
    {
        m_cameraDic.Add(ECameraType.Player, InstantiateCamera<PlayerCamera>(ECameraType.Player));
        m_cameraDic.Add(ECameraType.UI, InstantiateCamera<BaseCamera>(ECameraType.UI));
    }
    public T InstantiateCamera<T>(ECameraType type) where T : BaseCamera
    {
        T Camera = Instantiate<T>(Resources.Load<T>("Camera/" + type.ToString() + "Camera"));
        Camera.Init();
        return Camera;
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
