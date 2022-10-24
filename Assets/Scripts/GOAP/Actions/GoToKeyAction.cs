using GOAP;
using UnityEngine;

[CreateAssetMenu(menuName = "Planner/Actions/GoToKey")]
class GoToKeyAction : GOAP.Action
{
    public override void Execute(Agent agent)
    {
        // Go towards water
    }

    public override string GetName() => "GoToKeyAction";

    public override bool IsValid(Blackboard worldState)
    {
        // Need to check if it is still wet
        return true;
    }
}
