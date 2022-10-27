using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace GOAP
{
    public abstract class Action : ScriptableObject
    {
        public enum ExecutionStatus
        {
            None,
            Failed,
            Executing,
            Succeeded
        }

        [SerializeField] protected float m_Cost;
        [SerializeField] protected Precondition m_Precondition = new Precondition();
        [SerializeField] protected Effect m_Effect = new Effect();

        ExecutionStatus m_ExecutionStatus = ExecutionStatus.None;

        protected Agent m_Agent;

        public void OnEnable()
        {
            m_ExecutionStatus = ExecutionStatus.None;
        }

        public abstract string GetName();
        public float GetCost() => m_Cost;
        public Precondition GetPreconditions() => m_Precondition;
        public Effect GetEffect() => m_Effect;

        public virtual void Initialize(Agent agent) { }
        public abstract void Execute(Agent agent);

        public virtual void Exit(Agent agent) { }

        public virtual void Abort(Agent agent) { }

        public abstract bool CheckIfValid(Blackboard worldState);

        public virtual bool Validate(Blackboard worldState)
        {
            return m_Precondition.Validate(worldState);
        }

        public virtual void ClearPreconditions()
        {
            m_Precondition.Conditions.Clear();
        }

        public virtual void RemovePrecondition(IStateData condition)
        {
            m_Precondition.RemoveCondition(condition);
        }

        public ExecutionStatus GetStatus() => m_ExecutionStatus;
        public bool Succeeded() => m_ExecutionStatus == ExecutionStatus.Succeeded;
        public bool Failed() => m_ExecutionStatus == ExecutionStatus.Failed;


        public virtual void NotifyExecuting()
        {
            bool isValid = m_ExecutionStatus == ExecutionStatus.None
                        || m_ExecutionStatus == ExecutionStatus.Executing;

            Debug.Assert(isValid, $"Attempting to re-execute a completed/failed Action : {GetName()}");

            m_ExecutionStatus = ExecutionStatus.Executing;
        }

        public void ResetStatus()
        {
            m_ExecutionStatus = ExecutionStatus.None;
        }

        public virtual void NotifySuccess()
        {
            m_ExecutionStatus = ExecutionStatus.Succeeded;
        }

        public virtual void NotifyFailure()
        {
            m_ExecutionStatus = ExecutionStatus.Failed;
        }

        public bool IsSatisfiedBy(Effect effect)
        {
            return effect.Satisfies(m_Precondition);
        }

        public void AddPrecondition(IStateData precondition)
        {
            bool preconditionExists =
                m_Precondition.Conditions.Exists((IStateData stateData) =>
                {
                    return stateData.Equals(precondition);
                });

            if (preconditionExists == false)
            {
                m_Precondition.Conditions.Add(precondition);
            }
        }

        protected void Complete(Blackboard worldState)
        {
            m_ExecutionStatus = ExecutionStatus.Succeeded;

            foreach (IStateData result in m_Effect.Results)
            {
                worldState.SetStateValue(result);
            }
        }
    }
}


