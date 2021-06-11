using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/**
 * Creates a player game object That can take damage shoot and reload a gun and collect objects 
 * in an inventory. Player game object must use the player Tag. Allows both keyboard and windows xbox
 * controller to be used.
 */
public class Player : MonoBehaviour
{
    /**The max health the player can have**/
    [SerializeField]
    private int maxHealth = 100;

    /**The currently equip gun for the player**/
    [SerializeField]
    private GameObject activeGunGO;

    /**The sound played when the player takes damage**/
    [SerializeField]
    private AudioClip hurtSound;

    /**If the player is alive or not**/
    public bool IsAlive { get; private set; } // experimenting with unity and properties
    /**Controls when the player can take damage**/
    private bool invincibleFrames = false;
    /**The current health of the player**/
    private int currentHealth;
    /**The gun component of the currently equip gun**/
    private Gun activeGun;
    /**The list of items in the players inventory**/
    private List<GameObject> inventory;
    /**The animator component on the player**/
    private Animator animator;
    /**A UI controller component for the player**/
    private PlayerUIController playerUIController;
    /**The audio source component on the player**/
    private AudioSource audioSource;

    /**
     * Initializes several components on the player.
     */
    private void Awake()
    {
        activeGun = activeGunGO.GetComponent<Gun>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        inventory = new List<GameObject>();
        playerUIController = GetComponent<PlayerUIController>();
        gameObject.tag = "Player";
        audioSource = GetComponent<AudioSource>();
    }

    /**
     * Updates the ui just before the level starts and sets the player to be alive.
     */
    private void Start()
    {
        playerUIController.UpdateHealthText(currentHealth, maxHealth);
        IsAlive = true;
    }

    /**
     * Handles input to control aspects of the player such as shooting, reloading and interacting with game objects.
     */
    private void LateUpdate()
    {
        if (activeGun != null && IsAlive && Time.timeScale > 0)
        {
            
            if (Input.GetButtonDown("Reload"))
            {
                if(activeGun.CheckValidReload())
                {
                    animator.SetTrigger("Reload");
                    activeGun.Reload();
                }
                
            }
            else if (Input.GetButton("Fire"))
            {
                activeGun.Shoot();
            }
            else if (Input.GetAxisRaw("Fire") > 0.7f && activeGun.GetCanFire())
            {
                activeGun.Shoot();
            }
            else if(Input.GetButtonDown("Interact"))
            {
                Interact();
            }
        }

        if(!activeGun.GetCanFire() && Input.GetAxisRaw("Fire") < 0.1f)
        {
            activeGun.ResetCanFire();
        }
    }

    /**
     * Subtracts the damage amount from the players current health and updates the palyers UI. If the damage amount is greater
     * or equal too the current health sets the dying animation.
     * 
     * int (damageAmount): The amount of the damage being done.
     * GameObject (attacker): The game object attacking the player.
     */
    public void TakeDamage(int damageAmount, GameObject attacker)
    {
        if(IsAlive && !invincibleFrames)
        {
            if (damageAmount >= currentHealth)
            {
                currentHealth = 0;
                playerUIController.UpdateHealthText(currentHealth, maxHealth);
                IsAlive = false;
                animator.SetTrigger("Died");
            }
            else
            {
                currentHealth -= damageAmount;
                playerUIController.UpdateHealthText(currentHealth, maxHealth);
                StartCoroutine("ActivateInvincibleFrames");
            }
            audioSource.PlayOneShot(hurtSound, 0.4f);
        }
    }

    /**
     * Ends the level and sends loads the end game screen.
     */
    private void Die()
    {
        GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>().TriggerLevelEnd();
    }

    /**
     * Tells the player when the reload is complete. Called at the end of the reload animation.
     */
    private void ReloadComplete()
    {
        activeGun.ResetIsReloading();
    }

    /**
     * Box casts in front of the player to look for any interactable components
     */
    private void Interact()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + new Vector3(0f, 0.25f, 0f), new Vector3(0.75f, 1.25f, 0.75f), transform.TransformDirection(Vector3.forward), Quaternion.identity, 3f);
        foreach(RaycastHit hit in hits)
        {
            Interactable interactable = hit.transform.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this);
                break;
            }
        }
    }

    /**
     * Checks if an item is currently in the players inventory.
     * 
     * GameObject (item): the item being searched for.
     * 
     * returns if item was found
     */
    public bool CheckInventoryForItem(GameObject item)
    {
        foreach (GameObject inventoryItem in inventory)
        {
            if (inventoryItem == item)
                return true;
        }
        return false;
    }

    /**
     * Adds item to the players inventory
     * 
     * GameObject (item): the game object item being added to the players inventory
     */
    public void AddItemToInventory(GameObject item)
    {
        inventory.Add(item);
    }

    /**
     * Stops the player from taking damage for a small period of time.
     */
    IEnumerator ActivateInvincibleFrames()
    {
        invincibleFrames = true;
        yield return new WaitForSecondsRealtime(1f);
        invincibleFrames = false;
    }
}
