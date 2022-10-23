using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/Frozen")]
public class FrozenStatus : Status
{
    public override StatusEffect TransitionFrom(Status otherStatus)
    {
        StatusEffect statusEffect = null;

        statusEffect = new StatusEffect((Agent agent) =>
        {
            agent.SetSprite(m_Sprite);
            // Other effects..
        });

        return statusEffect;
    }
}