using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Agent : MonoBehaviour
{
    Path m_CurrentPath;
    Queue<Path.Request> m_PathPlanningRequests = new Queue<Path.Request>();

    Pathfinding m_Pathfinding = new Pathfinding();
    SpriteRenderer m_SpriteRenderer;
    Rigidbody2D m_Rigidbody2D;

    [Header("Audio")]
    [HideInInspector] public AudioSource aSource;
    public AudioClipGroup audioZap, audioZapDie, audioWet, audioSteam, audioBurn, audioBlow, 
                            audioFreeze, audioUnlock, audioGetKey, audioWallCrash;

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
    [SerializeField] GOAP.Planner m_Planner;
    [SerializeField] GOAP.Blackboard m_WorldState = new GOAP.Blackboard();
    public GOAP.Blackboard WorldState => m_WorldState;

    GOAP.WorkingMemory m_WorkingMemory;

    public float DefaultSpeed => m_DefaultSpeed;
    public float Speed => m_CurrentSpeed;

    void Awake()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        aSource = GetComponent<AudioSource>();

        Debug.Assert(m_SpriteRenderer != null, "Agent does not have a sprite renderer!");

        m_CurrentSpeed = m_DefaultSpeed;
        m_Planner.Initialize(this);
        m_WorldState.Initialize();
    }

    void Start()
    {
        Debug.Assert(m_Tilemap != null, "Agent has no assigned tilemap!");
        m_Pathfinding.Initialize(m_Tilemap);

        Dictionary<string, GOAP.IStateValue> desiredState = new Dictionary<string, GOAP.IStateValue>();
        desiredState.Add("HasKey", new GOAP.StateValue<bool>(true));

        m_Planner.AddPlanRequest(desiredState);
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

        if(m_IsBlown)
        {
            return;
        }

        GOAP.Plan currentPlan = m_Planner.GetCurrentPlan();

        if(currentPlan != null)
        {
            GOAP.Action currentAction = currentPlan.GetCurrentAction();

            if (currentAction.GetStatus() == GOAP.Action.ExecutionStatus.Executing ||
                currentAction.GetStatus() == GOAP.Action.ExecutionStatus.None)
            {
                // Debug.Log($"Executing {currentAction.GetName()}");
            }

            currentPlan.Execute(this);
        }
    }

    public Path GetCurrentPath()
    {
        EvaluateCurrentPath();

        if(m_PathPlanningRequests.Count > 0)
        {
            Path.Request pathRequest = m_PathPlanningRequests.Dequeue();

            m_CurrentPath = m_Pathfinding.FindPath(
                transform.position, 
                pathRequest.TargetPosition, 
                pathRequest.PathCompleteAction);
        }

        return m_CurrentPath;
    }

    public void AddPathRequest(Vector3 targetPosition, Action<Agent> pathCompleteAction)
    {
        m_PathPlanningRequests.Enqueue(new Path.Request(targetPosition, pathCompleteAction));
    }

    private void EvaluateCurrentPath()
    {
        if(m_CurrentPath == null)
        {
            return;
        }

        if(m_CurrentPath.Completed)
        {
            m_CurrentPath = null;
        }
    }

    public void AddPlanRequest(Dictionary<string, GOAP.IStateValue> desiredState)
    {
        m_Planner.AddPlanRequest(desiredState);
    }

    public void ApplyStatus(Status newStatus)
    {
        m_StatusHandler.QueueStatus(newStatus);
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

    public void Die(Vector3 deathPosition, Sprite deathSprite, AudioClipGroup deathSound = null)
    {
        m_Dead = true;
        m_IsBlown = false;
        m_Rigidbody2D.simulated = false;
        m_Rigidbody2D.isKinematic = true;

        Debug.Log("Death position " + deathPosition);

        StopAllCoroutines();

        SetCoroutine(DyingCoroutine(transform.position, deathPosition, deathSprite, deathSound));
    }

    IEnumerator DyingCoroutine(Vector3 currentPosition, Vector3 nextPosition, Sprite deathSprite, AudioClipGroup audio = null)
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

        if (audio != null)
            audio.PlayOneShot(aSource);
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

            // Reevaluate path based on old path
            m_CurrentPath = m_Pathfinding.FindPath(
                transform.position, 
                m_CurrentPath.TargetPosition, 
                m_CurrentPath.PathCompleteAction);

            audioWallCrash.PlayOneShot(aSource);
            ApplyStatus(m_FrozenStatus);
        }
    }
}