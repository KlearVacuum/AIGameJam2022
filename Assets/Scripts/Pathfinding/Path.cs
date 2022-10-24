using UnityEngine;
using System;
using System.Collections.Generic;

public class Path
{
    public class Request
    {
        Vector3 m_TargetPosition;
        public Vector3 TargetPosition => m_TargetPosition;

        Action<Agent> m_PathCompleteAction;

        public Action<Agent> PathCompleteAction => m_PathCompleteAction;

        public Request(Vector3 targetPosition, Action<Agent> pathCompleteAction)
        {
            m_TargetPosition = targetPosition;
            m_PathCompleteAction = pathCompleteAction;
        }
    }

    public class Node
    {
        private Vector3 m_Position;
        public Vector3 Position => m_Position;

        public Node(Vector3 position)
        {
            m_Position = position;
        }
    }

    Vector3 m_TargetPosition;
    public Vector3 TargetPosition => m_TargetPosition;

    Action<Agent> m_PathCompleteAction;

    public Action<Agent> PathCompleteAction => m_PathCompleteAction;

    List<Node> m_Nodes;
    int m_CurrentNodeIndex = 0;
    public bool Completed => m_CurrentNodeIndex >= m_Nodes.Count;

    public Path(List<Node> nodes, Vector3 targetPosition, Action<Agent> pathCompleteAction)
    {
        m_Nodes = nodes;
        m_TargetPosition = targetPosition;
        m_PathCompleteAction = pathCompleteAction;
    }

    public Node GetCurrentNode()
    {
        Node currentNode = null;

        if(Completed == false)
        {
            currentNode = m_Nodes[m_CurrentNodeIndex];
        }

        return currentNode;
    }

    public Vector3 Update (Agent agent)
    {
        Vector3 agentPosition = agent.transform.position;
        Node currentNode = GetCurrentNode();

        if(currentNode != null)
        {
            agentPosition = Vector3.MoveTowards(agentPosition, currentNode.Position, agent.Speed * Time.deltaTime);

            Debug.DrawLine(agentPosition, currentNode.Position, Color.red, 1f);

            // TODO: Change threshold value to be settable
            if ((agentPosition - currentNode.Position).sqrMagnitude <= 0.05f)
            {
                Advance();

                if(Completed)
                {
                    m_PathCompleteAction(agent);
                }
            }
        }

        return agentPosition;
    }

    public void Advance()
    {
        ++m_CurrentNodeIndex;
    }
}