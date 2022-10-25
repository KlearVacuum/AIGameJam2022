﻿using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/Frozen")]
public class FrozenStatus : Status
{
    public override StatusEffect TransitionFrom(Status otherStatus)
    {
        StatusEffect statusEffect = null;

        switch(otherStatus)
        {
            case WetStatus:
                statusEffect = new StatusEffect((Agent agent) =>
                {
                    agent.audioFreeze.PlayOneShot(agent.aSource);
                    agent.SetSprite(m_Sprite);
                    agent.SetLightColor(m_LightColor);
                    // Other effects..
                });
                break;
        }

        return statusEffect;
    }
}