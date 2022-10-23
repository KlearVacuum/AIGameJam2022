using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/FanTile")]
class FanTile : PathfindingTile
{
    [SerializeField] Fan m_FanPrefab;
    [SerializeField] Fan.Direction m_FanDirection;

    Fan m_Fan;
    Vector3 m_FanPosition;

    bool m_Activated = false;
    public override bool IsWalkable => m_Activated ? false : true;

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        if(go == null)
        {
            return false;
        }

        InteractableTrigger interactableTrigger = go.GetComponent<InteractableTrigger>(); 

        if(interactableTrigger != null)
        {
            // Instantiate fan
            m_Fan = Instantiate<Fan>(m_FanPrefab);
            m_Fan.transform.position = go.transform.position;

            // offset trigger
            go.transform.position += GetDirectionVector();
            return true;
        }

        return false;
    }

    public void Activate()
    {
        m_Activated = true;
    }

    public void Deactivate()
    {
        m_Activated = false;
    }

    Vector3Int GetDirectionVector()
    {
        switch(m_FanDirection)
        {
            case Fan.Direction.UP: return Vector3Int.up;
            case Fan.Direction.DOWN: return Vector3Int.down;
            case Fan.Direction.LEFT: return Vector3Int.left;
            case Fan.Direction.RIGHT: return Vector3Int.right;
        }

        throw new System.ComponentModel.InvalidEnumArgumentException("Invalid fan direction!");
    }
}