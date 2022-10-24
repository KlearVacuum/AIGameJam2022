using System;
using System.Collections;

using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Fire")]
public class FireInteractable : Interactable
{
    public Sprite wetSprite;
    public Sprite defaultSprite;
    public override void Interact(Agent agent, Vector3 tilePosition)
    {
        if(agent.Status is DefaultStatus)
        {
            agent.Die(tilePosition, m_Status.Sprite, 0.75f, agent.audioBurn);
        }
        else
        {
            if (agent.Status is FrozenStatus)
            {
                // Debug.Log("frozen to wet");
                agent.audioSteam.PlayOneShot(agent.aSource);
            }
            else if (agent.Status is WetStatus)
            {
                // Debug.Log("wet to default");
                agent.audioBurn.PlayOneShot(agent.aSource);
            }
            agent.ApplyStatus(m_Status);
        }
    }
}