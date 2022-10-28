using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private AudioSource aSource;
    public AudioClip hoverClip, enterClip;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
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
}
