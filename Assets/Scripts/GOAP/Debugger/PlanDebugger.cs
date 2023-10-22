using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GOAP
{
    public enum ePlanType
    {
        SIMPLE,
        DETAILED
    }

    public class ActionDebuggerUIData
    {
        public ActionDebugger actionDebugger;
        public Image backPanel;
        public NodePanelElement headerNodePanelElement;
        public List<NodePanelElement> nodePanelElementList;

        public Color activeColor = Color.white;
        public Color inactiveColor = new Color(1,1,1,0.4f);

        public void SetActive(bool active)
        {
            Color desiredCOlor = active ? activeColor : inactiveColor;
            backPanel.color = desiredCOlor;
            headerNodePanelElement.SetTextColor(desiredCOlor);

            if (nodePanelElementList == null || nodePanelElementList.Count <= 0) return;
            foreach (var element in nodePanelElementList)
            {
                element.SetTextColor(desiredCOlor);
            }
        }
    }

    public class PlanDebugger : MonoBehaviour
    {
        public bool m_ShowPlan;
        public ePlanType m_PlanType;

        Plan m_Plan = null;
        List<ActionDebugger> m_ActionDebuggers = new List<ActionDebugger>();
        ActionDebugger m_ActiveActionDebugger;
        GoalDebugger m_GoalDebugger;

        [SerializeField] GameObject m_NodePanelPrefab;
        [SerializeField] NodePanelElement m_NodePanelElementPrefab;

        List<ActionDebuggerUIData> m_ActionDebuggerUIDataList = new List<ActionDebuggerUIData>();

        void Start()
        {
            // toggles are set to false by default
            m_ShowPlan = false;
            m_PlanType = ePlanType.SIMPLE;
            // gameObject.SetActive(false);
        }

        public void Setup(Plan plan)
        {
            m_Plan = plan;
            m_Plan.AddOnNewActionExecuteListener(OnChangeActiveAction);
        }

        private void OnChangeActiveAction()
        {
            // Note, could create a pool instead of destroying each time
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            if (!m_ShowPlan) return;

            m_ActiveActionDebugger = new ActionDebugger(m_Plan.GetCurrentAction());

            //var elements = m_ActiveActionDebugger.CreateNodePanelElements(m_NodePanelElementPrefab);
            //foreach(NodePanelElement element in elements)
            //{
            //    element.transform.SetParent(transform);
            //}
            m_ActionDebuggers.Clear();
            for (int i = 0; i < m_Plan.m_Actions.Count; ++i)
            {
                ActionDebugger newAction = new ActionDebugger(m_Plan.m_Actions[i]);
                m_ActionDebuggers.Add(newAction);
            }

            m_ActionDebuggerUIDataList.Clear();
            switch (m_PlanType)
            {
                case ePlanType.SIMPLE:
                    // group each action into separate panels
                    for (int i = 0; i < m_ActionDebuggers.Count; ++i)
                    {
                        ActionDebuggerUIData uiData = new ActionDebuggerUIData();
                        uiData.actionDebugger = m_ActionDebuggers[i];
                        m_ActionDebuggerUIDataList.Add(uiData);

                        GameObject newPanel = Instantiate(m_NodePanelPrefab, transform);
                        uiData.backPanel = newPanel.GetComponent<Image>();

                        var headerElement = m_ActionDebuggers[i].CreateHeaderNodePanelElement(m_NodePanelElementPrefab);
                        uiData.headerNodePanelElement = headerElement;
                        headerElement.transform.SetParent(newPanel.transform);

                        uiData.SetActive(uiData.actionDebugger.Action == m_ActiveActionDebugger.Action);
                    }
                    break;
                case ePlanType.DETAILED:
                    // group each action into separate panels
                    for (int i = 0; i < m_ActionDebuggers.Count; ++i)
                    {
                        ActionDebuggerUIData uiData = new ActionDebuggerUIData();
                        uiData.actionDebugger = m_ActionDebuggers[i];
                        m_ActionDebuggerUIDataList.Add(uiData);

                        GameObject newPanel = Instantiate(m_NodePanelPrefab, transform);
                        uiData.backPanel = newPanel.GetComponent<Image>();

                        var headerElement = m_ActionDebuggers[i].CreateHeaderNodePanelElement(m_NodePanelElementPrefab);
                        uiData.headerNodePanelElement = headerElement;
                        headerElement.transform.SetParent(newPanel.transform);

                        var elements = m_ActionDebuggers[i].CreateNodePanelElements(m_NodePanelElementPrefab);
                        uiData.nodePanelElementList = elements;

                        foreach (NodePanelElement element in elements)
                        {
                            element.transform.SetParent(newPanel.transform);
                        }
                        uiData.SetActive(uiData.actionDebugger.Action == m_ActiveActionDebugger.Action);
                    }
                    break;
            }
        }
    }
}



