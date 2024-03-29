using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideOnClick : MonoBehaviour
{
    public GameObject showHideObject;
    public bool startHidden;

    private void Start()
    {
        if (startHidden) showHideObject.SetActive(false);
    }

    public void ShowHide()
    {
        if (GameManager.Instance.IsPaused)
        {
            return;
        }

        showHideObject.SetActive(!showHideObject.activeInHierarchy);
    }
}
