using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanelScript : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Continue()
    {
        GameManager.Instance.Resume();
    }

    public void Quit()
    {
        Time.timeScale = 1;
        GlobalGameData.blackPanelFade.nextLevel = "MainMenu";
        GlobalGameData.blackPanelFade.FadeIn();
    }
}
