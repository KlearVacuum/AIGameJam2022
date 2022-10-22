using UnityEngine;
using System.Collections.Generic;

class Path
{
    public class Node
    {
        private Vector3Int m_Position;
        public Vector3Int Position => m_Position;

        public Node(Vector3Int position)
        {
            m_Position = position;
        }
    }

    List<Node> m_Nodes;
    int m_CurrentNodeIndex = 0;
    public bool Completed => m_CurrentNodeIndex >= m_Nodes.Count;

    public Path(List<Node> nodes)
    {
        m_Nodes = nodes;
    }

    public Node GetCurrentNode()
    {
        Debug.Assert(m_CurrentNodeIndex < m_Nodes.Count, "Invalid node index");
        return m_Nodes[m_CurrentNodeIndex];
    }

    public void Advance()
    {
        ++m_CurrentNodeIndex; 
    }
}