using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Node/ActionNode")]
public class ActionNode : ScriptableObject
{
    [SerializeField] GOAP.Action m_Action;
    [SerializeField] ConditionNodeList m_ConditionNodeList;

    public ConditionNodeList ConditionList => m_ConditionNodeList; 

    // This is always a copy
    GOAP.IStateData m_SelectedCondition = null;

    private void Awake()
    {
        m_Action = Instantiate(m_Action);
    }

    public GOAP.Action GetAction()
    {
        GOAP.Action newAction = Instantiate(m_Action);

        if(m_SelectedCondition != null)
        {
            newAction.AddPrecondition(m_SelectedCondition);
        }

        return newAction;
    }
    
    public void SetConditionUsingIndex(int conditionIndex)
    {
        List<ConditionNode> conditionNodes = m_ConditionNodeList.ConditionNodes;

        if(conditionIndex == 0)
        {
            m_SelectedCondition = null;
        }
        else if ((conditionIndex - 1) < conditionNodes.Count)
        {
            m_SelectedCondition = conditionNodes[conditionIndex].GetCondition();   
        }
    }
}
