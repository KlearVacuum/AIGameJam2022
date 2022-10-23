using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/WaterTile")]
class WaterTile : PathfindingTile
{
    [SerializeField] WaterInteractable m_WaterInteractable;
    public override bool IsWalkable => true;
}