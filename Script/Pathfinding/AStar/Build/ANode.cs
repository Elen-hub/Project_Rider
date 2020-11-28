using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANode
{
    public ANode PrevNode { get; set; }
    public bool IsWalkable { get; set; }
    public Vector3 Position { get; set; }
    int m_gridX;
    int m_gridY;
    public int GetGridX { get { return m_gridX; } }
    public int GetGridY { get { return m_gridY; } }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int GetFCost {  get { return GCost + HCost; }  }
    public ANode(bool isWalkable, Vector3 position, int gridX, int gridY)
    {
        IsWalkable = isWalkable;
        Position = position;
        m_gridX = gridX;
        m_gridY = gridY;
    }
}
