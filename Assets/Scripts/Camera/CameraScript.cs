using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float desiredFOV;
    public float maxSpeed;
    public float FOVChangeSpeed;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        transform.position = GlobalGameData.playerGO.transform.position + offset;
        cam.fieldOfView = desiredFOV;
    }

    private void Update()
    {
        float distance = Vector2.Distance(target.position, transform.position + offset);
        if (distance < 0.005f && Mathf.Abs(cam.fieldOfView - desiredFOV) < 0.01f) return;

        Vector3 newPos = Vector3.Lerp(transform.position, target.position + offset, 0.01f);

        if (Vector3.Distance(transform.position, newPos) > maxSpeed * Time.deltaTime)
        {
            transform.position += (newPos - transform.position).normalized * maxSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = newPos;
        }

        float newFOV = Mathf.Lerp(cam.fieldOfView, desiredFOV, 0.5f);
        cam.fieldOfView = Mathf.Abs(newFOV - cam.fieldOfView) < 0.05f ? newFOV : cam.fieldOfView + Mathf.Sign(newFOV - cam.fieldOfView) * FOVChangeSpeed * Time.deltaTime;
    }

    public void SetCam(Transform _target, float _FOV, Vector3 _offset)
    {
        target = _target;
        desiredFOV = _FOV;
        offset = _offset;
    }
}
