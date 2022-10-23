using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/Burn")]
public class BurnStatus : Status
{
    [SerializeField] WetStatus m_WetStatus;
    [SerializeField] DefaultStatus m_DefaultStatus;

    public override StatusEffect TransitionFrom(Status otherStatus)
    {
        StatusEffect statusEffect = null;

        switch(otherStatus)
        {
            case FrozenStatus:
                statusEffect = new StatusEffect((Agent agent) =>
                {
                    agent.ApplyStatus(m_WetStatus);
                });
                break;
            case WetStatus:
                statusEffect = new StatusEffect((Agent agent) =>
                {
                    agent.ApplyStatus(m_DefaultStatus);
                });
                break;
        }

        return statusEffect;
    }
}