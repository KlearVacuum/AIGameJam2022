using GOAP;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Planner/Actions/GoToElectricity")]
class GoToElectricityAction : GOAP.Action
{
    public override void Initialize(Agent agent)
    {
        PathQuery pathQuery = new PathQuery();

        bool addedRequest = agent.AddPathRequestToClosestTileOfType<ElectricTrapTile>(
            pathQuery, 
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

    public override string GetName() => "GoToElectricity";

    public override bool IsValid(Blackboard worldState)
    {
        // Need to check if it is still wet
        return true;
    }
}
