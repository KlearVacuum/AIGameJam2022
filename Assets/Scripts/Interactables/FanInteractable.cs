using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Fan")]
public class FanInteractable : Interactable
{
    [SerializeField] Fan m_Fan;

    public override void Interact(Agent agent)
    {
        Debug.Log("Bot stepped infront of fan");
        // Spawn fan
    }
}