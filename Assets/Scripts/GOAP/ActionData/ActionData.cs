using System.Collections;
using UnityEngine;

namespace GOAP
{
    public abstract class ActionData : ScriptableObject
    {
        abstract public Action CreateAction();

        [SerializeField] protected float m_Cost;
        [SerializeField] protected Precondition m_Precondition = new Precondition();
        [SerializeField] protected Effect m_Effect = new Effect();
    }
}


