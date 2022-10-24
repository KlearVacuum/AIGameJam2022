using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/Shocked")]
public class ShockedStatus : Status
{
    [SerializeField] DefaultStatus m_DefaultStatus;
    [SerializeField] float m_SpeedBoostDuration;
    [SerializeField] float m_SpeedBoostAmount;

    public override StatusEffect TransitionFrom(Status otherStatus)
    {
        StatusEffect statusEffect = null;

        switch (otherStatus)
        {
            case DefaultStatus:
                statusEffect = new StatusEffect((Agent agent) =>
                {
                    agent.audioZap.PlayOneShot(agent.aSource);
                    SpeedBoost speedBoost = agent.gameObject.AddComponent<SpeedBoost>();
                    speedBoost.Initialize(agent, m_DefaultStatus, m_SpeedBoostDuration, m_SpeedBoostAmount);
                    agent.SetSprite(m_Sprite);
                });
                break;
        }

        return statusEffect;
    }
}