using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Water")]
public class WaterInteractable : Interactable
{
    public override void Interact(Agent agent, Vector3 tilePosition)
    {
        agent.ApplyStatus(m_Status);
        Debug.Log("Touched water");
    }
}