using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Status/Default")]
public class DefaultStatus : Status
{
    public override StatusEffect TransitionFrom(Status otherStatus)
    {
        return new StatusEffect((Agent agent) =>
        {
            agent.SetSprite(m_Sprite);
            agent.SetLightColor(m_LightColor);
        });
    }
}