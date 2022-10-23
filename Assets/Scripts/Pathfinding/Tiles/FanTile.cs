using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/FanTile")]
class FanTile : PathfindingTile
{
    bool m_Activated = false;

    [SerializeField] WaterInteractable m_WaterInteractable;
    public override bool IsWalkable => m_Activated ? false : true;

    public void Activate()
    {
        m_Activated = true;
    }

    public void Deactivate()
    {
        m_Activated = false;
    }
}