using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/DoorTile")]
public class DoorTile : PathfindingTile
{
    [SerializeField] Door.Type m_DoorType;
    [SerializeField] Door m_DoorPrefab;

    Door m_Door;
    public Door Door => m_Door;

    public override bool IsWalkable => true;

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        if(go == null)
        {
            return false;
        }

        InteractableTrigger interactableTrigger = go.GetComponent<InteractableTrigger>(); 

        if(interactableTrigger != null)
        {
            DoorInteractable doorInteractable = interactableTrigger.Interactable as DoorInteractable;

            // Instantiate door
            m_Door = Instantiate(m_DoorPrefab);
            m_Door.transform.position = go.transform.position;

            if(m_DoorType == Door.Type.HORIZONTAL)
            {
                Vector3 rotationVector = new Vector3(0, 0, 90);

                m_Door.transform.Rotate(rotationVector);
                go.transform.Rotate(rotationVector);
            }

            doorInteractable.Initialize(m_Door);

            return true;
        }

        return false;
    }
}