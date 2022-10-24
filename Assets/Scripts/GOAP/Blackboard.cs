using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    [System.Serializable]
    public class Blackboard
    {
        [System.Serializable]
        public struct State
        {
            [SerializeField] private string m_Key;
            public string Key => m_Key;


            [SerializeField] private bool m_Value;
            public bool Value => m_Value;

            public State(string key, bool value)
            {
                m_Key = key;
                m_Value = value;
            }

            public override string ToString()
            {
                return $"{m_Key} : {m_Value}";
            }
        }

        // [SerializeField] List<State> m_StateList = new List<State>();
        // Dictionary<string, bool> m_States = new Dictionary<string, bool>();

        [SerializeField] List<IStateData> m_StateList = new List<IStateData>();
        Dictionary<string, IStateValue> m_States = new Dictionary<string, IStateValue>();

        public void Initialize()
        {
            foreach (IStateData state in m_StateList)
            {
                m_States.Add(state.Key, state.GetStateValue());
            }
        }

        public T GetStateValue<T>(string stateKey)
        {
            Debug.Assert(m_States.ContainsKey(stateKey));
            return IStateValue.Get<T>(m_States[stateKey]);
        }

        public void SetStateValue(IStateData state)
        {
            switch (state)
            {
                case StateData<bool>:
                    SetStateValue(state.Key, (state as StateData<bool>).Value);
                    break;
                case StateData<int>:
                    SetStateValue(state.Key, (state as StateData<int>).Value);
                    break;
                case StateData<float>:
                    SetStateValue(state.Key, (state as StateData<float>).Value);
                    break;
                case StateData<Vector3>:
                    SetStateValue(state.Key, (state as StateData<Vector3>).Value);
                    break;

                default: throw new System.NotImplementedException("Invalid state data type.");
            }
        }

        public void SetStateValue<T>(string stateKey, T stateValue)
        {
            Debug.Assert(m_States.ContainsKey(stateKey));
            (m_States[stateKey] as StateValue<T>).Set(stateValue);
        }

        public void AddState<T>(string stateKey, T stateValue)
        {
            Debug.Assert(m_States.ContainsKey(stateKey) == false);
            m_States.Add(stateKey, new StateValue<T>(stateValue));
        }
    }
}


