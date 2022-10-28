using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Node/ConditionNode")]
public class ConditionNode : ScriptableObject
{
    [SerializeField] GOAP.IStateData m_ConditionReference;
    GOAP.IStateData m_ConditionInstance;
    public GOAP.IStateData Condition => m_ConditionInstance;

    public string ConditionName => m_ConditionName;

    private string m_ConditionName;

    private void OnEnable()
    {
        m_ConditionInstance = Instantiate(m_ConditionReference);
        m_ConditionName = m_ConditionReference.name;
    }

    public virtual GOAP.IStateData GetCondition()
    {
        return m_ConditionInstance;
    }
}
