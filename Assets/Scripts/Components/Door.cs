using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public enum Type { HORIZONTAL, VERTICAL }

    bool m_Opened = false;
    public void Open(Agent agent)
    {
        if(agent.HasKey == false)
        {
            return;
        }

        StartCoroutine(OpenDoor(agent));
    }

    IEnumerator OpenDoor(Agent agent)
    {
        Debug.Log("Waiting for door to open");
        yield return new WaitForSeconds(1f);
        Debug.Log("Door opened");

        gameObject.SetActive(false);
        agent.Move();
    }
}