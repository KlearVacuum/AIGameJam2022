using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class PathfindingTile : Tile
{
    public abstract bool IsWalkable { get; }
}
