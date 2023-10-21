using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(menuName = "GOAP/StateData/Int", order = 1)]
    class IntStateData : StateData<int>
    {
        protected override string StringifyTypeInternal()
        {
            return "Int";
        }
    }
}
