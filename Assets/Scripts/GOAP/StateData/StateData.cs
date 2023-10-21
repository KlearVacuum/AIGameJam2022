using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GOAP
{
    public abstract class StateData<T> : IStateData
    {
        [SerializeField] protected StateValue<T> m_StateValue;

        public override string Name => m_StateKey;

        public override string StringifyType() => StringifyTypeInternal();

        public virtual void Initialize(string key, T value)
        {
            m_StateKey = ScriptableObject.CreateInstance<StateKey>();
            m_StateKey.Initialize(key);

            m_StateValue = new StateValue<T>(value);
        }

        public T Value => m_StateValue;
        public override IStateValue GetStateValue() => m_StateValue;

        public override IStateValue CloneStateValue()
        {
            return new StateValue<T>(m_StateValue.Data);
        }

        protected abstract string StringifyTypeInternal();
    }
}
