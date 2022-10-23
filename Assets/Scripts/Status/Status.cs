using System;
using UnityEngine;

public abstract class Status : ScriptableObject
{
    [SerializeField] protected Sprite m_Sprite;
    public Sprite Sprite => m_Sprite;

    public virtual StatusEffect TransitionFrom(Status otherStatus)
    {
        throw new NotImplementedException();
    }
}