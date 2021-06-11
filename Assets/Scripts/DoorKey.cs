using System.Collections;
using UnityEngine;

/**
 * A Game object component that is used to let the player open a door.
 */
public class DoorKey : MonoBehaviour
{
    /**The sound played when the key is picked up**/
    [SerializeField]
    private AudioClip pickupSound;

    /**The audio source component of the game object**/
    private AudioSource audioSource;

    /**
     * Gets the audio source component on the game object.
     */
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /**
     * Calls Pickup if the player enters the trigger zone of the game object
     */
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            StartCoroutine("Pickup", other.gameObject);
        }
    }

    /**
     * Deactivates the box collider trigger, and the sprite render of the game object.
     * Adds the key to the players inventory. Then waits a few seconds and deactivated itself.
     */
    IEnumerator Pickup(GameObject triggerer)
    {
        audioSource.PlayOneShot(pickupSound);
        GetComponentInChildren<BoxCollider>().gameObject.SetActive(false);
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
        triggerer.GetComponent<Player>().AddItemToInventory(gameObject);
        yield return new WaitForSecondsRealtime(3f);
        gameObject.SetActive(false);
    }
}
