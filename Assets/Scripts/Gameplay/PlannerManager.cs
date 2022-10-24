using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannerManager : MonoBehaviour
{
    [SerializeField] Agent m_Agent;
    [SerializeField] List<ActionNode> m_AvailableActionNodes = new List<ActionNode>();
    [SerializeField] List<ConditionNode> m_AvailableConditionNodes = new List<ConditionNode>();

    List<ActionNode> m_AddedActionNodes = new List<ActionNode>();

    bool m_SimulationRunning = false;
    public bool SimulationRunning => m_SimulationRunning;

    private void Awake()
    {
        Debug.Assert(m_Agent != null, "Agent has not been set in Planner Manager");
    }

    public void AddActionNode(ActionNode actionNode)
    {
        if(m_AddedActionNodes.Contains(actionNode) == false)
        {
            m_AddedActionNodes.Add(actionNode);
        }
    }

    public void RemoveActionNode(ActionNode actionNode)
    {
        if(m_AddedActionNodes.Contains(actionNode))
        {
            m_AddedActionNodes.Remove(actionNode);
        }
    }

    public void StartSimulation()
    {
        SetupAgent();
        m_SimulationRunning = true;
    }

    private void SetupAgent()
    {
        List<GOAP.Action> actions = new List<GOAP.Action>();

        foreach(ActionNode actionNode in m_AddedActionNodes)
        {
            actions.Add(actionNode.GetAction());
        }

        m_Agent.AddActions(actions);
    }
}
