using UnityEngine;

public abstract class Interactable : ScriptableObject
{
    [SerializeField] protected Status m_Status;

    public abstract void Interact(Agent agent);
}