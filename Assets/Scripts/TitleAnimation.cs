using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    private AudioSource aSource;
    public AudioClipGroup shortNoiseClip;
    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
    }
    public void PlayShortNoiseSound()
    {
        shortNoiseClip.PlayOneShot(aSource);
    }
}
