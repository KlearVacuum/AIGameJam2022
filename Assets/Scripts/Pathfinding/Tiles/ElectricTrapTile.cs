using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/ElectricTrapTile")]
class ElectricTrapTile : PathfindingTile
{
    [SerializeField] ElectricityInteractable m_ElectricitytInteractable;
    
    public override bool IsWalkable => true;
}