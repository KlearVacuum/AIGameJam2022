using UnityEngine;

namespace GOAP
{
    public abstract class IStateData : ScriptableObject
    {
        [SerializeField] protected StateKey m_StateKey;
        public StateKey Key => m_StateKey;

        public abstract IStateValue GetStateValue();

        public static T Get<T>(IStateData data)
        {
            return (data as StateData<T>).Value;
        }
    }

}
