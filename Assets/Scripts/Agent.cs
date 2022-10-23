using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class Agent : MonoBehaviour
{

    [SerializeField] Tilemap m_Tilemap;
    [SerializeField] GameObject m_Target;


    [SerializeField] float m_Speed;
    public float Speed => m_Speed;

    Pathfinding m_Pathfinding;
    Path m_CurrentPath;

    void Start()
    {
        Debug.Assert(m_Tilemap != null, "Agent has no assigned tilemap!");

        m_Pathfinding = new Pathfinding();
        m_Pathfinding.Initialize(m_Tilemap);

        m_CurrentPath = m_Pathfinding.FindPath(transform.position, m_Target.transform.position);
    }

    public void Update()
    {
        if(m_CurrentPath != null && !m_CurrentPath.Completed)
        {
            transform.position = m_CurrentPath.Update(this);
        }
    }
}