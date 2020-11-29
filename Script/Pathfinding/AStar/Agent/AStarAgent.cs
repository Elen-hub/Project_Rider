using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AStarAgent : MonoBehaviour
{
    public enum EAgentState
    {
        Standby,
        Wait,
        Move,
    }
    public EAgentState State { get { return m_state; } }
    [SerializeField]
    EAgentState m_state;
    Stack<Vector3> m_path;
    public Stack<Vector3> GetPath { get { return m_path; } }
    AStarPathfinding m_pathFinding;
    public float Speed { set { m_speed = value; } }
    [SerializeField]
    float m_speed = 3;
    Coroutine m_currCoroutine;
    [SerializeField]
    Vector2 m_velocity;
    public Vector2 Velocity {
        get { return m_velocity; }
    }
    public ANode GetNodeToPosition(Vector3 pos)
    {
        return AStarMng.Instance.GetNode(pos);
    }
    public bool IsPossibleMoveToPosition(Vector3 pos)
    {
        ANode node = GetNodeToPosition(pos);
        if (node == null)
            return false;

        return node.IsWalkable;
    }
    public void SetDestination(Vector3 position)
    {
        if(m_currCoroutine != null)
            StopCoroutine(m_currCoroutine);

        m_state = EAgentState.Wait;
        AStarMng.Instance.RequestPathfinding(this, transform.position, position);
    }
    public void Stop()
    {
        m_velocity = Vector2.zero;
        m_state = EAgentState.Standby;
    }
    public void PathFindingCallback(Stack<Vector3> path)
    {
        m_path = path;
        m_currCoroutine = StartCoroutine(IEDestination());
    }
    IEnumerator IEDestination()
    {
        m_state = EAgentState.Move;
        Vector3 currPos = m_path.Pop();
        int count = 0;
        while (m_path.Count > 1)
        {
            yield return null;
            if (transform.position == currPos)
            {
                currPos = m_path.Pop();
                ++count;
            }

            Vector3 prev = transform.position;
            Vector3 next = Vector3.MoveTowards(prev, currPos, m_speed * Time.deltaTime);
            m_velocity = next - prev;
            transform.position = next;
        }
        m_currCoroutine = null;
        m_velocity = Vector2.zero;
        m_state = EAgentState.Standby;
    }
#if UNITY_EDITOR
    //private void OnDrawGizmos()
    //{
    //    if (m_path != null && m_path.Count > 1)
    //    {
    //        Vector3[] array = m_path.ToArray();
    //        Gizmos.color = Color.black;
    //        for (int i = 0; i < array.Length; ++i)
    //            Gizmos.DrawCube(array[i], Vector3.one * 20);
    //    }
    //}
#endif
}
