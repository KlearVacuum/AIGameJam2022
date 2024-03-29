﻿using UnityEngine;

public abstract class Interactable : ScriptableObject
{
    [SerializeField] protected Status m_Status;

    public abstract void Interact(Agent agent, Vector3 interactablePosition);
    public virtual void ExitTrigger(Agent agent)
    {

    }
}