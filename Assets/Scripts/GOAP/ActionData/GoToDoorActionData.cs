using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    [CreateAssetMenu(menuName = "Planner/Actions/GoToDoorAction")]
    class GoToDoorActionData : ActionData
    {
        public override Action CreateAction()
        {
            return new GoToDoorAction(m_Cost, m_Precondition, m_Effect);
        }
    }
}