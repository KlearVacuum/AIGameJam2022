using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/WaterTile")]
class WaterTile : PathfindingTile
{
    public override bool IsWalkable => true;
}