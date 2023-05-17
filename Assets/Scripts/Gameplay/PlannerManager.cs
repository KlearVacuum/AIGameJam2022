using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannerManager : MonoBehaviour
{
    [SerializeField] Agent m_Agent;
    [SerializeField] ActionNodePanel m_PanelPrefab;
    [SerializeField] List<ActionNode> m_ActionNodes = new List<ActionNode>();
    [SerializeField] GameObject actionPanel;

    List<ActionNodePanel> m_ActionNodePanels;

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

        foreach (ActionNode actionNode in m_ActionNodes)
        {
            ActionNodePanel actionNodePanel = Instantiate(m_PanelPrefab, actionPanel.transform);

            actionNodePanel.Initialize(actionNode);

            m_ActionNodePanels.Add(actionNodePanel);
        }
    }

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
        m_Agent.Replan();
    }
}
