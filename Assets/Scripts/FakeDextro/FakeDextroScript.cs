using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeDextroScript : MonoBehaviour
{
    [HideInInspector]
    public Vector3 spawnPoint;
    public float minSpeed;
    public float maxSpeed;
    float speed;
    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position = spawnPoint;
        speed = Random.Range(minSpeed, maxSpeed);
    }
}
