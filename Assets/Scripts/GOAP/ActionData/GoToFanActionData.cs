using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    [CreateAssetMenu(menuName = "Planner/Actions/GoToFanAction")]
    class GoToFanActionData : ActionData
    {
        public override Action CreateAction()
        {
            return new GoToFanAction(m_Cost, m_Precondition, m_Effect);
        }
    }
}
