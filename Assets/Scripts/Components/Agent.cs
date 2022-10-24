using System;
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

    bool m_IsBlown = false;
    bool m_Dead = false;
    bool m_HasKey = false;

    public bool IsBlown => m_IsBlown;
    public bool HasKey => m_HasKey;

    Fan m_Fan;

    [Header("Status")]
    [SerializeField] StatusHandler m_StatusHandler = new StatusHandler();
    [SerializeField] FrozenStatus m_FrozenStatus;
    public Status Status => m_StatusHandler.CurrentStatus;

    [Header("Spatial Properties")]
    [SerializeField] Tilemap m_Tilemap;

    // Temporary for testing purposes
    [SerializeField] GameObject m_Target;

    [Header("Movement Properties")]
    [SerializeField] float m_DefaultSpeed;
    float m_CurrentSpeed;

    [Header("Planner")]
    [SerializeField] GOAP.Blackboard m_WorldState = new GOAP.Blackboard();
    public GOAP.Blackboard WorldState => m_WorldState;

    GOAP.Plan m_CurrentPlan;
    GOAP.Planner m_Planner;
    GOAP.WorkingMemory m_WorkingMemory;

    public float DefaultSpeed => m_DefaultSpeed;
    public float Speed => m_CurrentSpeed;

    void Awake()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();


        m_Planner = new GOAP.Planner(this);

        m_CurrentSpeed = m_DefaultSpeed;

        Debug.Assert(m_SpriteRenderer != null, "Agent does not have a sprite renderer!");
    }

    void Start()
    {
        Debug.Assert(m_Tilemap != null, "Agent has no assigned tilemap!");

        m_Pathfinding.Initialize(m_Tilemap);
    }

    public void Update()
    {
        if(m_Dead)
        {
            if(m_Dead) Debug.Log("Dead");
            // Here lerp bot to center and then blow it
            return;
        }

        m_StatusHandler.Update(this);

        if (Input.GetKeyDown(KeyCode.Space) && m_CurrentPath == null)
        {
            m_CurrentPath = m_Pathfinding.FindPath(transform.position, m_Target.transform.position);
        }

        if(m_CurrentPath != null && !m_CurrentPath.Completed)
        {
            transform.position = m_CurrentPath.Update(this);
        }
    }

    public void ApplyStatus(Status newStatus)
    {
        m_StatusHandler.QueueStatus(newStatus);

        // Status effect is null when a "bad" transition occurs e.g. Wet -> Wet / Frozen -> Wet
        //StatusEffect statusEffect = m_StatusHandler.TransitionTo(newStatus);

        //if(statusEffect != null)
        //{
        //    statusEffect.Apply(this);
        //}
    }

    public void SetSprite(Sprite sprite)
    {
        m_SpriteRenderer.sprite = sprite;
    }

    public void SetSpeed(float newSpeed)
    {
        m_CurrentSpeed = newSpeed;
    }

    public void SetIsBlown(bool isBlown)
    {
        m_IsBlown = isBlown;
    }

    public void SetFan(Fan fan)
    {
        m_Fan = fan;
    }

    public void Die(Vector3 deathPosition, Sprite deathSprite)
    {
        m_Dead = true;
        m_IsBlown = false;
        m_Rigidbody2D.simulated = false;
        m_Rigidbody2D.isKinematic = true;

        Debug.Log("Death position " + deathPosition);

        StopAllCoroutines();

        SetCoroutine(DyingCoroutine(transform.position, deathPosition, deathSprite));
    }

    IEnumerator DyingCoroutine(Vector3 currentPosition, Vector3 nextPosition, Sprite deathSprite)
    {
        float totalTime = (currentPosition - nextPosition).magnitude / Speed;
        float t = 0;

        while (t < totalTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Speed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Truly dead");
        m_SpriteRenderer.sprite = deathSprite;
    }

    // Test only remove later if dont need
    Coroutine m_Coroutine;

    public void SetCoroutine(IEnumerator coroutine)
    {
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
        }

        m_Coroutine = StartCoroutine(coroutine);
    }

    public void Stop()
    {
        m_CurrentPath = null;
    }

    public void Move()
    {
        m_CurrentPath = m_Pathfinding.FindPath(transform.position, m_Target.transform.position);
    }

    public void PickupKey()
    {
        m_HasKey = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && m_IsBlown)
        {
            m_IsBlown = false;
            m_Rigidbody2D.isKinematic = true;
            m_Fan?.gameObject.SetActive(false);
            m_CurrentPath = m_Pathfinding.FindPath(transform.position, m_Target.transform.position);
            ApplyStatus(m_FrozenStatus);
        }
    }
}