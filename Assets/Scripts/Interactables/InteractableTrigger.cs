using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    [SerializeField] Interactable m_Interactable;

    public Interactable Interactable => m_Interactable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Agent agent = collision.transform.GetComponent<Agent>();

        if (agent != null)
        {
            m_Interactable.Interact(agent, transform.position);
        }
    }
}
