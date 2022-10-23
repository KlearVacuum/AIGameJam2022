using UnityEngine;
using UnityEngine.Tilemaps;

class FireTile : PathfindingTile
{
    [SerializeField] protected FireInteractable m_FireInteractable;

    public override bool IsWalkable => true;
}