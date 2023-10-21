using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GOAP
{
    public class PlanDebugger : MonoBehaviour
    {
        Plan m_Plan = null;
        List<ActionDebugger> m_ActionDebuggers = new List<ActionDebugger>();
        ActionDebugger m_ActiveActionDebugger;
        GoalDebugger m_GoalDebugger;

        [SerializeField] NodePanelElement m_NodePanelElementPrefab;

        public void Setup(Plan plan)
        {
            m_Plan = plan;
            m_Plan.AddOnNewActionExecuteListener(OnChangeActiveAction);
        }

        private void OnChangeActiveAction()
        {
            // Note, could create a pool instead of destroying each time
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            m_ActiveActionDebugger = new ActionDebugger(m_Plan.GetCurrentAction());

            var elements = m_ActiveActionDebugger.CreateNodePanelElements(m_NodePanelElementPrefab);
            foreach(NodePanelElement element in elements)
            {
                element.transform.SetParent(transform);
            }
        }

        string GetDebugString()
        {
            foreach(ActionDebugger actionDebugger in m_ActionDebuggers)
            {
                if (actionDebugger.Action == m_Plan.GetCurrentAction())
                {
                    return actionDebugger.Stringify();
                }
            }

            return "NO ACTIVE ACTION";
        }
    }
}



