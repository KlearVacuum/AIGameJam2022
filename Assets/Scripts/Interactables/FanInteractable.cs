using System;
using System.Collections;

using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Fan")]
public class FanInteractable : Interactable
{
    FanTile m_FanTile;

    public void Initialize(FanTile fanTile)
    {
        m_FanTile = fanTile;
    }

    public override void Interact(Agent agent)
    {
        // Activate fan
        m_FanTile.ActivateFan();

        // Set agent status
        agent.SetStatus(m_Status);
        agent.ClearPath();
        agent.SetCoroutine(MoveAgentToWindPosition(agent, m_FanTile.GetWindPosition()));
    }

    IEnumerator MoveAgentToWindPosition(Agent agent, Vector3 windPosition)
    {
        float totalTime = (agent.transform.position - windPosition).magnitude / agent.Speed;
        float t = 0;

        while(t < totalTime)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, windPosition, agent.Speed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        // Set 
        Rigidbody2D rigidbody2D = agent.GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }
}