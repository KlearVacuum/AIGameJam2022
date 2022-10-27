using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/FanTile")]
public class FanTile : PathfindingTile
{
    [SerializeField] Fan m_FanPrefab;
    [SerializeField] Fan.Direction m_FanDirection;

    Fan m_Fan;
    public Fan Fan => m_Fan;

    public override bool IsWalkable => m_Fan.gameObject.activeSelf ? false : true;

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        if(go == null)
        {
            return false;
        }

        InteractableTrigger interactableTrigger = go.GetComponent<InteractableTrigger>(); 

        if(interactableTrigger != null)
        {
            FanInteractable fanInteractable = interactableTrigger.Interactable as FanInteractable;

            // Instantiate fan
            m_Fan = Instantiate(m_FanPrefab);
            m_Fan.transform.position = go.transform.position;
            // DeactivateFan();

            // offset trigger
            go.transform.position += GetDirectionVector();
            go.transform.parent = m_Fan.gameObject.transform;
            m_Fan.SetWindPosition(go.transform.position);
            m_Fan.SetWindDirection(m_FanDirection);
            m_Fan.SetWindParticleDirection(m_FanDirection);

            // Initialize fan interactable
            fanInteractable.Initialize(m_Fan);

            return true;
        }

        return false;
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