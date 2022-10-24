using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : MonoBehaviour
{
    [SerializeField] GOAP.IStateData m_Condition;

    private void Awake()
    {
        m_Condition = Instantiate(m_Condition);
    }

    public GOAP.IStateData GetCondition()
    {
        return Instantiate(m_Condition);
    }
}
