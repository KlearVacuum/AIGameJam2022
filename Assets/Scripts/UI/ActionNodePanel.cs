using TMPro;
using UnityEngine;
using System.Collections.Generic;

class ActionNodePanel : MonoBehaviour
{
    [SerializeField] ActionNode m_ActionNode;
    [SerializeField] TMPro.TMP_Dropdown m_Dropdown;


    private void Awake()
    {
        // Make a deep copy of the action node at runtime
        m_ActionNode = Instantiate(m_ActionNode);
    }

    private void Start()
    {
        ConditionNodeList conditionList = m_ActionNode.ConditionList;
        List<string> optionList = new List<string>();
        
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
        
    }
}