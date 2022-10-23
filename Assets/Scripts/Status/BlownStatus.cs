using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/Blown")]
public class BlownStatus : Status
{
    [SerializeField] FrozenStatus m_FrozenStatus;
    [SerializeField] float m_BlownSpeed;

    public override StatusEffect TransitionFrom(Status otherStatus)
    {
        StatusEffect statusEffect = null;

        switch(otherStatus)
        {
            case WetStatus:
                statusEffect = new StatusEffect((Agent agent) =>
                {
                    agent.SetStatus(m_FrozenStatus);
                    agent.SetSpeed(m_BlownSpeed);
                    // Other effects..
                });
                break;
        }

        return statusEffect;
    }
}