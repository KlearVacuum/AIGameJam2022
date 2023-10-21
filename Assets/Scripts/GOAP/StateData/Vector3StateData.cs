using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(menuName = "GOAP/StateData/Vector3", order = 1)]
    class Vector3StateData : StateData<Vector3>
    {
        protected override string StringifyTypeInternal()
        {
            return "Vector3";
        }
    }
}
