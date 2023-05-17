using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    [CreateAssetMenu(menuName = "Planner/Actions/GoToKeyAction")]
    class GoToKeyActionData : ActionData
    {
        public override Action CreateAction()
        {
            return new GoToKeyAction(m_Cost, m_Precondition, m_Effect);
        }
    }
}
