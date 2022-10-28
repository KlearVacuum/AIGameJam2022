using GOAP;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Planner/Actions/GoToGetMelted")]
class GoToGetMeltedAction : GOAP.Action
{
    public override void Initialize(Agent agent)
    {
        PathQuery pathQuery = new PathQuery();

        bool addedRequest = agent.AddPathRequestToClosestTileOfType<FireTile>(
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

    public override string GetName() => "GoToGetMelted";

    public override bool CheckIfValid(Blackboard worldState)
    {
        // No longer wet, should replan
        if(worldState.GetStateValue<bool>("IsFrozen") == false)
        {
            return false;
        }

        return true;
    }
}
