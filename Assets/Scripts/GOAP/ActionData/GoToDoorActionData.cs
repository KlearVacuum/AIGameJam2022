using GOAP;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Planner/Actions/GoToDoor")]
class GoToDoorActionData : GOAP.ActionData
{
    public override Action CreateAction()
    {
        return new GoToDoorAction();
    }

}
