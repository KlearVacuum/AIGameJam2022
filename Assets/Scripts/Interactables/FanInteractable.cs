using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/FanInteractable")]
public class FanInteractable : Interactable
{
    public override void Interact(Agent agent)
    {
        Debug.Log("Bot stepped infront of fan");
        agent.SetStatus(m_Status);
    }
}