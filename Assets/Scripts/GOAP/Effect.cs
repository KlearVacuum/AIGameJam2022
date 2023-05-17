using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [System.Serializable]
    public class Effect
    {
        [SerializeField] List<IStateData> m_Results = new List<IStateData>();
        public List<IStateData> Results => m_Results;

        public Effect()
        {
            m_Results = new List<IStateData>();
        }

        public Effect(Effect other)
        {
            m_Results = new List<IStateData>(other.Results);
        }

        public bool Satisfies(Dictionary<string, IStateValue> desiredState)
        {
            foreach (IStateData result in m_Results)
            {
                Debug.Assert(result != null, "IStateData is null!");

                if (desiredState.ContainsKey(result.Key) == false)
                {
                    return false;
                }

                if (!StateValueComparer.Compare(desiredState[result.Key], result.GetStateValue()))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Satisfies(Precondition precondition)
        {
            Dictionary<string, IStateValue> desiredState = new Dictionary<string, IStateValue>();

            foreach (IStateData data in precondition.Conditions)
            {
                desiredState.Add(data.Key, data.GetStateValue());
            }

            return Satisfies(desiredState);
        }
    }
}

