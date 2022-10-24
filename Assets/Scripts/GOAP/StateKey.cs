using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(menuName = "GOAP/State Key", order = 1)]
    public class StateKey : ScriptableObject
    {
        [SerializeField] private string m_Value;

        public void Initialize(string value)
        {
            m_Value = value;
        }

        public static implicit operator string(StateKey key)
        {
            return key.m_Value;
        }
    }
}
