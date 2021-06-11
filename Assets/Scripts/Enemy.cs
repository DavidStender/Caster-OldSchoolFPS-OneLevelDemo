using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/**
 * Creates an enemy game object that can take damage, attack a player, and uses a nav mesh and nav mesh agent to
 * move the game object around the level.
 */
public class Enemy : MonoBehaviour
{
    /**The name of the enemy**/
    [SerializeField]
    private string enemyName;

    /**The max health of the enemy**/
    [SerializeField]
    private int maxHealth = 1;

    /**The damage done to the player**/
    [SerializeField]
    private int attackDamage = 1;

    /**The attack range of the enemy**/
    [SerializeField]
    private float attackRadius = 3f;

    /**The sound played when the enemy is hit**/
    [SerializeField]
    private AudioClip hitSound;

    /**The sound played when attacking**/
    [SerializeField]
    private AudioClip attackSound;

    /**The dying sound of the enemy**/
    [SerializeField]
    private AudioClip dyingSound;

    /**The sound played when the enemy moves**/
    [SerializeField]
    private AudioClip moveSound;

    /**The alerts the enemy if the player enters the trigger zone**/
    [SerializeField]
    private BoxCollider alertCollider;

    /**The movement speed of the enemy**/
    private float movementSpeed;
    /**The current health of the enemy**/
    private int currentHealth;
    /**If the enemy is alive or not**/
    private bool isAlive = true;
    /**If the enemy is attacking or not**/
    private bool isAttacking = false;
    /**The time until the next idle animation plays**/
    private float timeToNextIdleAnimation;
    /**If the player is in the enemy's alert zone**/
    private bool playerInAlertZone;
    /**The player of the game**/
    private Player player;
    /**The current target to navigate to**/
    private GameObject currentTarget;
    /**The default color of the sprite renderer**/
    private Color normalColor;
    /**The sprite renderer component of the game object**/
    private SpriteRenderer spriteRenderer;
    /**The audio source component of the game object**/
    private AudioSource audioSource;
    /**The nav mesh agent of the game object**/
    private NavMeshAgent navMeshAgent;
    /**The animator component of the game object**/
    private Animator animator;
    
    /**A delegate that sends a message out about a change in state of the enemy**/
    public delegate void EnemyMessage(GameObject enemy);
    /**A event message that is sent when a enemy is killed**/
    public static event EnemyMessage EnemyKilled;

    /**
     * Setups of the enemy game objects components. This includes setting the current health,
     * the sprite renderer, the audio source, the normal color, the nav mesh agent, the animator
     * and the movement speed.
     */
    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        normalColor = spriteRenderer.color;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        movementSpeed = navMeshAgent.speed;
        timeToNextIdleAnimation = Random.Range(10f, 30f);
    }

    /**
     * If the enemy has a current target will update its location every frame.
     * If a player is with in range(Inside the alert collider) I will check to see if they are visible
     * and make them the current target
     */
    private void Update()
    {
        if(player != null)
        {
            if(PlayerIsVisible(player))
            {
                UpdateCurrentTarget(player.gameObject);
                player = null;
            }
        }

        if(currentTarget != null && isAlive)
        {
            navMeshAgent.SetDestination(currentTarget.transform.position);
        }

        if(Time.time >= timeToNextIdleAnimation)
        {
            animator.SetTrigger("Idle");
            timeToNextIdleAnimation = Time.time + Random.Range(5f, 25f);
        }
    }

    /**
     * Returns the current target of the enemy.
     */
    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }
    
    /**
     * Returns if the enemy is alive or not.
     */
    public bool GetIsAlive()
    {
        return isAlive;
    }

    /**
     * Updates the current target to the passed in game object.
     * 
     * GameObject (target): The target being set as the current target.
     */
    private void UpdateCurrentTarget(GameObject target)
    {
        animator.SetBool("Walking", true);
        currentTarget = target;
    }

    /**
     * The enemy will take damage by the amount passed in. Will only take damage when alive.
     * If damage is great or equal to the amount of current health then the enemy will play the
     * death animation and be deactivated.
     * 
     * float (damage): The amount of damage to do to the enemy.
     */
    private void TakeDamage(float damage)
    {
        if(isAlive)
        {
            StartCoroutine("DamageFlash");
            audioSource.PlayOneShot(hitSound);
            if(damage >= currentHealth)
            {
                isAlive = false;
                animator.SetTrigger("Die");
                audioSource.PlayOneShot(dyingSound, 1.5f);
                navMeshAgent.speed = 0;
                navMeshAgent.velocity = Vector3.zero;
                currentTarget = null;
            }
            else
            {
                currentHealth -= (int)damage;
            }
        }
    }

    /**
     * The calls the private TakeDamage method and sets currentTarget if it is null.
     * 
     * float (damage): the amount of damage to do to the enemy.
     * GameObject (attacker): the game object attacking the enemy.
     */
    public void TakeDamage(float damage, GameObject attacker)
    {
        TakeDamage(damage);
        if(currentTarget == null)
            UpdateCurrentTarget(attacker);
    }

    /**
     * Checks if the EnemyKilled event exsits; calls it and deactivates the game object.
     */
    private void Die()
    {
        if (EnemyKilled != null)
            EnemyKilled(this.gameObject);
        gameObject.SetActive(false);
    }

    /**
     * Attacks a game object that has entered the monsters attack trigger zone. Is called by the animator
     * during the attack animation.
     */
    private void TriggerAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRadius, new Vector3(0.0f, 0.25f, 0.0f), 0.25f);
        bool playerFound = false;
        foreach(RaycastHit hit in hits)
        {
            if(hit.transform.tag == "Player")
            {
                player = hit.transform.GetComponent<Player>();
                if(PlayerIsVisible(player))
                    player.TakeDamage(attackDamage, gameObject);
                playerFound = true;
                break;
            }
        }

        if (!playerFound && isAlive)
        {
            animator.SetBool("Walking", true);
            ResetAttackingState();
        }
    }

    /**
     * Handles a game object entering and exiting either the alert trigger zone, or the attack trigger zone, then
     * calls the corresponding method or sets an animator trigger.
     */
    public void TriggerEventHandler(string message, GameObject triggerer)
    {
        if(isAlive)
        {
            if (message == "Alert" && currentTarget == null)
            {
                player = triggerer.GetComponent<Player>();
            }
            else if (message == "Attack" && PlayerIsVisible(triggerer.GetComponent<Player>()))
            {
                animator.SetTrigger("Attack");
                animator.SetBool("Walking", false);
                navMeshAgent.speed = 0;
                navMeshAgent.velocity = Vector3.zero;
                isAttacking = true;
            }
            else if(message == "Alert Exit")
            {
                player = null;
                ResetAttackingState();
            }
        }
    }

    /**
     * Checks to see if the enemy can see the player from where they are or if the player is obstructed by 
     * level geometry
     */
    public bool PlayerIsVisible(Player player)
    {
        bool playerVisible = false;
        RaycastHit visibilityCheck;
        Vector3 hitDirection = new Vector3(player.gameObject.transform.position.x - transform.position.x, player.gameObject.transform.position.y - transform.position.y, player.gameObject.transform.position.z - transform.position.z);
        if (Physics.Raycast(transform.position, hitDirection, out visibilityCheck, 100, 1))
        {
            if (visibilityCheck.transform.tag == "Player")
            {
                playerVisible = true;
            }
        }
        return playerVisible;
    }

    /**
     * If the current target is no longer in the attack zone then sets the back to what it was.
     */
    public void ResetAttackingState()
    {
        navMeshAgent.speed = movementSpeed;
        isAttacking = false;
    }

    /**
     * Flashes the sprite red and then back again to show the enemy taking damage.
     */
    IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSecondsRealtime(.05f);
        spriteRenderer.color = normalColor;
    }

    /**
     * Plays the attack sound of the enemy. (Used in the animator to sync the sound and animation)
     */
    private void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound, 1.5f);
    }

    /**
     * Plays the move sound of the enemy. (Used in the animator to sync the sound and animation)
     */
    private void PlayMoveSound()
    {
        audioSource.PlayOneShot(moveSound, 1.2f);
    }
}
