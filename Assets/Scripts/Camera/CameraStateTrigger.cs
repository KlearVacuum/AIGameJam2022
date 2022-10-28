using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateTrigger : MonoBehaviour
{
    public Transform target;
    public float fieldOfView;
    public Vector3 offset;
    public bool fadeIn;
    public float fadeDelay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Camera.main.GetComponent<CameraScript>().SetCam(target, fieldOfView, offset);
            if (fadeIn)
                GlobalGameData.blackPanelFade.FadeIn(fadeDelay);
            gameObject.SetActive(false);
        }
    } 
}
