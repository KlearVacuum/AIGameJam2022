using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    public abstract class IStateValue
    {
        public static T Get<T>(IStateValue value)
        {
            return (value as StateValue<T>).Data;
        }
    }

    [System.Serializable]
    public class StateValue<T> : IStateValue
    {
        [SerializeField] T m_Data;
        public T Data => m_Data;

        public StateValue(T data)
        {
            m_Data = data;
        }

        public void Set(T data)
        {
            m_Data = data;
        }

        public static implicit operator T(StateValue<T> stateValue)
        {
            return stateValue.m_Data;
        }
    }

    public class StateValueComparer
    {
        public static bool Compare(IStateValue lhs, IStateValue rhs)
        {
            switch (lhs, rhs)
            {
                case (StateValue<bool>, StateValue<bool>):
                    return IStateValue.Get<bool>(lhs) == IStateValue.Get<bool>(rhs);
                case (StateValue<float>, StateValue<float>):
                    return IStateValue.Get<float>(lhs) == IStateValue.Get<float>(rhs);
                case (StateValue<int>, StateValue<int>):
                    return IStateValue.Get<int>(lhs) == IStateValue.Get<int>(rhs);
                case (StateValue<Vector3>, StateValue<Vector3>):
                    return IStateValue.Get<Vector3>(lhs) == IStateValue.Get<Vector3>(rhs);
                default:
                    return false;
            }
        }
    }
}