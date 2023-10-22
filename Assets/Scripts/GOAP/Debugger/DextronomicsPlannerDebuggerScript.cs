using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DextronomicsPlannerDebuggerScript : MonoBehaviour
{
    public GameObject m_DebuggerPanel;
    private GOAP.PlanDebugger m_PlanDebugger;

    void Awake()
    {
        m_PlanDebugger = GameObject.FindGameObjectWithTag("PlanDebugger").GetComponent<GOAP.PlanDebugger>();
    }
    void Start()
    {
        m_DebuggerPanel.SetActive(false);
    }

    public void ToggleDebuggerPanel()
    {
        m_DebuggerPanel.SetActive(!m_DebuggerPanel.activeInHierarchy);
    }

    public void ToggleShowPlan()
    {
        m_PlanDebugger.m_ShowPlan = !m_PlanDebugger.m_ShowPlan;
    }

    public void ToggleSetDetailed()
    {
        if (m_PlanDebugger.m_PlanType == GOAP.ePlanType.SIMPLE)
        {
            m_PlanDebugger.m_PlanType = GOAP.ePlanType.DETAILED;
        }
        else
        {
            m_PlanDebugger.m_PlanType = GOAP.ePlanType.SIMPLE;
        }
    }
}
