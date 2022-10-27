using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/WindTile")]
class WindTile : PathfindingTile
{
    public override bool IsWalkable => true;
}