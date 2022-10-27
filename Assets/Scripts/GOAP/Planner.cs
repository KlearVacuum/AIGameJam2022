using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    [System.Serializable]
    public class Planner
    {
        [SerializeField] List<GOAP.Action> m_DefaultActions = new List<GOAP.Action>();
        [SerializeField] List<Action> m_AvailableActions = new List<Action>();
        [SerializeField] List<Goal> m_AvailableGoals = new List<Goal>();

        Queue<PlanRequest> m_PlanRequests = new Queue<PlanRequest>();
        Dictionary<string, IStateValue> m_PreviousDesiredState;

        Plan m_CurrentPlan = null;

        bool m_Paused = false;
        public bool IsPaused => m_Paused;
        private Agent m_Agent;

        public void Initialize(Agent agent)
        {
            m_Agent = agent;
        }

        public void AddPlanRequest(Dictionary<string, IStateValue> desiredState)
        {
            m_PlanRequests.Enqueue(new PlanRequest(desiredState));
        }

        public Plan GetCurrentPlan()
        {
            EvaluateCurrentPlan();

            if(m_CurrentPlan == null || m_PlanRequests.Count > 0)
            {
                PlanRequest planRequest = m_PlanRequests.Dequeue();
                m_PreviousDesiredState = new Dictionary<string, IStateValue>(planRequest.DesiredState);
                m_CurrentPlan = Plan(planRequest.DesiredState);
            }

            return m_CurrentPlan;
        }

        public void RestartPlan(Blackboard worldState)
        {
            if(m_PlanRequests.Count > 0)
            {
                return;
            }

            m_CurrentPlan = null;

            if (m_PreviousDesiredState == null)
            {
                m_PreviousDesiredState = new Dictionary<string, IStateValue>();

                if(worldState.GetStateValue<bool>("HasKey") == false)
                {
                    m_PreviousDesiredState.Add("HasKey", new StateValue<bool>(true));
                }
                else if (worldState.GetStateValue<bool>("ExitedRoom") == false)
                {
                    m_PreviousDesiredState.Add("ExitedRoom", new StateValue<bool>(true));
                }
            }

            m_PlanRequests.Enqueue(new PlanRequest(m_PreviousDesiredState));
        }

        public void PausePlanner()
        {
            m_Paused = true;
        }

        public void ResumePlanner()
        {
            m_Paused = false;
        }
        private void EvaluateCurrentPlan()
        {
            if (m_CurrentPlan == null)
            {
                return;
            }

            if (m_CurrentPlan.IsComplete() || !m_CurrentPlan.IsValid())
            {
                if (IsPreviousDesiredStateFulfilled() == false)
                {
                    // Continue to try to satisfy previous desired state if it hasn't been achieved.
                    AddPlanRequest(m_PreviousDesiredState);
                }
                else
                {
                    m_CurrentPlan = null;
                }
            }            
        }

        private Plan Plan(Dictionary<string, IStateValue> desiredState)
        {
            Goal bestGoal = GetBestGoal(desiredState);

            if (bestGoal == null)
            {
                Debug.Log($"Failed to find a plan for " + desiredState);
                return null;
            }

            List<Action> availableActions = new List<Action>(m_AvailableActions);

            foreach(Action action in m_DefaultActions)
            {
                availableActions.Add(action);
            }

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
        
        public void AssignActions(List<Action> actions)
        {
            m_AvailableActions = actions;
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

        private bool IsPreviousDesiredStateFulfilled()
        {
            // If there is no previous desired state, then there is nothing to do
            if(m_PreviousDesiredState == null)
            {
                return true;
            }

            return m_Agent.WorldState.Fulfills(m_PreviousDesiredState);
        }
    }
}

