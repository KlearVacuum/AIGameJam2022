﻿using System.Collections.Generic;
using UnityEngine;


namespace GOAP
{
    [System.Serializable]
    public abstract class Goal : ScriptableObject
    {
        [SerializeField] protected float m_Priority;
        [SerializeField] protected Precondition m_Precondition = new Precondition();
        [SerializeField] protected Effect m_Effect = new Effect();

        public virtual float GetPriority() => m_Priority;

        public Precondition GetPreconditions() => m_Precondition;
        public Effect GetEffects() => m_Effect;
        public abstract string GetName();

        public bool Satifies(Dictionary<string, IStateValue> desiredState)
        {
            return m_Effect.Satisfies(desiredState);
        }

        public bool IsSatisfiedBy(Effect effect)
        {
            return effect.Satisfies(m_Precondition);
        }

    }
}

