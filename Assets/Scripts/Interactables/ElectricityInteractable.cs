using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Electricity")]
public class ElectricityInteractable : Interactable
{
    [SerializeField] Sprite m_ShockedDieSprite;

    public override void Interact(Agent agent, Vector3 tilePosition)
    {
        // Debug.Log("Touched electricity");
        if(agent.Status is WetStatus)
        {
            // agent.audioZapDie.PlayOneShot(agent.aSource);
            agent.Die(tilePosition, m_ShockedDieSprite, 100f, agent.audioZapDie);
        }
        else
        {
            agent.audioZap.PlayOneShot(agent.aSource);
            agent.ApplyStatus(m_Status);
        }

    }
}