using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float maxAudioDistance;
    private void Awake()
    {
        GlobalGameData.playerGO = GameObject.FindGameObjectWithTag("Player");
        GlobalGameData.maxAudioDistance = maxAudioDistance;
        GlobalGameData.blackPanelFade = GameObject.FindGameObjectWithTag("BlackPanel").GetComponent<UIFade>();
    }

    private void Start()
    {
        GlobalGameData.blackPanelFade.FadeOut();
    }
}