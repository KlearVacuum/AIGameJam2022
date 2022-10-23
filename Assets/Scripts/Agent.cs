﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Agent : MonoBehaviour
{
    Path m_CurrentPath;

    Pathfinding m_Pathfinding = new Pathfinding();
    SpriteRenderer m_SpriteRenderer;
    Rigidbody2D m_Rigidbody2D;

    [Header("Status")]
    [SerializeField] StatusHandler m_StatusHandler = new StatusHandler();
    [SerializeField] FrozenStatus m_FrozenStatus;
 
    [Header("Spatial Properties")]
    [SerializeField] Tilemap m_Tilemap;

    // Temporary for testing purposes
    [SerializeField] GameObject m_Target;

    [Header("Movement Properties")]
    [SerializeField] float m_DefaultSpeed;
    float m_CurrentSpeed;

    public float DefaultSpeed => m_DefaultSpeed;
    public float Speed => m_CurrentSpeed;

    void Awake()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        m_CurrentSpeed = m_DefaultSpeed;

        Debug.Assert(m_SpriteRenderer != null, "Agent does not have a sprite renderer!");
    }

    void Start()
    {
        Debug.Assert(m_Tilemap != null, "Agent has no assigned tilemap!");

        m_Pathfinding.Initialize(m_Tilemap);
        m_CurrentPath = m_Pathfinding.FindPath(transform.position, m_Target.transform.position);

        Debug.Assert(m_CurrentPath != null);
    }

    public void Update()
    {
        if(m_StatusHandler.CurrentStatus is BlownStatus)
        {
            // Here lerp bot to center and then blow it
            return;
        }

        if(m_CurrentPath != null && !m_CurrentPath.Completed)
        {
            transform.position = m_CurrentPath.Update(this);
        }
    }

    public void SetStatus(Status newStatus)
    {
        // Status effect is null when a "bad" transition occurs e.g. Wet -> Wet / Frozen -> Wet
        StatusEffect statusEffect = m_StatusHandler.TransitionTo(newStatus);

        if(statusEffect != null)
        {
            statusEffect.Apply(this);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        m_SpriteRenderer.sprite = sprite;
    }

    public void SetSpeed(float newSpeed)
    {
        m_CurrentSpeed = newSpeed;
    }

    // Test only remove later if dont need
    Coroutine m_Coroutine;

    public void SetCoroutine(IEnumerator coroutine)
    {
        if(m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
        }

        m_Coroutine = StartCoroutine(coroutine);
    }

    public void ClearPath()
    {
        m_CurrentPath = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") &&
           m_StatusHandler.CurrentStatus is BlownStatus)
        {
            m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            SetStatus(m_FrozenStatus);
        }
    }
}