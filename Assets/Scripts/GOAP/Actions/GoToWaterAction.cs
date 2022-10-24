using GOAP;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Planner/Actions/GoToWater")]
class GoToWaterAction : GOAP.Action
{
    public override void Initialize(Agent agent)
    {
        bool addedRequest = agent.AddPathRequestToClosestTileOfType<WaterTile>(
            new PathQuery(), 
            (Agent agent) =>
            {
                Complete(agent.WorldState);
            });

        Debug.LogWarning("No water tile found in go to water action");
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

    public override bool IsValid(Blackboard worldState)
    {
        // Need to check if it is still wet
        return true;
    }
}
