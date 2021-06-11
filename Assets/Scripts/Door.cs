using UnityEngine;

/**
 * A compenent used to control a door with an animator. A key game object
 * can be set to open the door.
 */
public class Door : MonoBehaviour, Interactable
{
    /**
     * The states a door can be in
     */
    private enum DoorState
    {
        OPEN,
        CLOSED
    }

    /**The key used to open the door**/
    [SerializeField]
    private GameObject key;

    /**The sound played when opening and closing the door**/
    [SerializeField]
    private AudioClip interactSound;

    /**The current state of the door**/
    private DoorState currentDoorState = DoorState.CLOSED;
    /**The animator component of the door**/
    private Animator animator;
    /**The audio source component of the door**/
    private AudioSource audioSource;

    /**
     * Sets up the animator and audio source componemnts of the game object
     */
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = interactSound;
    }

    /**
     * Controls the state of the door and sets the appropriate trigger on the
     * animator
     */
    public void Interact(Player player)
    {
        if(key == null || player.CheckInventoryForItem(key))
        {
            if(currentDoorState == DoorState.CLOSED)
            {
                currentDoorState = DoorState.OPEN;
                animator.SetTrigger("Open");
            }
            else
            {
                currentDoorState = DoorState.CLOSED;
                animator.SetTrigger("Close");
            }
            audioSource.Stop();
            audioSource.Play();
        }
    }
}
