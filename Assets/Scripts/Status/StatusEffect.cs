using System;
using UnityEngine;
public class StatusEffect
{
    Action<Agent> m_Effect;

    public StatusEffect(Action<Agent> effect)
    {
        m_Effect = effect;
    }

    public void Apply(Agent agent)
    {
        m_Effect(agent);
    }
}