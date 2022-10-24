using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        GlobalGameData.playerGO = GameObject.FindGameObjectWithTag("Player");
    }
}
