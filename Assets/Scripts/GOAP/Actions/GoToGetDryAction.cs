using GOAP;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Planner/Actions/GoToGetDry")]
class GoToGetDryAction : GOAP.Action
{
    public override void Initialize(Agent agent)
    {
        PathQuery pathQuery = new PathQuery();

        agent.ClearCurrentPath();
        agent.ClearPathPlanningRequests();

        bool addedRequest = agent.AddPathRequestToClosestTileOfType<FireplaceTile>(
            (Agent agent) =>
            {
                Complete(agent.WorldState);
            });
    }

    public override void Execute(Agent agent)
    {
        Path path = agent.GetCurrentPath();

        if (path != null)
        {
            Vector3 newPosition = path.Update(agent);
            agent.transform.position = newPosition;
        }
    }

    public override string GetName() => "GoToGetDry";

    public override bool CheckIfValid(Blackboard worldState)
    {
        // No longer wet, should replan
        if(worldState.GetStateValue<bool>("IsWet") ==  false)
        {
            return false;
        }

        return true;
    }
}
