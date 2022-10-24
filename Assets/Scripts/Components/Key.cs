using UnityEngine;
class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Agent agent = collision.gameObject.GetComponent<Agent>();
            agent.audioGetKey.PlayOneShot(agent.aSource);
            agent.PickupKey();
            gameObject.SetActive(false);
        }
    }
}