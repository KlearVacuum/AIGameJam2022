using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/FloorTile")]
class FloorTile : PathfindingTile
{
    public override bool IsWalkable => true;
}