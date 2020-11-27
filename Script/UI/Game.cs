using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : UIBase
{
    HUD_Input m_input;
    HUD_Dashboard m_dashBoard;
    public BaseCar SetTarget { 
        set { 
            m_input.SetTarget = value;
            m_dashBoard.SetTarget = value;
        } 
    }
    public override UIBase Init()
    {
        m_input = GetComponentInChildren<HUD_Input>().Init();
        m_dashBoard = GetComponentInChildren<HUD_Dashboard>().Init();
        return base.Init();
    }
    public override void Open()
    {
        
    }
    public override void Close()
    {
        
    }
}
