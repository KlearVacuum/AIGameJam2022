using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public Transform leftDoorEndTransform;
    public Transform rightDoorEndTransform;
    public float doorMoveSpeed;
    BoxCollider2D col;
    public enum Type { HORIZONTAL, VERTICAL }

    bool m_Opened = false;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }
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
        // Debug.Log("Waiting for door to open");
        yield return new WaitForSeconds(1f);
        // Debug.Log("Door opened");
        agent.audioUnlock.PlayOneShot(agent.aSource);


        float totalTime = (leftDoor.transform.position - leftDoorEndTransform.position).magnitude / doorMoveSpeed;
        float t = 0;

        while (t < totalTime)
        {
            leftDoor.transform.position = Vector3.MoveTowards(leftDoor.transform.position, leftDoorEndTransform.position, doorMoveSpeed * Time.deltaTime);
            rightDoor.transform.position = Vector3.MoveTowards(rightDoor.transform.position, rightDoorEndTransform.position, doorMoveSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        col.enabled = false;
    }
}