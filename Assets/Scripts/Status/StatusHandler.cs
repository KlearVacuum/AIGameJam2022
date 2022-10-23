using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
class StatusHandler
{
    [SerializeField] Status m_CurrentStatus;
    Status m_PreviousStatus;

    Queue<Status> m_StatusQueue = new Queue<Status>();

    public Status CurrentStatus => m_CurrentStatus;
    public Status PreviousStatus;
    
    public void QueueStatus(Status status)
    {
        m_StatusQueue.Enqueue(status);
    }

    public void Update(Agent agent)
    {
        while(m_StatusQueue.Count > 0)
        {
            StatusEffect effect = TransitionTo(m_StatusQueue.Dequeue());

            if(effect != null)
            {
                effect.Apply(agent);
            }
        }
    }

    public StatusEffect TransitionTo(Status newStatus)
    {
        StatusEffect effect = newStatus.TransitionFrom(m_CurrentStatus);

        if(effect != null)
        {
            m_PreviousStatus = m_CurrentStatus;
            m_CurrentStatus = newStatus;
        }

        return effect;
    }
}