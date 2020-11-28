using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : BaseCamera
{
    static Vector3 m_initPos = new Vector3(0, 5, 0);
    static Vector3 m_initAngle = new Vector3(25, 0, 0);
    Vector3 m_pos;
    float m_prevSpeed;

    const float m_minDistanceToTarget = 6f;
    const float m_maxDistanceToTarget = 10f;
    const float m_maxDistanceToSpeed = 130f;
    float m_distanceToTarget;

    BaseCar m_character;
    public override BaseCamera Init()
    {
        return base.Init();
    }
    public void Enabled(BaseCar target)
    {
        m_character = target as BaseCar;
        transform.localPosition = m_initPos;
        transform.localEulerAngles = m_initAngle;
        m_distanceToTarget = 1;
        m_pos = m_character.transform.position + m_initPos;
        m_prevSpeed = m_character.GetMoveSystem.GetSpeed;
        gameObject.SetActive(true);
    }
    void Update()
    {
        float currSpeed = m_character.GetMoveSystem.GetSpeed;
        m_distanceToTarget = currSpeed / m_maxDistanceToSpeed;
        if (currSpeed != 0 && m_prevSpeed != 0)
            m_distanceToTarget += currSpeed / m_prevSpeed * Time.fixedDeltaTime * 2;

        m_distanceToTarget = Mathf.Clamp(m_minDistanceToTarget+m_distanceToTarget, 0, m_maxDistanceToTarget);
        m_pos = m_character.transform.position + m_initPos;
        m_pos -= m_character.transform.forward * m_distanceToTarget;
        transform.position = m_pos;

        if (m_character.GetMoveSystem.Velociry == Vector3.zero)
            return;

        float angle = Vector3.Angle(Vector3.forward, m_character.GetMoveSystem.Velociry);
        if (m_character.GetMoveSystem.Velociry.x < 0)
            angle *= -1;

        transform.rotation = Quaternion.Euler(m_initAngle.x, angle, 0);
        m_prevSpeed = currSpeed;
    }
}
