using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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
    public float maxAudioDistance;

    private GameObject losePanel;
    private void Awake()
    {
        GlobalGameData.playerGO = GameObject.FindGameObjectWithTag("Player");
        GlobalGameData.maxAudioDistance = maxAudioDistance;
        GlobalGameData.blackPanelFade = GameObject.FindGameObjectWithTag("BlackPanel").GetComponent<UIFade>();
        losePanel = GameObject.FindGameObjectWithTag("LosePanel");

        nextGameState = currentGameState = eGameState.RUNNING;
    }

    private void Start()
    {
        GlobalGameData.blackPanelFade.FadeOut();
    }

    private void Update()
    {
        CheckLose();
        UpdateGameState();
    }

    public void SetGameState(eGameState newState)
    {
        nextGameState = newState;
    }

    void UpdateGameState()
    {
        if (currentGameState != nextGameState)
                currentGameState = nextGameState;
    }

    public void CheckLose()
    {
        if (currentGameState != eGameState.LOSE && nextGameState == eGameState.LOSE)
        {
            StartCoroutine(ShowLosePanelAfterSeconds(2.5f));
        }
    }

    IEnumerator ShowLosePanelAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        losePanel.SetActive(true);
    }
}