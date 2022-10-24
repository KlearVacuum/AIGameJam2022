using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : ScriptableObject
{
    [SerializeField] GOAP.IStateData m_Condition;

    private void OnEnable()
    {
        m_Condition = Instantiate(m_Condition);
    }

    public GOAP.IStateData GetCondition()
    {
        return Instantiate(m_Condition);
    }
}
