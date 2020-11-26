using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSingleton<T> : MonoBehaviour where T : class
{
    static T m_instance;
    public static T Instance
    {
        get 
        {
            if (m_instance != null)
                return m_instance;

            GameObject obj = new GameObject(typeof(T).ToString(), typeof(T));
            //DontDestroyOnLoad(obj);
            m_instance = obj.GetComponent<T>();
            return m_instance;
        }
    }
}
