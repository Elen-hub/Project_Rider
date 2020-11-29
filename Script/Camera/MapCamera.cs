using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : BaseCamera
{
    Vector3 m_localPosition = new Vector3(0, 400, -200);
    public void Enabled(BaseCar character)
    {
        transform.SetParent(character.transform);
        transform.localPosition = m_localPosition;
    }
}
