using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AccelerateButton : BaseButton
{
    MoveSystem m_playerMoveSystem;
    BaseCar m_character;
    public BaseCar SetInputCar { set { m_character = value; } }
    public new AccelerateButton Init()
    {
        base.Init();
        return this;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (m_character != null)
            m_character.GetMoveSystem.SetAccelerate = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (m_character != null)
            m_character.GetMoveSystem.SetAccelerate = false;
    }
    private void Update()
    {
        
    }
}
