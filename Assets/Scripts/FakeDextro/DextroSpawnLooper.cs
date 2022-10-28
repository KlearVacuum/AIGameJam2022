using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DextroSpawnLooper : MonoBehaviour
{
    public Vector2 startSpawnPoint;
    public float varianceX;
    public float varianceY;
    public GameObject dextroFakePrefab;
    public int qty;

    private void Start()
    {
        for (int i = 0; i < qty; i++)
        {
            Vector2 spawnPoint = new Vector2(Random.Range(startSpawnPoint.x - varianceX, startSpawnPoint.x + varianceX),
                                                                            Random.Range(startSpawnPoint.y - varianceY, startSpawnPoint.y + varianceY));
            GameObject spawn = Instantiate(dextroFakePrefab, spawnPoint, Quaternion.identity);
            spawn.GetComponent<FakeDextroScript>().spawnPoint = startSpawnPoint;
            spawn.GetComponent<SpriteRenderer>().sortingOrder = (int)(spawnPoint.y * 100f + startSpawnPoint.y);
        }
    }
}
