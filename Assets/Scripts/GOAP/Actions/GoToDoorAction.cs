using GOAP;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Planner/Actions/GoToDoor")]
class GoToDoorAction : GOAP.Action
{
    public override void Initialize(Agent agent)
    {
        // Find Key
        Door door = FindObjectOfType<Door>();

        Debug.Assert(door != null, "There is no door in the world!");

        agent.AddPathRequest(door.transform.position, (Agent agent) =>
        {
            Complete(agent.WorldState);
        });
    }

    public override void Execute(Agent agent)
    {
        // Go towards water
        Path path = agent.GetCurrentPath();

        if (path != null)
        {
            Vector3 newPosition = path.Update(agent);
            agent.transform.position = newPosition;
        }
    }

    public override void Exit(Agent agent)
    {
        Dictionary<string, IStateValue> desiredState = new Dictionary<string, IStateValue>();

        desiredState.Add("ExitedRoom", new StateValue<bool>(true));

        agent.AddPlanRequest(desiredState);
    }

    public override string GetName() => "GoToDoorAction";

    public override bool IsValid(Blackboard worldState)
    {
        // Need to check if it is still wet
        return true;
    }
}
