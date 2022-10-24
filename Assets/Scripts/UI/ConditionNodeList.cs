using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNodeList : ScriptableObject
{
    [SerializeField] List<ConditionNode> m_ConditionNodes;
    List<ConditionNode> ConditionNodes => m_ConditionNodes;
}
