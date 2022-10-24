using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Electricity")]
public class ElectricityInteractable : Interactable
{
    [SerializeField] Sprite m_ShockedDieSprite;

    public override void Interact(Agent agent, Vector3 tilePosition)
    {
        Debug.Log("Touched electricity");
        if(agent.Status is WetStatus)
        {
            Debug.Log("zapp die");
            agent.audioZapDie.PlayOneShot(agent.aSource);
            agent.Die(tilePosition, m_ShockedDieSprite);
        }
        else
        {
            Debug.Log("zapp");
            agent.audioZap.PlayOneShot(agent.aSource);
            agent.ApplyStatus(m_Status);
        }

    }
}