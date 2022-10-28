using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    private void Awake()
    {
        GlobalGameData.blackPanelFade = GameObject.FindGameObjectWithTag("BlackPanel").GetComponent<UIFade>();
    }
    private void Start()
    {
        GlobalGameData.blackPanelFade.FadeOut();
    }

    public void NextScene()
    {
        GlobalGameData.blackPanelFade.FadeIn();
    }
}
