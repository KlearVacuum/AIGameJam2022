using System;
using UnityEngine;

class SpeedBoost : MonoBehaviour
{
    Agent m_Agent;
    DefaultStatus m_DefaultStatus;

    float m_OriginalSpeed;
    float m_Timer = 0;
    float m_Duration;

    public void Initialize(Agent agent, DefaultStatus defaultStatus, float duration, float boostAmount)
    {
        m_DefaultStatus = defaultStatus;

        m_Agent = agent;
        m_OriginalSpeed = m_Agent.Speed;
        m_Agent.SetSpeed(boostAmount);

        m_Duration = duration;
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;

        if(m_Timer >= m_Duration)
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        m_Agent.SetSpeed(m_OriginalSpeed);
        m_Agent.ApplyStatus(m_DefaultStatus);


        Path path = m_Agent.GetCurrentPath();

        if(path != null)
        {
            PathQuery pathQuery = new PathQuery(path.PathQuery);

            pathQuery.RemoveFilter<WaterTile>();

            if(m_Agent.WorldState.GetStateValue<bool>("IsWet") == false)
            {
                pathQuery.AddFilter<FireTile>();
            }

            m_Agent.ReplanWithNewPathQuery(pathQuery);
        }
    }
}