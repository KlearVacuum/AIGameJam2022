using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanelScript : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void RestartLevel()
    {
        GlobalGameData.blackPanelFade.nextLevel = SceneManager.GetActiveScene().name;
        GlobalGameData.blackPanelFade.FadeIn();
    }

    public void QuitToMainMenu()
    {
        GlobalGameData.blackPanelFade.nextLevel = "MainMenu";
        GlobalGameData.blackPanelFade.FadeIn();
    }
}
