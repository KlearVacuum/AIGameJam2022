using GOAP;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Planner/Actions/GoToKey")]
class GoToKeyAction : GOAP.Action
{
    public override void Initialize(Agent agent)
    {
        // Find Key
        Key key = FindObjectOfType<Key>();

        Debug.Assert(key != null, "There is no key in the world!");

        agent.AddPathRequest(key.transform.position);
    }

    public override void Execute(Agent agent)
    {
        // Go towards water
        Path path = agent.GetCurrentPath();

        if(path != null)
        {
            Vector3 newPosition = path.Update(agent);
            agent.transform.position = newPosition;

            if(path.Completed)
            {
                Complete(agent.WorldState);
            }
        }
    }

    public override void Exit(Agent agent)
    {
        Dictionary<string, IStateValue> desiredState = new Dictionary<string, IStateValue>();

        desiredState.Add("ExitedRoom", new StateValue<bool>(true));

        agent.AddPlanRequest(desiredState);
    }

    public override string GetName() => "GoToKeyAction";

    public override bool IsValid(Blackboard worldState)
    {
        // Need to check if it is still wet
        return true;
    }
}
