using UnityEngine;

/**
 * A component that used to send an event to the enemy component
 */
 [RequireComponent(typeof(Enemy))]
public class EnemyTriggerEvent : MonoBehaviour
{
    /**The message being sent**/
    [SerializeField]
    private string message;

    /**The game object's enemy component**/
    private Enemy enemy;

    /**
     * Gets the enemy component in a parent game object
     */
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    /**
     * Sends the event message to the enemy component when the player enters
     * the trigger zone.
     */
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            enemy.TriggerEventHandler(message, other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            enemy.TriggerEventHandler(message+" Exit", other.gameObject);
        }
    }
}
