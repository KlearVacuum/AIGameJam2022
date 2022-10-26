using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Node/ActionNode")]
public class ActionNode : ScriptableObject
{
    [SerializeField] GOAP.Action m_ActionReference;
    [SerializeField] ConditionNodeList m_ConditionNodeList;

    GOAP.Action m_ActionInstance;

    public string ActionName => m_ActionInstance.name;
    public ConditionNodeList ConditionList => m_ConditionNodeList; 

    // This is always a copy
    GOAP.IStateData m_SelectedCondition = null;

    private void OnEnable()
    {
        if(m_ActionReference != null)
        {
            m_ActionInstance = Instantiate(m_ActionReference);
        }
    }

    public GOAP.Action GetAction()
    {
        GOAP.Action newAction = m_ActionInstance;

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
            m_SelectedCondition = conditionNodes[conditionIndex - 1].GetCondition();   
        }
    }
}
