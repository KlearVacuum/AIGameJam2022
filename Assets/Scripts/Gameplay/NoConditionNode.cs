using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoConditionNode : ConditionNode
{
    public new string ConditionName => "No Condition";

    public override GOAP.IStateData GetCondition()
    {
        return null;
    }
}