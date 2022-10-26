using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Node/ConditionNode")]
public class ConditionNode : ScriptableObject
{
    [SerializeField] GOAP.IStateData m_ConditionReference;
    GOAP.IStateData m_ConditionInstance;
    public GOAP.IStateData Condition => m_ConditionInstance;

    public string ConditionName => m_ConditionInstance.name;

    private void OnEnable()
    {
        m_ConditionInstance = Instantiate(m_ConditionReference);
    }

    public virtual GOAP.IStateData GetCondition()
    {
        return m_ConditionInstance;
    }
}
