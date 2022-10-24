using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : MonoBehaviour
{
    [SerializeField] GOAP.Action m_Action;

    private void Awake()
    {
        m_Action = Instantiate(m_Action);
    }

    public void AddCondition(ConditionNode conditionNode)
    {
        m_Action.AddPrecondition(conditionNode.GetCondition());
    }

    public GOAP.Action GetAction()
    {
        return Instantiate(m_Action);
    }
}
