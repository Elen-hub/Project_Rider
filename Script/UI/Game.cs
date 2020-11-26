using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : UIBase
{
    HUD_Input m_input;
    HUD_Status m_status;
    public BaseCar SetTarget { 
        set { 
            m_input.SetTarget = value;
            m_status.SetTarget = value;
        } 
    }
    public override UIBase Init()
    {
        m_input = GetComponentInChildren<HUD_Input>().Init();
        m_status = GetComponentInChildren<HUD_Status>().Init();
        return base.Init();
    }
    public override void Open()
    {
        
    }
    public override void Close()
    {
        
    }
}
