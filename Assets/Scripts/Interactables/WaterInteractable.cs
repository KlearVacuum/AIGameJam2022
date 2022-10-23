using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Water")]
public class WaterInteractable : Interactable
{
    public override void Interact(Agent agent)
    {
        agent.SetStatus(m_Status);
    }
}