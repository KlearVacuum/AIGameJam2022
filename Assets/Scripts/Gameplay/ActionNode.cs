using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : MonoBehaviour
{
    [SerializeField] GOAP.Action m_Action;
    [SerializeField] ConditionNodeList m_ConditionNodeList;
    
    ConditionNode m_SelectedConditioNode = null;



    private void Awake()
    {
        m_Action = Instantiate(m_Action);
    }

    public GOAP.Action GetAction()
    {
        GOAP.Action newAction = Instantiate(m_Action);

        foreach (ConditionNode conditionNode in m_ConditionNodes)
        {
            newAction.AddPrecondition(conditionNode.GetCondition());    
        }

        return newAction;
    }
}
