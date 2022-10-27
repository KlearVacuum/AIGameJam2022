using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/FanTile")]
public class FanTile : PathfindingTile
{
    [SerializeField] Fan m_FanPrefab;
    [SerializeField] Fan.Direction m_FanDirection;

    public override bool IsWalkable => false;

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
            Fan fan = Instantiate(m_FanPrefab);
            fan.transform.position = go.transform.position;
            // DeactivateFan();

            // offset trigger
            go.transform.position += GetDirectionVector();
            go.transform.parent = fan.gameObject.transform;
            fan.SetWindPosition(go.transform.position);
            fan.SetWindDirection(m_FanDirection);
            fan.SetWindParticleDirection(m_FanDirection);

            // Initialize fan interactable
            fanInteractable.Initialize(fan);

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