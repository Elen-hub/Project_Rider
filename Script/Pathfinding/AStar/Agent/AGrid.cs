using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AGrid : MonoBehaviour
{
    public Vector3 WorldSize = Vector3.zero;
    public float NodeRadius = 0;

    public ANode[,] Nodes;
    int m_gridSizeX, m_gridSizeY, m_gridSizeZ;

    #region CraeteGrid Overloading
    // 에디터에서 그리드를 생성하는 함수
    public void CreateGrid()
    {
        m_gridSizeX = Mathf.RoundToInt(WorldSize.x / (NodeRadius * 2));
        m_gridSizeY = Mathf.RoundToInt(WorldSize.y);
        m_gridSizeZ = Mathf.RoundToInt(WorldSize.z / (NodeRadius * 2));

        Nodes = new ANode[m_gridSizeX, m_gridSizeZ];
        Vector3 bottomLeft = Vector3.left * WorldSize.x * 0.5f - Vector3.forward * WorldSize.z * 0.5f;
        Vector3 pivot;
        LayerMask walkableLayer = LayerMask.NameToLayer("Track");
        RaycastHit hit;
        for (int i = 0; i< m_gridSizeX; ++i)
        {
            for(int j =0; j<m_gridSizeZ; ++j)
            {
                pivot = bottomLeft + Vector3.right * (i * NodeRadius * 2 + NodeRadius) + Vector3.forward * (j * NodeRadius * 2 + NodeRadius);
                pivot.y = m_gridSizeY * 0.5f;
                bool isWalkable = Physics.Raycast(pivot, Vector3.down, out hit, m_gridSizeY, 1 << walkableLayer);
                Nodes[i, j] = new ANode(isWalkable, isWalkable ? hit.point : new Vector3(pivot.x, 0, pivot.z), i, j);
            }
        }
        for(int i =0; i<m_gridSizeX; ++i)
        {
            for(int j = 0; j<m_gridSizeZ; ++j)
            {
                ANode node = Nodes[i, j];
                if (!node.IsWalkable)
                    continue;
                bool iPossible = false;
                bool jPossible = false;
                if (i > 0 && Nodes[i - 1, j].IsWalkable)
                {
                    float cal = node.Position.y - Nodes[i - 1, j].Position.y;
                    iPossible = cal <= AStarMng.MaxMoveHeight * 1f && cal >= -AStarMng.MaxMoveHeight * 1f;
                }
                if (!iPossible && i < m_gridSizeX && Nodes[i + 1, j].IsWalkable)
                {
                    float cal = node.Position.y - Nodes[i + 1, j].Position.y;
                    iPossible = cal <= AStarMng.MaxMoveHeight * 1f && cal >= -AStarMng.MaxMoveHeight * 1f;
                }
                if (j > 0 && Nodes[i, j - 1].IsWalkable)
                {
                    float cal = node.Position.y - Nodes[i, j - 1].Position.y;
                    jPossible = cal <= AStarMng.MaxMoveHeight * 1f && cal >= -AStarMng.MaxMoveHeight * 1f;
                }
                if (!jPossible && j < m_gridSizeZ && Nodes[i, j + 1].IsWalkable)
                {
                    float cal = node.Position.y - Nodes[i, j + 1].Position.y;
                    jPossible = cal <= AStarMng.MaxMoveHeight * 1f && cal >= -AStarMng.MaxMoveHeight * 1f;
                }
                node.IsWalkable = iPossible && jPossible;
            }
        }
    }
    // 게임중 빌드된 노드를 입력받았을때 이를 노드배열로 바꿔주는 함수
    public void CreateGrid(string[] node)
    {
        m_gridSizeX = Mathf.RoundToInt(WorldSize.x / (NodeRadius * 2));
        m_gridSizeZ = Mathf.RoundToInt(WorldSize.z / (NodeRadius * 2));
        Nodes = new ANode[m_gridSizeX, m_gridSizeZ];
        for (int n = 1; n<node.Length; ++n)
        {
            string[] value = node[n].Split(',');
            int x = int.Parse(value[0]);
            int z = int.Parse(value[1]);
            bool isWalkable = value[2] == "True";
            Vector3 pos = new Vector3(float.Parse(value[3]), float.Parse(value[4]), float.Parse(value[5]));
            Nodes[x, z] = new ANode(isWalkable, pos, x, z);
            Nodes[x, z].Speed = float.Parse(value[6]);
            Nodes[x, z].Friction = float.Parse(value[7]);
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

                if (x >= 0 && x < m_gridSizeX && y >= 0 && y < m_gridSizeZ)
                    neighbours.Add(Nodes[x, y]);
            }
        }
        return neighbours;
    }
    // 해당 위치의 노드를 찾는 함수
    public ANode GetNode(Vector3 position)
    {
        float percentX = Mathf.Clamp01((position.x + WorldSize.x * 0.5f) / WorldSize.x);
        float percentZ = Mathf.Clamp01((position.z + WorldSize.z * 0.5f) / WorldSize.z);
        int x = Mathf.RoundToInt((m_gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((m_gridSizeZ - 1) * percentZ);
        return Nodes[x, z];
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Color redColor = Color.red;
        redColor.a = 0.5f;
        Color blueColor = Color.blue;
        blueColor.a = 0.5f;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(WorldSize.x, WorldSize.y, WorldSize.z));
        if (Nodes != null)
        {
            // Gizmos.color = blueColor;
            Vector3 size = Vector3.zero;
            foreach (ANode node in Nodes)
            {
                //if (node.IsWalkable)
                //{
                //    size.x = NodeRadius * 2;
                //    size.z = NodeRadius * 2;
                //    Gizmos.DrawCube(node.Position, size);
                //}
                Gizmos.color = node.IsWalkable ? blueColor : redColor;
                size.x = NodeRadius * 2;
                size.z = NodeRadius * 2;
                Gizmos.DrawCube(node.Position, size);
            }
        }
    }
#endif
}
