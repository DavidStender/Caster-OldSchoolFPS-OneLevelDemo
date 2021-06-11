using UnityEngine;

/**
 * A component used to create a switch that triggers another game object to do something.
 */
public class Switch : MonoBehaviour, Interactable
{
    /**The game object to send an activation message to**/
    [SerializeField]
    private GameObject activatedGameObject;

    /**The sound played when the swithc is interacted with**/
    [SerializeField]
    private AudioClip interactSound;

    /**The animator component of the switch**/
    private Animator animator;
    /**The animator of the object being activated**/
    private Animator activatedGameObjectAnimator;
    /**The audio source component of the switch**/
    private AudioSource audioSource;
    /**If the switch has been activated or not**/
    private bool isActivated = false;

    /**
     * Initializes several components of the switch.
     */
    private void Awake()
    {
        animator = GetComponent<Animator>();
        activatedGameObjectAnimator = activatedGameObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    /**
     * Activates the activatedGameObject. (Such as lowering a draw bridge)
     * 
     * Player (player): The player component of the game object interacting with the switch
     */
    public void Interact(Player player)
    {
        if (!isActivated)
        {
            isActivated = true;
            audioSource.PlayOneShot(interactSound);
            animator.SetTrigger("Activated");
            activatedGameObjectAnimator.SetTrigger("Activated");
            activatedGameObject.GetComponent<AudioSource>().Play();
        }
    }
}
