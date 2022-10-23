using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/FanTile")]
public class FanTile : PathfindingTile
{
    [SerializeField] Fan m_FanPrefab;
    [SerializeField] Fan.Direction m_FanDirection;

    Fan m_Fan;

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
            DeactivateFan();

            // Initialize fan interactable
            fanInteractable.Initialize(this);

            // offset trigger
            go.transform.position += GetDirectionVector();
            m_Fan.SetWindPosition(go.transform.position);
            m_Fan.SetWindDirection(m_FanDirection);

            return true;
        }

        return false;
    }

    public void ActivateFan()
    {
        m_Fan.gameObject.SetActive(true);
    }

    public void DeactivateFan()
    {
        m_Fan.gameObject.SetActive(false);
    }

    public Vector3 GetWindPosition()
    {
        return m_Fan.WindPosition;
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