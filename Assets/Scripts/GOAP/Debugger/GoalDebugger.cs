using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GOAP
{
    class GoalDebugger : NodeDebugger
    {
        Goal m_Goal;

        public GoalDebugger(Goal goal)
        {
            m_Goal = goal;
        }

        public string Stringify()
        {
            return String.Format(
                    "Goal: {0} \n Conditions : {1} Effects : {2}",
                    m_Goal.GetName(),
                    StringifyStateDatas(m_Goal.GetEffects().Results),
                    StringifyStateDatas(m_Goal.GetPreconditions().Conditions));
        }
    }
}
