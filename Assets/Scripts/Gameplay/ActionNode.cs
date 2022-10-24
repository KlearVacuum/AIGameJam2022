using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : MonoBehaviour
{
    [SerializeField] GOAP.Action m_Action;
    List<ConditionNode> m_ConditionNodes = new List<ConditionNode>(); 

    private void Awake()
    {
        m_Action = Instantiate(m_Action);
    }

    public void AddCondition(ConditionNode conditionNode)
    {
        m_ConditionNodes.Add(conditionNode);
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
