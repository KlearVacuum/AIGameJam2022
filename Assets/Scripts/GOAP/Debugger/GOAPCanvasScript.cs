using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GOAPCanvasScript : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
