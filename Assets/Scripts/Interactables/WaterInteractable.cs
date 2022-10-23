using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Water")]
public class WaterInteractable : Interactable
{
    public override void Interact(Agent agent)
    {
        Debug.Log("Bot stepped on water");
        agent.SetStatus(m_Status);
    }
}