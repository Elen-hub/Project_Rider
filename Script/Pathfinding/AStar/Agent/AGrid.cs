using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AGrid : MonoBehaviour
{
    public Vector2 WorldSize = Vector2.zero;
    public float NodeRadius = 0;

    public ANode[,] Nodes;
    int m_gridSizeX, m_gridSizeY;

    #region CraeteGrid Overloading
    // 에디터에서 그리드를 생성하는 함수
    public void CreateGrid()
    {
        m_gridSizeX = Mathf.RoundToInt(WorldSize.x / (NodeRadius * 2));
        m_gridSizeY = Mathf.RoundToInt(WorldSize.y / (NodeRadius * 2));

        Nodes = new ANode[m_gridSizeX, m_gridSizeY];
        Vector3 bottomLeft = Vector3.left * WorldSize.x * 0.5f - Vector3.forward * WorldSize.y * 0.5f;
        Vector3 pivot;
        LayerMask walkableLayer = LayerMask.NameToLayer("Track");
        for (int i = 0; i< m_gridSizeX; ++i)
        {
            for(int j =0; j<m_gridSizeY; ++j)
            {
                pivot = bottomLeft + Vector3.right * (i * NodeRadius * 2 + NodeRadius) + Vector3.forward * (j * NodeRadius * 2 + NodeRadius);
                bool isWalkable = Physics.CheckSphere(pivot, NodeRadius, 1 << walkableLayer);
                Nodes[i, j] = new ANode(isWalkable, pivot, i, j);
            }
        }
    }
    // 게임중 빌드된 노드를 입력받았을때 이를 노드배열로 바꿔주는 함수
    public void CreateGrid(string[] node)
    {
        m_gridSizeX = Mathf.RoundToInt(WorldSize.x / (NodeRadius * 2));
        m_gridSizeY = Mathf.RoundToInt(WorldSize.y / (NodeRadius * 2));
        Nodes = new ANode[m_gridSizeX, m_gridSizeY];
        for (int n = 1; n<node.Length; ++n)
        {
            string[] value = node[n].Split(',');
            int x = int.Parse(value[0]);
            int y = int.Parse(value[1]);
            bool isWalkable = value[2] == "True";
            Vector3 pos = new Vector3(float.Parse(value[3]), 0, float.Parse(value[4]));
            Nodes[x, y] = new ANode(isWalkable, pos, x, y);
            Nodes[x, y].Speed = float.Parse(value[5]);
            Nodes[x, y].Friction = float.Parse(value[6]);
        }
    }
    #endregion
    // 임의의 노드의 이웃한 노드들을 받아오는 함수
    public List<ANode> GetNeighbourNode(ANode node)
    {
        List<ANode> neighbours = new List<ANode>();
        for(int i = -1; i <= 1; ++i)
        {
            for(int j = -1; j <=1; ++j)
            {
                if (i == 0 && j == 0)
                    continue;

                int x = node.GetGridX + i;
                int y = node.GetGridY + j;

                if (x >= 0 && x < m_gridSizeX && y >= 0 && y < m_gridSizeY)
                    neighbours.Add(Nodes[x, y]);
            }
        }
        return neighbours;
    }
    // 해당 위치의 노드를 찾는 함수
    public ANode GetNode(Vector3 position)
    {
        float percentX = Mathf.Clamp01((position.x + WorldSize.x * 0.5f) / WorldSize.x);
        float percentY = Mathf.Clamp01((position.z + WorldSize.y * 0.5f) / WorldSize.y);
        int x = Mathf.RoundToInt((m_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((m_gridSizeY - 1) * percentY);
        return Nodes[x, y];
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Color redColor = Color.red;
        redColor.a = 0.5f;
        Color blueColor = Color.blue;
        blueColor.a = 0.5f;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(WorldSize.x, 0, WorldSize.y));
        if (Nodes != null)
        {
            foreach (ANode node in Nodes)
            {
                Gizmos.color = node.IsWalkable ? blueColor : redColor;
                Gizmos.DrawCube(node.Position, Vector3.one * (NodeRadius * 2));
            }
        }
    }
#endif
}
