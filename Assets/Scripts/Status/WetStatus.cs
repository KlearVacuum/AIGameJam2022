using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/Wet")]
public class WetStatus : Status
{
    public override StatusEffect TransitionFrom(Status otherStatus)
    {
        StatusEffect statusEffect = null;

        switch(otherStatus)
        {
            case DefaultStatus:
            case BurnStatus:
            case ShockedStatus:
                statusEffect = new StatusEffect((Agent agent) =>
                {
                    agent.SetSprite(m_Sprite);
                    agent.SetLightColor(m_LightColor);
                    // Other effects..
                });
                break;
        }

        return statusEffect;
    }
}