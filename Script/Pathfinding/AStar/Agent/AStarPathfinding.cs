using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    AGrid m_buildGrid;
    ANode m_startNode;
    ANode m_endNode;
    const int m_straightCost = 10;
    const int m_diagonalCost = 14;

    public AGrid Grid
    {
        set { m_buildGrid = value; }
    }
    // 노드를 추적하여 Stack에 담아 리턴 
    Stack<Vector3> GetPath(Vector3 startPos, Vector3 targetPos)
    {
        SearchPath(startPos, targetPos);

        ANode current = m_endNode;
        Stack<Vector3> path = new Stack<Vector3>();
        while (current != m_startNode)
        {
            path.Push(current.Position);
            current = current.PrevNode;
        }
        return path;
    }
    // 노드를 기반으로 선형연결리스트처럼 노드를 이어줌 
    public IEnumerator SearchPath(Vector3 startPos, Vector3 endPos)
    {
        yield return null;

        bool isSuccess = false;
        m_startNode = m_buildGrid.GetNode(startPos);
        m_endNode = m_buildGrid.GetNode(endPos);

        if (m_startNode.IsWalkable && m_endNode.IsWalkable)
        {
            List<ANode> openNode = new List<ANode>();
            HashSet<ANode> closeNode = new HashSet<ANode>();
            openNode.Add(m_startNode);
            while (openNode.Count > 0)
            {
                ANode current = openNode[0];

                for (int i = 1; i < openNode.Count; ++i)
                    if (openNode[i].GetFCost < current.GetFCost || openNode[i].GetFCost == current.GetFCost && openNode[i].HCost < current.HCost)
                        current = openNode[i];

                openNode.Remove(current);
                closeNode.Add(current);

                if (current == m_endNode)
                {
                    isSuccess = true;
                    break;
                }

                foreach (ANode node in m_buildGrid.GetNeighbourNode(current))
                {
                    if (!node.IsWalkable || closeNode.Contains(node))
                        continue;

                    int currentNeighbourCost = current.GCost + GetCost(node, m_endNode);
                    if (currentNeighbourCost < node.GCost || !openNode.Contains(node))
                    {
                        node.GCost = currentNeighbourCost;
                        node.HCost = GetCost(node, m_endNode);
                        node.PrevNode = current;
                        if (!openNode.Contains(node))
                            openNode.Add(node);
                    }
                }
            }
        }
        AStarMng.Instance.CompleteProcess(isSuccess ? GetPath(startPos, endPos) : null);
    }
    int GetCost(ANode a, ANode b)
    {
        int x = Mathf.Abs(a.GetGridX - b.GetGridX);
        int y = Mathf.Abs(a.GetGridY - b.GetGridY);
        return x > y ? m_diagonalCost * y + m_straightCost * (x - y) : m_diagonalCost * x + m_straightCost * (y - x);
    }
    public Stack<Vector3> TestSearch(Vector3 startPos, Vector3 endPos)
    {
        m_startNode = m_buildGrid.GetNode(startPos);
        m_endNode = m_buildGrid.GetNode(endPos);

        if (m_startNode.IsWalkable && m_endNode.IsWalkable)
        {
            List<ANode> openNode = new List<ANode>();
            HashSet<ANode> closeNode = new HashSet<ANode>();
            openNode.Add(m_startNode);
            while (openNode.Count > 0)
            {
                ANode current = openNode[0];

                for (int i = 1; i < openNode.Count; ++i)
                    if (openNode[i].GetFCost < current.GetFCost || openNode[i].GetFCost == current.GetFCost && openNode[i].HCost < current.HCost)
                        current = openNode[i];

                openNode.Remove(current);
                closeNode.Add(current);

                if (current == m_endNode)
                    break;

                foreach (ANode node in m_buildGrid.GetNeighbourNode(current))
                {
                    if (!node.IsWalkable || closeNode.Contains(node))
                        continue;

                    int currentNeighbourCost = current.GCost + GetCost(node, m_endNode);
                    if (currentNeighbourCost < node.GCost || !openNode.Contains(node))
                    {
                        node.GCost = currentNeighbourCost;
                        node.HCost = GetCost(node, m_endNode);
                        node.PrevNode = current;
                        if (!openNode.Contains(node))
                            openNode.Add(node);
                    }
                }
            }
        }
        return GetPath(startPos, endPos);
    }
}
