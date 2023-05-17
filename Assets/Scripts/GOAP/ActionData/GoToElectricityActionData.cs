using GOAP;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Planner/Actions/GoToElectricity")]
class GoToElectricityActionData : GOAP.ActionData
{
    public override Action CreateAction()
    {
        return new GoToDoorAction();
    }
}
