using TMPro;
using UnityEngine;
using System.Collections.Generic;

class ActionNodePanel : MonoBehaviour
{
    ActionNode m_ActionNode;
    [SerializeField] TMPro.TMP_Dropdown m_Dropdown;

    public ActionNode ActionNode => m_ActionNode;

    public void Initialize(ActionNode actionNode)
    {
        ConditionNodeList conditionList = actionNode.ConditionList;
        List<string> optionList = new List<string>();

        m_ActionNode = Instantiate(actionNode);
        m_Dropdown.ClearOptions();

        optionList.Add("No Condition");

        foreach (ConditionNode conditionNode in conditionList.ConditionNodes)
        {
            optionList.Add(conditionNode.ConditionName);
        }

        m_Dropdown.AddOptions(optionList);
    }

    public void OnSelectOption(int optionSelected)
    {
        m_ActionNode.SetConditionUsingIndex(optionSelected);
    }
}