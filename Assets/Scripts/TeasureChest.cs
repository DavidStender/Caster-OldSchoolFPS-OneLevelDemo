using UnityEngine;

/**
 * A component used to create a treasure chest game object that the player can interact with.
 */
public class TeasureChest : MonoBehaviour, Interactable
{
    /**The treasure in the treasure chest**/
    [SerializeField]
    private GameObject treasure;

    /**The location to spawn the treasure at when the chest is open**/
    [SerializeField]
    private Transform treasureSpawn;

    /**The sound to play when the chest is open**/
    [SerializeField]
    private AudioClip openSound;

    /**The animator component on the treasure chest**/
    private Animator animator;
    /**The audio source component on the treasure chest**/
    private AudioSource audioSource;

    /**
     * Initializes components on the treasure chest
     */
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    /**
     * Triggers the chest to open and spawn the treasure
     * 
     * Player (player): The player interacting with the treasure chest
     */
    public void Interact(Player player)
    {
        if(!treasure.activeSelf)
        {
            animator.SetTrigger("Open");
            audioSource.PlayOneShot(openSound);
        }
    }

    /**
     * Spawns the treasure at the treasureSpawn location. Called during the opening animation.
     */
    private void SpawnTreasure()
    {
        treasure.transform.position = treasureSpawn.position;
        treasure.SetActive(true);
    }
}
