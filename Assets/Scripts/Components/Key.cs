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
            agent.WorldState.SetStateValue("HasKey", true);
            gameObject.SetActive(false);
        }
    }
}