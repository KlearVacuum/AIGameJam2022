using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannerManager : MonoBehaviour
{
    [SerializeField] Agent m_Agent;
    [SerializeField] ActionNodePanel m_PanelPrefab;
    [SerializeField] List<ActionNodePanelData> m_ActionNodePanelDataList = new List<ActionNodePanelData>();

    List<ActionNodePanel> m_ActionNodePanels;
    // [SerializeField] List<ConditionNode> m_AvailableConditionNodes = new List<ConditionNode>();

    // List<ActionNode> m_AddedActionNodes = new List<ActionNode>();

    bool m_SimulationRunning = false;
    public bool SimulationRunning => m_SimulationRunning;

    private void Awake()
    {
        // Debug.Assert(m_Agent != null, "Agent has not been set in Planner Manager");
    }

    private void Start()
    {
        m_Agent = GlobalGameData.playerGO.GetComponent<Agent>();
        m_ActionNodePanels = new List<ActionNodePanel>();

        foreach (ActionNodePanelData actionNodePanelData in m_ActionNodePanelDataList)
        {
            ActionNodePanel actionNodePanel = Instantiate(m_PanelPrefab, transform);

            actionNodePanel.Initialize(actionNodePanelData.ActionNode);

            m_ActionNodePanels.Add(actionNodePanel);
        }
    }

    //public void AddActionNode(ActionNode actionNode)
    //{
    //    if(m_AddedActionNodes.Contains(actionNode) == false)
    //    {
    //        m_AddedActionNodes.Add(actionNode);
    //    }
    //}

    //public void RemoveActionNode(ActionNode actionNode)
    //{
    //    if(m_AddedActionNodes.Contains(actionNode))
    //    {
    //        m_AddedActionNodes.Remove(actionNode);
    //    }
    //}

    public void StartSimulation()
    {
        SetupAgent();
        m_SimulationRunning = true;
    }

    public void Update()
    {
        if(m_SimulationRunning)
        {
            m_Agent.UpdateAgent();
        }
    }

    private void SetupAgent()
    {
        List<GOAP.Action> actions = new List<GOAP.Action>();

        foreach(ActionNodePanel actionNodePanel in m_ActionNodePanels)
        {
            actions.Add(actionNodePanel.ActionNode.GetAction());
        }

        m_Agent.AssignActions(actions);
    }
}
