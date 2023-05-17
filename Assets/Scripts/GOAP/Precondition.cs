using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    [System.Serializable]
    public class Precondition
    {
        // [SerializeField] List<StateData> m_Conditions = new List<StateData>();
        [SerializeField] List<IStateData> m_Conditions = new List<IStateData>();

        public List<IStateData> Conditions => m_Conditions;

        public Precondition()
        {
            m_Conditions = new List<IStateData>();
        }

        public Precondition(Precondition other)
        {
            m_Conditions = new List<IStateData>(other.m_Conditions);
        }

        public bool Validate(Blackboard worldState)
        {
            foreach (IStateData state in m_Conditions)
            {
                bool failed = false;

                switch (state)
                {
                    case BoolStateData:
                        {
                            failed = CompareStateValueToWorldState<bool>(state, worldState);
                            break;
                        }
                    case IntStateData:
                        {
                            failed = CompareStateValueToWorldState<int>(state, worldState);
                            break;
                        }
                    case FloatStateData:
                        {
                            failed = CompareStateValueToWorldState<float>(state, worldState);
                            break;
                        }
                    case Vector3StateData:
                        {
                            failed = CompareStateValueToWorldState<Vector3>(state, worldState);
                            break;
                        }

                    default: throw new System.NotImplementedException("Invalid state data type.");
                }

                if (failed)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Contains<T>(string key, T value) where T : struct
        {
            foreach(IStateData condition in m_Conditions)
            {
                if(condition.Key == key)
                {
                    return IStateData.Get<T>(condition).Equals(value);
                }
            }

            return false;
        }

        public bool ContainsKey(string key)
        {
            foreach (IStateData condition in m_Conditions)
            {
                if (condition.Key == key)
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveCondition(IStateData conditionToRemove)
        {
            if(conditionToRemove != null)
            {
                foreach(IStateData condition in m_Conditions)
                {
                    if(condition.Key != conditionToRemove.Key)
                    {
                        continue;
                    }

                    bool hasSameValue =
                        StateValueComparer.Compare(
                           condition.GetStateValue(),
                           conditionToRemove.GetStateValue());

                    if(hasSameValue)
                    {
                        m_Conditions.Remove(conditionToRemove);
                        break;
                    }
                }
            }
        }

        private bool CompareStateValueToWorldState<T>(IStateData state, Blackboard worldState)
        {
            StateData<T> data = state as StateData<T>;
            return !worldState.GetStateValue<T>(data.Key).Equals(data.Value);
        }
    }
}
