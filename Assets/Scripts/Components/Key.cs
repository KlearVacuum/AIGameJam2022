using UnityEngine;
class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Agent agent = collision.gameObject.GetComponent<Agent>();
            agent.PickupKey();
            Destroy(gameObject);
        }
    }
}