﻿using GOAP;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

class GoToFanAction : GOAP.Action
{
    public GoToFanAction(float cost, Precondition precondition, Effect effect)
              : base(cost, precondition, effect)
    {
    }

    public override void Initialize(Agent agent)
    {
        bool addedRequest = agent.AddPathRequestToClosestTileOfType<WindTile>(
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

    public override string GetName() => "GoToFanAction";

    public override bool CheckIfValid(Blackboard worldState)
    {
        if(worldState.GetStateValue<bool>("IsFrozen") == true)
        {
            NotifyFailure();
            return false;
        }

        return true;
    }
}
