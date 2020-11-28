using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AStarBuildEditor : EditorWindow
{
    static AGrid m_grid;
    static EditorWindow m_window;
    bool m_isDone;

    [MenuItem("AStar/AStarBuildEditor")]
    public static void Window()
    {
        m_window = GetWindow(typeof(AStarBuildEditor));
        m_grid = LoadPathGrid();
        if (m_grid == null)
        {
            GameObject obj = new GameObject(typeof(AGrid).ToString(), typeof(AGrid));
            m_grid = obj.GetComponent<AGrid>();
        }
    }
    private void OnGUI()
    {
        Vector2 windowSize = new Vector2(0, 0);

        SetGrid(ref windowSize);
        SetWindowSize(windowSize);
    }
    private void OnDestroy()
    {
        DestroyImmediate(m_grid.gameObject);
    }
    void SetGrid(ref Vector2 size)
    {
        Vector2 uiSize = new Vector2(400, 20);
        size.y += 10;

        m_grid.WorldSize = EditorGUI.Vector2Field(new Rect(size, uiSize), "WorldSize", m_grid.WorldSize);
        size.y += 40;

        EditorGUI.LabelField(new Rect(size.x, size.y, uiSize.x * 0.4f, uiSize.y), "NodeRadius");
        m_grid.NodeRadius = EditorGUI.FloatField(new Rect(uiSize .x * 0.4f, size.y, uiSize.x* 0.6f, uiSize.y), m_grid.NodeRadius);
        size.y += 40;

        if (m_grid.WorldSize != Vector2.zero && m_grid.NodeRadius > 0.1f)
        {
            if (GUI.Button(new Rect(size.x, size.y, uiSize.x, 20), "Create Grid"))
                m_grid.CreateGrid();

            size.y += 25;

            if (m_grid.Nodes != null)
            {
                if (GUI.Button(new Rect(size.x, size.y, uiSize.x, 20), "Test PathFinding"))
                    AStarTestEditor.Window(m_grid);
                
                size.y += 25;
            }

            if (GUI.Button(new Rect(size.x, size.y, uiSize.x, 20), "Build Grid"))
                BuildPathGrid();

            size.y += 25;
        }
    }
    void SetWindowSize(Vector2 size)
    {
        size.x = 400;
        if(m_window.position.size != size)
        m_window.position = new Rect(m_window.position.position, size);
    }
    static AGrid LoadPathGrid()
    {
        string path = "Assets/AStarBuildArray.astar";

        if (!File.Exists(path))
            return null;

        TextAsset asset = new TextAsset(File.ReadAllText(path));
        string[] value = asset.text.Split('_');
        GameObject obj = new GameObject(typeof(AGrid).ToString(), typeof(AGrid));
        AGrid grid = obj.GetComponent<AGrid>();
        string[] arr = value[0].Split(',');
        grid.NodeRadius = float.Parse(arr[0]);
        grid.WorldSize.x = float.Parse(arr[1]);
        grid.WorldSize.y = float.Parse(arr[2]);
        grid.CreateGrid(value);
        return grid;
    }
    void BuildPathGrid()
    {
        string path = "Assets/AStarBuildArray.astar";
        FileStream file = new FileStream(path, FileMode.Create);
        string[] nodes = new string[m_grid.Nodes.Length+1];

        int i = 1;
        nodes[0] = m_grid.NodeRadius + "," + m_grid.WorldSize.x + "," + m_grid.WorldSize.y;
        foreach(ANode node in m_grid.Nodes)
        {
            string arr = node.GetGridX + "," + node.GetGridY + "," + node.IsWalkable + "," + node.Position.x + "," + node.Position.y;
            nodes[i] = arr;
            ++i;
        }
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(string.Join("_", nodes));
        file.Write(bytes, 0, bytes.Length);
        file.Close();
    }
}
