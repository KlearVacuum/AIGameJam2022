﻿using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Water")]
public class WaterInteractable : Interactable
{
    public Sprite shockDieSprite;

    public override void Interact(Agent agent, Vector3 tilePosition)
    {
        // Debug.Log("Touched water");
        if (agent.Status is ShockedStatus)
        {
            // agent.audioZapDie.PlayOneShot(agent.aSource);
            agent.Die(tilePosition, shockDieSprite, 100f, agent.audioZapDie);
            agent.zapParticles.Play();
        }
        else
        {
            agent.audioWet.PlayOneShot(agent.aSource);
            agent.ApplyStatus(m_Status);
            agent.wetParticles.Play();
        }
    }
}