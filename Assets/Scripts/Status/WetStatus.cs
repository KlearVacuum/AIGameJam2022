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
            case ShockedStatus:
                statusEffect = new StatusEffect((Agent agent) =>
                {
                    agent.SetSprite(m_Sprite);
                    // Other effects..
                });
                break;
        }

        return statusEffect;
    }
}