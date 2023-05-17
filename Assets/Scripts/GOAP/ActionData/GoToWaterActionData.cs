using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    [CreateAssetMenu(menuName = "Planner/Actions/GoToWaterAction")]
    class GoToWaterActionData : ActionData
    {
        public override Action CreateAction()
        {
            return new GoToWaterAction(m_Cost, m_Precondition, m_Effect);
        }
    }
}
