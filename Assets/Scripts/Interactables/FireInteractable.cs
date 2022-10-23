using System;
using System.Collections;

using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Fire")]
public class FireInteractable : Interactable
{
    public override void Interact(Agent agent, Vector3 tilePosition)
    {
        if(agent.Status is DefaultStatus)
        {
            agent.Die(tilePosition, m_Status.Sprite);
        }
        else
        {
            agent.SetStatus(m_Status);
        }
    }
}