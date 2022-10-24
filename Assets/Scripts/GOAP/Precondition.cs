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

        private bool CompareStateValueToWorldState<T>(IStateData state, Blackboard worldState)
        {
            StateData<T> data = state as StateData<T>;
            return !worldState.GetStateValue<T>(data.Key).Equals(data.Value);
        }
    }
}
