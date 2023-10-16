using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.Rendering.Universal;

public class Agent : MonoBehaviour
{
    Path m_CurrentPath;
    Queue<Path.Request> m_PathPlanningRequests = new Queue<Path.Request>();

    GameManager gameManager;
    Pathfinding m_Pathfinding = new Pathfinding();
    SpriteRenderer m_SpriteRenderer;
    Rigidbody2D m_Rigidbody2D;
    Animator m_Animator;
    Light2D m_GlowLight;
    
    public SpriteRenderer deadSpriteRenderer;

    [Header("Audio")]
    [HideInInspector] public AudioSource aSource;
    public AudioClipGroup audioZap, audioZapDie, audioWet, audioSteam, audioBurn, audioBlow, 
                            audioFreeze, audioUnlock, audioGetKey, audioWallCrash;
    public AudioClip audioMoveStart, audioMoveLoop, audioMoveEnd;

    public AudioSource moveAudioSource;

    bool m_IsBlown = false;
    bool m_Dead = false;
    bool m_trulyDead = false;
    bool m_HasKey = false;

    public bool IsBlown => m_IsBlown;
    public bool HasKey => m_HasKey;

    Fan m_Fan;

    [Header("Status")]
    public ParticleSystem wetParticles;
    public ParticleSystem zapParticles;
    public ParticleSystem freezeParticles;
    [SerializeField] StatusHandler m_StatusHandler = new StatusHandler();
    [SerializeField] FrozenStatus m_FrozenStatus;
    public Status Status => m_StatusHandler.CurrentStatus;

    [Header("Spatial Properties")]
    [SerializeField] Tilemap m_Tilemap;
    public Tilemap Tilemap => m_Tilemap;

    [Header("Movement Properties")]
    [SerializeField] float m_DefaultSpeed;
    float m_CurrentSpeed;
    [SerializeField] KeyCode m_ManualControlKey;
    [SerializeField] bool manualControl;
    [SerializeField] bool enableManualControl;
    public int startingControlChips;
    int remainingControlChips;
    public float manualControlTime;
    float currentControlTime;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI itemsUI;

    [Header("Planner")]
    [SerializeField] GOAP.Planner m_Planner;
    [SerializeField] GOAP.Blackboard m_WorldState = new GOAP.Blackboard();
    public GOAP.Blackboard WorldState => m_WorldState;

    GOAP.WorkingMemory m_WorkingMemory;
    bool flipped;
    float startingScale;
    Vector2 currentFacingDir;
    float flipXTime = 0.1f;
    float facingDirTime;
    bool isMoving;
    bool moveAudioStarted;
    bool moveAudioLooping;
    float dissolveSpeed;
    float dissolveAmount = 0;

    public float DefaultSpeed => m_DefaultSpeed;
    public float Speed => m_CurrentSpeed;

    public Transform scriptedMoveDestination;
    bool scriptedMoveUp;

    void Awake()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        aSource = GetComponent<AudioSource>();
        m_Animator = m_SpriteRenderer.gameObject.GetComponent<Animator>();
        m_GlowLight = GetComponentInChildren<Light2D>();
        gameManager = FindObjectOfType<GameManager>();

        Debug.Assert(m_SpriteRenderer != null, "Agent does not have a sprite renderer!");
        startingScale = transform.localScale.x;

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

        currentFacingDir = Vector2.right;
        flipped = false;
        manualControl = false;
        remainingControlChips = startingControlChips;

        UpdateItemsUI();

        scriptedMoveDestination = GameObject.FindGameObjectWithTag("CamStartPos").transform;

        scriptedMoveUp = true;
        StartCoroutine(ScriptedMoveUp(Vector3.Distance(transform.position, scriptedMoveDestination.position) / m_CurrentSpeed));
    }

    public void MoveUp(float distance)
    {
        scriptedMoveUp = true;
        StartCoroutine(ScriptedMoveUp(distance / m_CurrentSpeed));
    }

    public void UpdateAgent()
    {
        if (m_Dead)
        {
            // if(m_Dead) Debug.Log("Dead");
            // Here lerp bot to center and then blow it
            // m_Animator.SetBool("isMoving", false);
            Camera.main.GetComponent<CameraScript>().SetCam(transform, 25f, new Vector3(0, 0, -10));

            if (m_trulyDead)
            {
                isMoving = false;
                m_Animator.SetBool("isDead", true);
                deadSpriteRenderer.enabled = true;
                gameManager.SetGameState(GameManager.eGameState.LOSE);

                dissolveAmount += Time.deltaTime * dissolveSpeed;
                m_SpriteRenderer.material.SetFloat("_DissolveAmount", dissolveAmount);
                deadSpriteRenderer.material.SetFloat("_DissolveAmount", 1-dissolveAmount);
            }
            return;
        }

        if (GameManager.Instance.IsPaused)
        {
            return;
        }

        m_Animator.SetBool("isMoving", isMoving);
        if (scriptedMoveUp) return;

        m_StatusHandler.Update(this);

        if(m_IsBlown)
        {
            return;
        }

        isMoving = GetCurrentPath() != null;

        float materialColorIntensity = m_SpriteRenderer.material.GetFloat("_AddColorIntensity");
        if (materialColorIntensity > 0)
        {
            m_SpriteRenderer.material.SetFloat("_AddColorIntensity", materialColorIntensity -= Time.deltaTime * 0.75f);
        }

        if (m_Planner.IsPaused == false)
        {
            // GOAP
            GOAP.Plan currentPlan = m_Planner.GetCurrentPlan();

            if (currentPlan != null)
            {
                GOAP.Action currentAction = currentPlan.GetCurrentAction();

                if (currentAction.GetStatus() == GOAP.Action.ExecutionStatus.Executing ||
                    currentAction.GetStatus() == GOAP.Action.ExecutionStatus.None)
                {
                    Debug.Log($"Executing {currentAction.GetName()}");
                }

                currentPlan.Execute(this);
            }
        }

        ManualControl();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            if (!moveAudioStarted)
            {
                PlaySound(moveAudioSource, audioMoveStart, 0.5f);
                moveAudioStarted = true;
            }
            else if (!moveAudioLooping && !moveAudioSource.isPlaying)
            {
                PlaySound(moveAudioSource, audioMoveLoop, 0.5f, true);
                moveAudioLooping = true;
            }
        }
        else
        {
            if (moveAudioStarted && !moveAudioSource.isPlaying)
            {
                PlaySound(moveAudioSource, audioMoveEnd, 0.5f);
                moveAudioStarted = false;
                moveAudioLooping = false;
            }
            if (moveAudioLooping)
            {
                PlaySound(moveAudioSource, audioMoveEnd, 0.5f);
                moveAudioStarted = false;
                moveAudioLooping = false;
            }
        }
    }

    private void PlaySound(AudioSource audioSource, AudioClip clip, float volume, bool loop = false)
    {
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void FlashColor(float amount, Color color)
    {
        m_SpriteRenderer.material.SetColor("_AddColor", color);
        m_SpriteRenderer.material.SetFloat("_AddColorIntensity", amount);
    }

    public void SetDirection(Vector3 direction)
    {
        Vector2 newDir = direction;
        newDir.y = 0;
        newDir.Normalize();
        if (Vector2.Dot(newDir, currentFacingDir) < 0f)
        {
            // moving opposite way
            facingDirTime -= Time.deltaTime;

            if (facingDirTime < 0)
            {
                StartCoroutine(FlipXCoroutine(flipXTime));
                currentFacingDir = newDir;
                facingDirTime = flipXTime;
            }
        }
        else
        {
            facingDirTime = flipXTime;
        }
    }

    private void ManualControl()
    {
        if (Input.GetKeyDown(m_ManualControlKey))
        {
            if (remainingControlChips > 0)
            {
                remainingControlChips--;
                manualControl = true;
                currentControlTime = manualControlTime;
                m_Planner.PausePlanner();
                UpdateItemsUI();
            }
        }
        if (currentControlTime > 0)
        {
            currentControlTime -= Time.deltaTime;
            float hori = Input.GetAxisRaw("Horizontal");
            float vert = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(hori, vert, 0).normalized;
            Debug.DrawLine(transform.position, transform.position + direction, Color.green, 1f);
            SetDirection(direction);
            float checkDist = transform.localScale.y/2;

            Vector2 startPos = transform.position;
            startPos.y += 0.2f;

            if (hori != 0 && vert != 0) checkDist *= Mathf.Sqrt(2);

            RaycastHit2D hit = Physics2D.Raycast(startPos, direction);
            float distanceFromWall = Vector2.Distance(hit.point, startPos);
            if (hit.collider != null && hit.collider.CompareTag("Wall") && distanceFromWall < checkDist)
            {
                // Debug.DrawLine(startPos, hit.point, Color.red, 1f);
                Debug.Log("moving into wall");
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, m_CurrentSpeed * Time.deltaTime);
            }

            if (currentControlTime <= 0)
            {
                // end manual control
                manualControl = false;
                m_Planner.ResumePlanner();

                UpdateItemsUI();
            }
        }
    }

    IEnumerator ScriptedMoveUp(float duration)
    {
        float t = 0;
        m_Planner.PausePlanner();
        isMoving = true;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, m_CurrentSpeed * Time.deltaTime);
            yield return null;
        }

        m_Planner.ResumePlanner();
        scriptedMoveUp = false;
        isMoving = false;
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
                pathRequest.PathQuery,
                pathRequest.PathCompleteAction);
        }

        return m_CurrentPath;
    }

    // Need to figure out a cleaner way to handle path requests, what is considered a valid repeated request?
    public void ClearPathPlanningRequests()
    {
        m_PathPlanningRequests.Clear();
    }

    // Todo: Deprecate this and need to find a better way to replan path or set current path to null?
    public void ClearCurrentPath()
    {
        m_CurrentPath = null;
    }

    public void AddPathRequest(Vector3 targetPosition, PathQuery pathQuery, Action<Agent> pathCompleteAction)
    {
        m_PathPlanningRequests.Enqueue(new Path.Request(targetPosition, pathQuery, pathCompleteAction));
    }

    public bool AddPathRequestToClosestTileOfType<T>(PathQuery pathQuery, Action<Agent> pathCompleteAction)
    {
        Path.Request pathRequest = m_Pathfinding.GeneratePathRequestToClosestTileOfType<T>(
            transform.position, 
            pathQuery, 
            pathCompleteAction);

        if(pathRequest != null)
        {
            m_PathPlanningRequests.Enqueue(pathRequest);
            return true;
        }

        return false;
    }

    public bool AddPathRequestToClosestTileOfType<T>(Action<Agent> pathCompleteAction)
    {
        return AddPathRequestToClosestTileOfType<T>(new PathQuery(), pathCompleteAction);
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

    public void AssignActions(List<GOAP.Action> actions)
    {
        m_Planner.AssignActions(actions);
    }

    public void Replan()
    {
        m_Planner.RestartPlan(m_WorldState);
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

    public void SetLightColor(Color color)
    {
        m_GlowLight.color = color;
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

    public void GetPathToClosestPosition(List<Vector3> positions)
    {
    }

    public void Die(Vector3 deathPosition, Sprite deathSprite, float _dissolveSpeed, AudioClipGroup deathSound = null)
    {
        m_Dead = true;
        m_IsBlown = false;
        dissolveSpeed = _dissolveSpeed;

        // Debug.Log("Death position " + deathPosition);

        StopAllCoroutines();

        SetCoroutine(DyingCoroutine(transform.position, deathPosition, deathSprite, deathSound));
    }

    IEnumerator DyingCoroutine(Vector3 currentPosition, Vector3 nextPosition, Sprite deathSprite, AudioClipGroup audio = null)
    {
        float currentSpeed = m_Rigidbody2D.isKinematic ? Speed : m_Rigidbody2D.velocity.magnitude;

        m_Rigidbody2D.simulated = false;
        m_Rigidbody2D.isKinematic = true;

        float totalTime = (currentPosition - nextPosition).magnitude / currentSpeed;
        float t = 0;

        while (t < totalTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, currentSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Truly dead");
        m_trulyDead = true;
        deadSpriteRenderer.sprite = deathSprite;

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

    public void ReplanWithNewPathQuery(PathQuery newPathQuery)
    {
        if(m_CurrentPath != null)
        {
            Path.Request pathRequest = new Path.Request(
                m_CurrentPath.TargetPosition,
                newPathQuery,
                m_CurrentPath.PathCompleteAction);

            m_PathPlanningRequests.Enqueue(pathRequest);
        }

        m_CurrentPath = null;
    }
    public void PickupKey()
    {
        m_HasKey = true;
        UpdateItemsUI();
    }

    private void UpdateItemsUI()
    {
        itemsUI.text = "Key Obtained: " + m_HasKey + "\n\n" 
                            + "[X] Override Chips: " + remainingControlChips;
        if (manualControl)
        {
            itemsUI.text += "\n\n" + "OVERRIDE ACTIVE";
        }
    }

    IEnumerator FlipXCoroutine(float seconds)
    {
        float totalTime = seconds;
        float t = 0;
        float startScale = 1;
        float endScale = 1;
        if (flipped)
        {
            startScale = -startingScale;
            endScale = startingScale;
        }
        else
        {
            startScale = startingScale;
            endScale = -startingScale;
        }
        while (t < totalTime)
        {
            float lerp = Mathf.Lerp(startScale, endScale, t / totalTime);
            if (lerp <= 0.001f && lerp >= -0.001f) lerp = 0.001f;
            transform.localScale = new Vector3(lerp, transform.localScale.y, transform.localScale.z);
            t += Time.deltaTime;
            if (t > totalTime) t = totalTime;
            yield return null;
        }
        transform.localScale = new Vector3(endScale, transform.localScale.y, transform.localScale.z);
        flipped = !flipped;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && m_IsBlown)
        {
            m_IsBlown = false;
            m_Rigidbody2D.isKinematic = true;
            m_Rigidbody2D.velocity = Vector2.zero;
            audioWallCrash.PlayOneShot(aSource);
            Replan();
            ApplyStatus(m_FrozenStatus);

            //Path resumePath = m_CurrentPath;
            //m_CurrentPath = null;
            //StartCoroutine(ResumeAfterWait(1.0f, resumePath));
        }
    }

    private IEnumerator ResumeAfterWait(float seconds, Path resumePath)
    {
        yield return new WaitForSeconds(seconds);

        // Reevaluate path based on old path
        //m_CurrentPath = m_Pathfinding.FindPath(
        //    transform.position,
        //    resumePath.TargetPosition,
        //    resumePath.PathQuery,
        //    resumePath.PathCompleteAction);
    }
}