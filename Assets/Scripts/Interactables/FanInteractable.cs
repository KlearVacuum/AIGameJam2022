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

    public override void Interact(Agent agent, Vector3 tilePosition)
    {
        // Activate fan
        m_FanTile.ActivateFan();

        if(agent.Status is FrozenStatus)
        {
            return;
        }

        if (!agent.IsBlown)
        {
            agent.SetCoroutine(MoveAgentToWindPosition(agent, m_FanTile.GetWindPosition()));
        }

        agent.SetIsBlown(true);
        agent.SetFan(m_FanTile.Fan);

        Debug.Log("Blow");
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
        agent.audioBlow.PlayOneShot(agent.aSource);
        Rigidbody2D rigidbody2D = agent.GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }
}