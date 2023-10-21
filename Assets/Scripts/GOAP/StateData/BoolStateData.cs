using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(menuName = "GOAP/StateData/Boolean", order = 1)]
    class BoolStateData : StateData<bool>
    {
        protected override string StringifyTypeInternal()
        {
            return "Bool";
        }
    }
}
