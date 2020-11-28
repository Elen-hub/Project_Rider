using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class ASortTrack : EditorWindow
{
    static EditorWindow m_window;

    [MenuItem("Track/SortTrack")]
    public static void Window()
    {
        GameObject obj = GameObject.Find("Track");
        if (obj == null)
            return;

        Transform parent = obj.transform;
        
        for(int i =0; i<parent.childCount; ++i)
        {
            Transform child = parent.GetChild(i);
            child.localPosition = new Vector3(Mathf.RoundToInt(child.localPosition.x*0.025f)*40, 0, Mathf.RoundToInt(child.localPosition.z * 0.025f) *40);
        }
    }
}
