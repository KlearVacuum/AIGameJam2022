using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Agent : MonoBehaviour
{
    Path m_CurrentPath;

    Pathfinding m_Pathfinding = new Pathfinding();
    SpriteRenderer m_SpriteRenderer;

    [Header("Status")]
    [SerializeField] StatusHandler m_StatusHandler = new StatusHandler();

    [Header("Spatial Properties")]
    [SerializeField] Tilemap m_Tilemap;
    // Temporary for testing purposes
    [SerializeField] GameObject m_Target;

    [Header("Movement Properties")]
    [SerializeField] float m_Speed;
    public float Speed => m_Speed;

    void Awake()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Debug.Assert(m_SpriteRenderer != null, "Agent does not have a sprite renderer!");
    }

    void Start()
    {
        Debug.Assert(m_Tilemap != null, "Agent has no assigned tilemap!");

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

    public void SetStatus(Status newStatus)
    {
        StatusEffect statusEffect = m_StatusHandler.TransitionTo(newStatus);

        statusEffect.Apply(this);
    }

    public void SetSprite(Sprite sprite)
    {
        m_SpriteRenderer.sprite = sprite;
    }
}