using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum eGameState
    {
        NONE,
        RUNNING,
        PAUSED,
        LOSE,
        WIN
    }
    private eGameState currentGameState;
    private eGameState nextGameState;
    private float oldTimeScale = 1;
    public float maxAudioDistance;

    private GameObject losePanel;
    private GameObject pausePanel;
    public bool IsPaused => currentGameState == eGameState.PAUSED || currentGameState == eGameState.LOSE;

    protected override void Awake() 
    {
        base.Awake();

        GlobalGameData.playerGO = GameObject.FindGameObjectWithTag("Player");
        GlobalGameData.maxAudioDistance = maxAudioDistance;
        GlobalGameData.blackPanelFade = GameObject.FindGameObjectWithTag("BlackPanel").GetComponent<UIFade>();
        losePanel = GameObject.FindGameObjectWithTag("LosePanel");
        pausePanel = FindObjectOfType<PausePanelScript>().gameObject;

        nextGameState = currentGameState = eGameState.RUNNING;
    }

    private void Start()
    {
        GlobalGameData.blackPanelFade.FadeOut();
    }

    private void Update()
    {
        UpdateGameState();
        CheckLose();
    }

    public void SetGameState(eGameState newState)
    {
        nextGameState = newState;
    }

    void UpdateGameState()
    {
        if (currentGameState != nextGameState)
                currentGameState = nextGameState;

        if (currentGameState != eGameState.LOSE && Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentGameState)
            {
                case eGameState.RUNNING:
                    StartCoroutine(PauseAfterDelay(0.15f));
                    break;
                case eGameState.PAUSED:
                    StartCoroutine(ResumeAfterDelay(0.15f));
                    break;
            }
        }
    }

    public void CheckLose()
    {
        if (currentGameState == eGameState.LOSE)
        {
            StartCoroutine(PauseAfterDelay(2.5f));
        }
    }

    IEnumerator PauseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnablePanel(true);

        if (currentGameState != eGameState.LOSE)
        {
            nextGameState = eGameState.PAUSED;
        }
    }

    public void Resume()
    {
        StartCoroutine(ResumeAfterDelay(0.15f));
    }

    IEnumerator ResumeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnablePanel(false);
        nextGameState = eGameState.RUNNING;
    }

    void EnablePanel(bool enable)
    {
        switch (currentGameState)
        {
            case eGameState.RUNNING:
            case eGameState.PAUSED:
                pausePanel.SetActive(enable);
                break;
            case eGameState.LOSE:
                losePanel.SetActive(enable);
                break;
        }
    }
}