using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public abstract class ActionData : ScriptableObject
    {
        [SerializeField] protected float m_Cost;
        [SerializeField] protected Precondition m_Precondition = new Precondition();
        [SerializeField] protected Effect m_Effect = new Effect();

        public abstract Action CreateAction();
    }
}


