using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AStarTestEditor : EditorWindow
{
    static AStarAgent m_agent;
    static EditorWindow m_window;
    Vector2 m_startPos, m_endPos;
    public static void Window(AGrid grid)
    {
        m_window = GetWindow(typeof(AStarTestEditor));
        AStarMng.Instance.Init();
        AStarMng.Instance.Grid = grid;
        GameObject obj = new GameObject(typeof(AStarAgent).ToString(), typeof(AStarAgent));
        m_agent = obj.AddComponent<AStarAgent>();
    }

    private void OnGUI()
    {
        Vector2 windowSize = new Vector2(0, 0);

        m_startPos = EditorGUI.Vector2Field(new Rect(0, windowSize.y, 400, 20), "StartPos" ,m_startPos);
        windowSize.y += 40;
        m_endPos = EditorGUI.Vector2Field(new Rect(0, windowSize.y, 400, 20), "EndPos", m_endPos);
        windowSize.y += 40;

        if(GUI.Button(new Rect(0, windowSize.y, 400, 20), "Start Pathfinding"))
            m_agent.TestSearch(m_startPos, m_endPos);

        windowSize.y += 30;

        windowSize.x = 400;
        if (m_window.position.size != windowSize)
            m_window.position = new Rect(m_window.position.position, windowSize);
    }
    private void OnDestroy()
    {
        DestroyImmediate(m_agent.gameObject);
        DestroyImmediate(AStarMng.Instance.gameObject);
    }
}
