using UnityEngine;
using UnityEngine.Tilemaps;

public class Fan : MonoBehaviour
{
    public enum Direction { UP, DOWN, LEFT, RIGHT };

    Vector3 m_WindPosition;
    public Vector3 WindPosition => m_WindPosition;

    [SerializeField] AreaEffector2D m_AreaEffector2D;
    [SerializeField] GameObject windParticles;
    GameObject windParticlesInstance;
    ParticleSystem particles;

    private void Awake()
    {
        windParticlesInstance = Instantiate(windParticles);
        particles = windParticlesInstance.GetComponent<ParticleSystem>();
    }

    public void SetWindPosition(Vector3 windPosition)
    {
        m_WindPosition = windPosition;
        m_AreaEffector2D.transform.position = windPosition;
        windParticlesInstance.transform.position = windPosition;
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
    public void SetWindParticleDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.DOWN:
                windParticlesInstance.transform.Rotate(new Vector3(0, 0, -90));
                break;

            case Direction.UP:
                windParticlesInstance.transform.Rotate(new Vector3(0, 0, 90));
                break;
            case Direction.RIGHT:
                windParticlesInstance.transform.Rotate(new Vector3(0, 0, -180));
                break;
        }
    }

    public void Activate()
    {
        PlayParticles(true);
    }

    public void Deactivate()
    {
        PlayParticles(false);
    }

    public void PlayParticles(bool play)
    {
        if (play)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
    }

}