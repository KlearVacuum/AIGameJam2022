using UnityEngine;

[System.Serializable]
class StatusHandler
{
    [SerializeField] Status m_CurrentStatus;
    Status m_PreviousStatus;

    public Status CurrentStatus => m_CurrentStatus;
    public Status PreviousStatus;
    
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