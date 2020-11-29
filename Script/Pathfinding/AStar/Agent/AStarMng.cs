using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

struct ARequestHandler
{
    public AStarAgent Agent;
    public Vector3 Start;
    public Vector3 End;
   
    public ARequestHandler(AStarAgent agent, Vector3 start, Vector3 end)
    {
        Agent = agent;
        Start = start;
        End = end;
    }
}
public class AStarMng : TSingleton<AStarMng>
{
    Queue<ARequestHandler> m_requestQueue = new Queue<ARequestHandler>();
    ARequestHandler m_currentHandler;
    // 경로를 계산하는 클래스
    AStarPathfinding m_pathFinding;
    bool m_isDone;
    public AGrid Grid { set { m_pathFinding.Grid = value; } }
    public ANode GetNode(Vector3 position)
    {
        return m_pathFinding.GetNode(position);
    }
    // 큐에 요청받은 내용을 구조체를 통하여 입력
    public void RequestPathfinding(AStarAgent agent, Vector3 start, Vector3 end)
    {
        ARequestHandler handler = new ARequestHandler(agent, start, end);
        m_requestQueue.Enqueue(handler);
        NextProcess();
    }
    // 경로추적이 완료되면 결과를 받고 해당객체에 알리고 다음프로세스 진행
    public void CompleteProcess(Stack<Vector3> path)
    {
        m_currentHandler.Agent.PathFindingCallback(path);
        m_isDone = false;
        NextProcess();
    }
    // 프로세스 진행
    void NextProcess()
    {
        if(!m_isDone && m_requestQueue.Count > 0)
        {
            m_isDone = true;
            m_currentHandler = m_requestQueue.Dequeue();
            StartCoroutine(m_pathFinding.SearchPath(m_currentHandler.Start, m_currentHandler.End));
        }
    }
    // 초기화
    public void Init()
    {
        m_pathFinding = new AStarPathfinding();

        TextAsset asset = Resources.Load<TextAsset>("AStarBuildArray");
        string[] value = asset.text.Split('_');
        GameObject obj = new GameObject(typeof(AGrid).ToString(), typeof(AGrid));
        AGrid grid = obj.GetComponent<AGrid>();
        string[] arr = value[0].Split(',');
        grid.NodeRadius = float.Parse(arr[0]);
        grid.WorldSize.x = float.Parse(arr[1]);
        grid.WorldSize.y = float.Parse(arr[2]);
        grid.CreateGrid(value);
        Grid = grid;
    }
}

