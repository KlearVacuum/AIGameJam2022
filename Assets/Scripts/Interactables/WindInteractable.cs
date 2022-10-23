using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/WaterWind")]
public class WindInteractable : Interactable
{
    public override void Interact(Agent agent)
    {
        Debug.Log("Bot is blown");
        agent.SetStatus(m_Status);
    }
}