using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(menuName = "GOAP/Planner Data", order = 1)]
    public class PlannerData : ScriptableObject
    {
        [SerializeReference] private List<Action> m_ActionList;
        [SerializeReference] private List<Goal> m_GoalList;
    }
}

