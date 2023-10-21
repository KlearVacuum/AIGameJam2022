using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace GOAP
{
    public class Plan
    {
        public enum ExecutionStatus
        {
            None,
            Executing,
            Succeeded,
            Failed
        }

        public Goal m_Goal { get; private set; }
        public List<Action> m_Actions { get; private set; }
        int m_CurrentActionIndex = 0;
        ExecutionStatus m_ExecutionStatus = ExecutionStatus.None;
        System.Action m_OnNewActionExecute;

        public Plan(Goal goal, List<Action> actions)
        {
            m_Goal = goal;
            m_Actions = actions;

            foreach (Action action in actions)
            {
                action.ResetStatus();
            }
        }

        public void AddOnNewActionExecuteListener(System.Action action)
        {
            m_OnNewActionExecute = action;
        }

        public void Execute(Agent agent)
        {
            switch (m_ExecutionStatus)
            {
                case ExecutionStatus.None:
                    {
                        m_ExecutionStatus = ExecutionStatus.Executing;
                        goto case ExecutionStatus.Executing;
                    }
                case ExecutionStatus.Executing:
                    {
                        ExecuteInternal(agent);
                        break;
                    }
                default: break;
            }
        }

        private void ExecuteInternal(Agent agent)
        {
            if (IsValid())
            {
                Action currentAction = m_Actions[m_CurrentActionIndex];

                switch (currentAction.GetStatus())
                {
                    case Action.ExecutionStatus.None:
                        {
                            currentAction.NotifyExecuting();
                            currentAction.Initialize(agent);
                            m_OnNewActionExecute.Invoke();
                            goto case Action.ExecutionStatus.Executing;
                        }
                    case Action.ExecutionStatus.Executing:
                        {
                            currentAction.CheckIfValid(agent.WorldState);
                            currentAction.Execute(agent);
                            break;
                        }
                    case Action.ExecutionStatus.Succeeded:
                        {
                            if (m_Goal.IsSatisfiedBy(currentAction.GetEffect()))
                            {
                                currentAction.Exit(agent);
                                m_ExecutionStatus = ExecutionStatus.Succeeded;
                            }
                            else
                            {
                                ProgressToNextAction();
                            }
                            break;
                        }
                    case Action.ExecutionStatus.Failed:
                        {
                            currentAction.Abort(agent);
                            m_ExecutionStatus = ExecutionStatus.Failed;
                            break;
                        }
                    default: break;
                }
            }
        }

        private void ProgressToNextAction()
        {
            ++m_CurrentActionIndex;
        }

        public bool IsValid() => m_ExecutionStatus != ExecutionStatus.Failed && m_CurrentActionIndex < m_Actions.Count;

        public bool IsComplete() => m_ExecutionStatus == ExecutionStatus.Succeeded;

        public Action GetCurrentAction()
        {
            return IsValid() ? m_Actions[m_CurrentActionIndex] : null;
        }
    }
}
