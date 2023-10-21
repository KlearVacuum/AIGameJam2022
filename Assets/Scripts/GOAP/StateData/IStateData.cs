using System;
using UnityEngine;

namespace GOAP
{
    public abstract class IStateData : ScriptableObject
    {
        [SerializeField] protected StateKey m_StateKey;
        public StateKey Key => m_StateKey;

        public abstract string Name { get; }
        public abstract string StringifyType();

        public abstract IStateValue GetStateValue();

        public abstract IStateValue CloneStateValue();
        
        public static T Get<T>(IStateData data)
        {
            return (data as StateData<T>).Value;
        }
    }

}
