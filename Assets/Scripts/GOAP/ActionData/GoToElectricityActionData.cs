using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    [CreateAssetMenu(menuName = "Planner/Actions/GoToElectricityAction")]
    class GoToElectricityActionData : ActionData
    {
        public override Action CreateAction()
        {
            return new GoToElectricityAction(m_Cost, m_Precondition, m_Effect);
        }
    }
}
