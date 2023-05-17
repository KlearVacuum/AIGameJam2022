using GOAP;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

class GoToWaterAction : GOAP.Action
{
    public GoToWaterAction(float cost, Precondition precondition, Effect effect)
                 : base(cost, precondition, effect)
    {
    }

    public override void Initialize(Agent agent)
    {
        agent.ClearCurrentPath();
        agent.ClearPathPlanningRequests();
        bool addedRequest = agent.AddPathRequestToClosestTileOfType<WaterTile>(
            new PathQuery(), 
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

    public override string GetName() => "GoToWaterAction";

    public override bool CheckIfValid(Blackboard worldState)
    {
        // Need to check if it is still wet
        return true;
    }
}
