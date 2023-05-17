using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    [CreateAssetMenu(menuName = "Planner/Actions/GoToGetMeltedAction")]
    class GoToGetMeltedActionData : ActionData
    {
        public override Action CreateAction()
        {
            return new GoToGetMeltedAction(m_Cost, m_Precondition, m_Effect);
        }
    }
}
