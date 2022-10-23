using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/WaterWind")]
public class WindInteractable : Interactable
{
    public override void Interact(Agent agent, Vector3 tilePosition)
    {
        agent.SetStatus(m_Status);
    }
}