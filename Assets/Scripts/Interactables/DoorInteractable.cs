using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Door")]
public class DoorInteractable : Interactable
{
    Door m_Door;

    public void Initialize(Door door)
    {
        m_Door = door;
    }

    public override void Interact(Agent agent, Vector3 interactablePosition)
    {
        agent.Stop();
        m_Door.Open(agent);
    }
}