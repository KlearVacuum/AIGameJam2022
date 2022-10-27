using System;
using System.Collections;

using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Fan")]
public class FanInteractable : Interactable
{
    Fan m_Fan;

    public void Initialize(Fan fan)
    {
        m_Fan = fan;
    }

    public override void Interact(Agent agent, Vector3 tilePosition)
    {
        // Activate fan
        m_Fan.Activate();

        if(agent.Status is FrozenStatus)
        {
            return;
        }

        if (!agent.IsBlown)
        {
            agent.SetCoroutine(MoveAgentToWindPosition(agent, m_Fan.WindPosition));
        }

        agent.SetIsBlown(true);
        agent.SetFan(m_Fan);

        // Debug.Log("Blow");
    }

    public override void ExitTrigger(Agent agent)
    {
        m_Fan.Deactivate();
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