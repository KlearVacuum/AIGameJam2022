using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private AudioSource aSource;
    public AudioClip hoverClip, enterClip;
    public GameObject[] levels;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
        aSource.volume = 0.5f;
    }
    public void PlayUIHoverSound()
    {
        aSource.PlayOneShot(hoverClip);
    }
    
    public void PlayUIEnterSound()
    {
        aSource.PlayOneShot(enterClip);
    }

    public void GoToLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ShowLevels()
    {
        foreach(GameObject level in levels)
        {
            level.SetActive(true);
        }
    }
}
