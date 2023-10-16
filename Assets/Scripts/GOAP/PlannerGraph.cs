using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class PlannerGraph
    {
        public abstract class INode : IComparable<INode>
        {
            private readonly INode m_Parent;
            public INode Parent => m_Parent;

            private List<INode> m_Children;

            public List<INode> Children => m_Children;

            private float m_Cost;

            protected INode(INode parent, float cost)
            {
                m_Parent = parent;
                m_Children = null;
                m_Cost = cost;
            }

            public virtual void AddChild(INode child)
            {
                m_Children.Add(child);
            }

            public int CompareTo(INode other)
            {
                return GetCost().CompareTo(other.GetCost());
            }

            public float GetCost()
            {
                return m_Cost;
            }

            public abstract ScriptableObject GetItem();
            public abstract bool Validate(Blackboard worldState);
            public abstract bool IsSatisfiedBy(Effect effect);
            public abstract string GetName();
        }

        public class ActionNode : INode
        {
            private readonly Action m_Action;
            public Action Action => m_Action;

            public ActionNode(ref INode parent, ref Action action, float cost)
                : base(parent, cost)
            {
                m_Action = action;
            }

            public override ScriptableObject GetItem()
            {
                return m_Action;
            }

            public override bool Validate(Blackboard worldState)
            {
                return m_Action.Validate(worldState);
            }

            public override bool IsSatisfiedBy(Effect effect)
            {
                return m_Action.IsSatisfiedBy(effect);
            }

            public override string GetName()
            {
                return m_Action.GetName();
            }
        }

        public class GoalNode : INode
        {
            private readonly Goal m_Goal;
            public Goal Goal => m_Goal;

            public GoalNode(ref Goal goal, float cost)
                : base(null, cost)
            {
                m_Goal = goal;
            }

            public override ScriptableObject GetItem()
            {
                return m_Goal;
            }

            public override bool Validate(Blackboard worldState)
            {
                return false;
            }

            public override bool IsSatisfiedBy(Effect effect)
            {
                return m_Goal.IsSatisfiedBy(effect);
            }

            public override string GetName()
            {
                return m_Goal.GetName();
            }
        }

        private GoalNode m_GoalNode;

        public PlannerGraph(Goal goal, List<Action> availableActions)
        {
            int numActions = availableActions.Count;
            BitArray usedActions = new BitArray(numActions);
            Queue<INode> openList = new Queue<INode>();

            m_GoalNode = new GoalNode(ref goal, 0);

            openList.Enqueue(m_GoalNode);

            while (openList.Count > 0)
            {
                INode currentNode = openList.Dequeue();

                for (int i = 0; i < numActions; ++i)
                {
                    Action action = availableActions[i];

                    if (usedActions.Get(i) == false &&
                       currentNode.IsSatisfiedBy(action.GetEffect()))
                    {
                        ActionNode newNode = new ActionNode(ref currentNode, ref action, action.GetCost());

                        openList.Enqueue(newNode);
                        usedActions.Set(i, true);
                        currentNode.AddChild(newNode);
                    }
                }
            }
        }

        public static Plan GetBestPlan(List<Action> availableActions, Blackboard worldState, Goal goal)
        {
            int numActions = availableActions.Count;
            BitArray usedActions = new BitArray(numActions);
            PriorityQueue<INode> openList = new PriorityQueue<INode>();
            Plan bestPlan = null;

            float bestRunningCost = float.MaxValue;

            openList.Push(new GoalNode(ref goal, 0));

            while (openList.Count > 0)
            {
                INode currentNode = openList.Pop();

                if (currentNode.Validate(worldState))
                {
                    List<Action> actionList = new List<Action>();
                    float currentRunningCost = 0;

                    while (currentNode is ActionNode)
                    {
                        Action action = currentNode.GetItem() as Action;

                        currentRunningCost += action.GetCost();
                        actionList.Add(action);

                        currentNode = currentNode.Parent;
                    }

                    if (currentRunningCost < bestRunningCost)
                    {
                        // actionList.Reverse();
                        bestPlan = new Plan(goal, actionList);
                        bestRunningCost = currentRunningCost;
                    }

                    continue;
                }

                for (int i = 0; i < numActions; ++i)
                {
                    if (usedActions.Get(i) == true)
                    {
                        continue;
                    }

                    Action action = availableActions[i];

                    if (currentNode.IsSatisfiedBy(action.GetEffect()))
                    {
                        float cost = action.GetCost() + currentNode.GetCost();

                        usedActions.Set(i, true);
                        openList.Push(new ActionNode(ref currentNode, ref action, cost));
                    }
                }
            }

            return bestPlan;
        }

        public Plan GetBestPlan(ref Blackboard worldState)
        {
            Plan bestPlan = null;
            PriorityQueue<INode> openList = new PriorityQueue<INode>();
            Dictionary<string, float> costTable = new Dictionary<string, float>();

            // Initialize
            costTable.Add(m_GoalNode.GetName(), 0);
            openList.Push(m_GoalNode);

            while (openList.Count > 0)
            {
                INode currentNode = openList.Pop();

                if (currentNode.Validate(worldState))
                {
                    return BuildPlan(currentNode);
                }

                foreach (INode childNode in currentNode.Children)
                {
                    float totalCost = childNode.GetCost() + currentNode.GetCost();

                    costTable.Add(childNode.GetName(), totalCost);
                }
            }

            return bestPlan;
        }

        private Plan BuildPlan(INode node)
        {
            List<Action> actions = new List<Action>();

            do
            {
                actions.Add(node.GetItem() as Action);
                node = node.Parent;

            } while (node.Parent != null && node.Parent is not GoalNode);

            return actions.Count > 0 ? new Plan(m_GoalNode.GetItem() as Goal, actions) : null;
        }
    }
}
