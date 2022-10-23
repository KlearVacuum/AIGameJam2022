using UnityEngine;

[System.Serializable]
class StatusHandler
{
    [SerializeField] Status m_CurrentStatus;

    public StatusEffect TransitionTo(Status newStatus)
    {
        StatusEffect effect = newStatus.TransitionFrom(m_CurrentStatus);

        m_CurrentStatus = newStatus;

        return effect;
    }
}