using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(menuName = "GOAP/StateData/Float", order = 1)]
    class FloatStateData : StateData<float>
    {
        protected override string StringifyTypeInternal()
        {
            return "Float";
        }
    }
}
