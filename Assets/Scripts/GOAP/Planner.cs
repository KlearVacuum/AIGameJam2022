using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    [System.Serializable]
    public class Planner
    {
        [SerializeField] List<Action> m_AvailableActions = new List<Action>();
        [SerializeField] List<Goal> m_AvailableGoals = new List<Goal>();

        private Agent m_Agent;
        public Planner(Agent agent)
        {
            m_Agent = agent;
        }

        public Plan Plan(Dictionary<string, IStateValue> desiredState)
        {
            Goal bestGoal = GetBestGoal(desiredState);

            if (bestGoal == null)
            {
                Debug.Log($"Failed to find a plan for " + desiredState);
                return null;
            }

            List<Action> availableActions = new List<Action>(m_AvailableActions);

            Plan bestPlan = PlannerGraph.GetBestPlan(availableActions, m_Agent.WorldState, bestGoal);

            if (bestPlan == null)
            {
                Debug.Log($"Failed to find a plan for " + bestGoal);
            }

            return bestPlan;
        }

        public void AddAction(Action actionToAdd)
        {
            bool actionAlreadyExists = m_AvailableActions.Exists((Action action) =>
            {
                return action == actionToAdd;
            });


            if (actionAlreadyExists == false)
            {
                m_AvailableActions.Add(actionToAdd);
                // Debug.Log($"Added new action << {actionToAdd.GetName()} >> .");
            }
            else
            {
                Debug.LogWarning($"Attempting to add an action << {actionToAdd.GetName()} >> that already exists.");
            }
        }

        public void AddGoal(Goal goalToAdd)
        {
            bool goalAlreadyExists = m_AvailableGoals.Exists((Goal goal) =>
            {
                return goal == goalToAdd;
            });

            if (goalAlreadyExists == false)
            {
                m_AvailableGoals.Add(goalToAdd);
                // Debug.Log($"Added new goal << {goalToAdd.GetName()} >>.");
            }
            else
            {
                Debug.LogWarning($"Attempting to add an action << {goalToAdd.GetName()} >> that already exists.");
            }
        }

        private Goal GetBestGoal(Dictionary<string, IStateValue> desiredState)
        {
            Goal bestGoal = null;
            float bestCost = float.MaxValue;

            foreach (Goal goal in m_AvailableGoals)
            {
                if (goal.Satifies(desiredState) && goal.GetPriority() < bestCost)
                {
                    bestGoal = goal;
                }
            }

            return bestGoal;
        }
    }
}

