using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/WallTile")]
class WallTile : PathfindingTile
{
    public override bool IsWalkable => false;
}