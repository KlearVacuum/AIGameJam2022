using UnityEngine;
using UnityEngine.Tilemaps;

public class Fan : MonoBehaviour
{
    public enum Direction { UP, DOWN, LEFT, RIGHT };

    Vector3 m_WindPosition;
    public Vector3 WindPosition => m_WindPosition;

    [SerializeField] AreaEffector2D m_AreaEffector2D;

    public void SetWindPosition(Vector3 windPosition)
    {
        m_WindPosition = windPosition;
        m_AreaEffector2D.transform.position = windPosition;
    }

    public void SetWindDirection(Direction direction)
    {
        switch(direction)
        {
            case Direction.DOWN:
                // -90.f
                m_AreaEffector2D.forceAngle = -90f;
                break;

            case Direction.UP:
                m_AreaEffector2D.forceAngle = 90f;
                break;
            case Direction.LEFT:
                m_AreaEffector2D.forceAngle = 180f;
                break;
            case Direction.RIGHT:
                m_AreaEffector2D.forceAngle = 0f;
                break;
        }
    }

}