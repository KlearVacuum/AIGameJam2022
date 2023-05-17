using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    [CreateAssetMenu(menuName = "Planner/Actions/GoToGetDryAction")]
    class GoToGetDryActionData : ActionData
    {
        public override Action CreateAction()
        {
            return new GoToGetDryAction(m_Cost, m_Precondition, m_Effect);
        }
    }
}
