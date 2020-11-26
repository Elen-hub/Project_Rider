using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMng : TSingleton<UIMng>
{
    public Camera UICamera;
    public enum UIName
    {
        Game,
    }
    Dictionary<UIName, UIBase> m_uiDic = new Dictionary<UIName, UIBase>();
    public UIName CLOSE { set { m_uiDic[value].Close(); } }
    public UIName DESTROY { set { Destroy(m_uiDic[value].gameObject); m_uiDic.Remove(value); } }
    public UIName OPEN { set { Open<UIBase>(value); } }
    public T Open<T>(UIName uiName) where T : UIBase
    {
        if (!m_uiDic.ContainsKey(uiName))
        {
            T prefabs = Resources.Load<T>("UI/" + uiName.ToString());

            if (prefabs == null)
                return null;

            T obj = Instantiate<T>(prefabs);
            m_uiDic.Add(uiName, obj.Init());
            obj.Open();
            obj.transform.SetParent(transform);

            return obj;
        }
        else
        {
            m_uiDic[uiName].Open();
            return m_uiDic[uiName] as T;
        }
    }
    public void Init()
    {
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }
}
