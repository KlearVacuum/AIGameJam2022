using System;
using UnityEngine;

public abstract class Status : ScriptableObject
{
    [SerializeField] protected Sprite m_Sprite;
    [SerializeField] protected Color m_LightColor;
    public Sprite Sprite => m_Sprite;
    public Color LightColor => m_LightColor;

    public virtual StatusEffect TransitionFrom(Status otherStatus)
    {
        throw new NotImplementedException();
    }
}