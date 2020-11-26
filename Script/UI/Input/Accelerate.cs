using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Accelerate : BaseButton
{
    MoveSystem m_playerMoveSystem;
    BaseCar m_character;
    public BaseCar SetInputCar { set { m_character = value; } }
    public new Accelerate Init()
    {
        base.Init();
        return this;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (m_character != null)
            m_character.GetMoveSystem.SetAccelerate = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {

    }
    private void Update()
    {
        
    }
}
