using UnityEngine;

public abstract class Interactable : ScriptableObject
{
    [SerializeField] protected Status m_Status;
    [SerializeField] protected AudioClipGroup m_Audio;

    public abstract void Interact(Agent agent, Vector3 interactablePosition);
}