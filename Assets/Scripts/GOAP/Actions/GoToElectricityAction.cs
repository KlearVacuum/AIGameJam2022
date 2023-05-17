using GOAP;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

class GoToElectricityAction : GOAP.Action
{
    public GoToElectricityAction(float cost, Precondition precondition, Effect effect)
           : base(cost, precondition, effect)
    {
    }

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

    public override bool CheckIfValid(Blackboard worldState)
    {
        // Replan if wet
        if(worldState.GetStateValue<bool>("IsWet") == true)
        {
            NotifyFailure();
            return false;
        }

        return true;
    }
}
