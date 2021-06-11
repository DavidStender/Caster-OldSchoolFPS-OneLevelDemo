using System.Collections;
using UnityEngine;

/**
 * Creates a treasure game object used to complete objectives on a level.
 */
public class Treasure : MonoBehaviour
{
    /**The sound played when the treasure is picked up by the player**/
    [SerializeField]
    private AudioClip pickupSound;

    /**The audio source component on the treasure**/
    private AudioSource audioSource;

    /**A delegate that sends a message out when the state of the treasure changes**/
    public delegate void TreasureAction(Treasure treasure);
    /**A event message that is sent when a treasire is picked up**/
    public static event TreasureAction TreasurePickedUp;

    /**
     * Initializes the audio source component
     */
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /**
     * Is triggered when the player enters the pick up trigger zone of the treasure
     */
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if (TreasurePickedUp != null)
            {
                StartCoroutine("Pickup");
            }
        }
    }

    /**
     * Deactivates the box collider, mesh renderer, treasure component then waits a set amount of time before sending out
     * the TreasurePickUp message.
     */
    IEnumerator Pickup()
    {
        audioSource.PlayOneShot(pickupSound);
        GetComponent<BoxCollider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<Treasure>().enabled = false;
        yield return new WaitForSecondsRealtime(3f);
        gameObject.SetActive(false);
        TreasurePickedUp(this);
    }
}
