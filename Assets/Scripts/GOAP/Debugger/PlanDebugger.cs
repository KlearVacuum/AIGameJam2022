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

        [SerializeField] private TextMeshProUGUI m_ActionNameText;

        public void Setup(Plan plan)
        {
            m_Plan = plan;
            // Activate once first
            OnChangeActiveAction();

            m_Plan.AddOnChangeCurrentActionListener(OnChangeActiveAction);
        }

        private void OnChangeActiveAction()
        {
            m_ActiveActionDebugger = new ActionDebugger(m_Plan.GetCurrentAction());
            m_ActionNameText.text = m_ActiveActionDebugger.Stringify();    
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



