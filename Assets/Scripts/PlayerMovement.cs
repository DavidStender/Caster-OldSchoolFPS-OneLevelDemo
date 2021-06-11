using UnityEngine;

/**
 * A component used to control the player game object using the unity character controller.
 * Allows both keyboard and windows xbox controller controls.
 */
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    /**The speed the character will move at**/
    [SerializeField]
    private float movementSpeed = 1f;

    /**Used to hold the values of the Horizontal and Vertical inputs from the unity input manager**/
    private float horizontal, vertical;

    /**The character controller component on the player**/
    private CharacterController characterController;
    /**The animator component on the player**/
    private Animator animator;
    /**The player component of the player**/
    private Player player;
    /**The rigid body component of the player**/
    private Rigidbody rigidBody;

    /**
     * Initializes several of the components on the player
     */
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody>();
    }

    /**
     * Controls moving the player around while they are alive.
     */
    void Update()
    {
        if(player.IsAlive)
        {
            Movement();
        }
    }

    /**
     * Uses the unity input manager to get both keyboard and controller input to move the character left, right, forward, and back.
     */
    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 playerVelocity = (transform.TransformDirection(Vector3.forward) * vertical) + (transform.TransformDirection(Vector3.right * horizontal));
        characterController.SimpleMove(playerVelocity * movementSpeed);
        
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        //if the player is not ground then a negative y velocity is applied to make going down inclines smooth
        if (!characterController.isGrounded && characterController.velocity != Vector3.zero)
        {
            characterController.Move(new Vector3(0f, -1f, 0f));
        }
    }

    /**
     * Returns the current movement velocity of the player
     */
    public Vector3 GetCurrentVelocity()
    {
        return characterController.velocity;
    }
}
