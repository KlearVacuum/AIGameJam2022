using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/FireplaceTile")]
class FireplaceTile : FireTile
{
    public override bool IsWalkable => true;
}