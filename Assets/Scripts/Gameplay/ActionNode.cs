using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Node/ActionNode")]
public class ActionNode : ScriptableObject
{
    [SerializeField] GOAP.ActionData m_ActionData;
    [SerializeField] ConditionNodeList m_ConditionNodeList;

    public GOAP.ActionData ActionData => m_ActionData;

    GOAP.Action m_ActionInstance;

    public string ActionName => m_ActionInstance.GetName();
    public ConditionNodeList ConditionList => m_ConditionNodeList; 

    // This is always a copy
    GOAP.IStateData m_SelectedCondition = null;

    private void OnEnable()
    {
        // To be fixed, need to find a way to store the name..
        m_ActionInstance = ActionData.CreateAction();
    }

    public GOAP.Action GetAction()
    {
        GOAP.Action newAction = m_ActionData.CreateAction();

        newAction.ClearPreconditions();

        if(m_SelectedCondition != null)
        {
            newAction.AddPrecondition(m_SelectedCondition);
            Debug.Log("Adding condition " + m_SelectedCondition.name);
        }

        return newAction;
    }
    
    public void SetConditionUsingIndex(int conditionIndex)
    {
        List<ConditionNode> conditionNodes = m_ConditionNodeList.ConditionNodes;

        m_ActionInstance.RemovePrecondition(m_SelectedCondition);

        if(conditionIndex == 0)
        {
            m_SelectedCondition = null;
        }
        else if ((conditionIndex - 1) < conditionNodes.Count)
        {
            m_SelectedCondition = conditionNodes[conditionIndex - 1].GetCondition();
            Debug.Log(this.name + " Setting condition " + m_SelectedCondition);
        }

    }
}
