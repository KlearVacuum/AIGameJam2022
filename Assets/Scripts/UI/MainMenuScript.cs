using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void GoToLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
