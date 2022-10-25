using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateTrigger : MonoBehaviour
{
    public Transform target;
    public float fieldOfView;
    public Vector3 offset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Camera.main.GetComponent<CameraScript>().SetCam(target, fieldOfView, offset);
            gameObject.SetActive(false);
        }
    }
}
