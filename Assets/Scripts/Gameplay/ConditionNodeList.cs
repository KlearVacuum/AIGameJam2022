using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Node/ConditionNodeList")]
public class ConditionNodeList : ScriptableObject
{
    [SerializeField] List<ConditionNode> m_ConditionNodes = new List<ConditionNode>();
    public List<ConditionNode> ConditionNodes => m_ConditionNodes;
}
