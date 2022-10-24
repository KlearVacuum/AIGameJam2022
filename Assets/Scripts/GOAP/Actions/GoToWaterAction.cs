using GOAP;
using UnityEngine;


[CreateAssetMenu(menuName = "Planner/Actions/GoToWater")]
class GoToWaterAction : GOAP.Action
{
    public override void Execute(Agent agent)
    {
        // Go towards water
    }

    public override string GetName() => "GoToWaterAction";

    public override bool IsValid(Blackboard worldState)
    {
        // Need to check if it is still wet
        return true;
    }
}
