using UnityEngine;

/**
 * Lets a player game object look around by rotating both the game object and main camera in a first person
 * view. Allows both mouse and windows xbox controller controls.
 */
public class PlayerCameraController : MonoBehaviour
{
    /**How fast the playe turns left and right with a mouse**/
    [SerializeField]
    private float mouseHorizontalLookSpeed = 1f;

    /**How fast the player looks up and down with a mouse**/
    [SerializeField]
    private float mouseVerticalLookSpeed = 1f;

    /**How fast the playe turns left and right with a controller**/
    [SerializeField]
    private float controllerHorizontalLookSpeed = 1f;

    /**How fast the player looks up and down with a controller**/
    [SerializeField]
    private float controllerVerticalLookSpeed = 1f;

    /**The main camera for the player**/
    [SerializeField]
    private GameObject playerCamera;

    /**The player component of the player game object**/
    private Player player;
    private PlayerMovement playerMovement;

    /**
     * Initializes the player component.
     */
    private void Awake()
    {
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    /**
     * Lets the player control the camera if they are alive
     */
    void LateUpdate()
    {
        if(player.IsAlive && Time.timeScale > 0)
            Look();
    }

    /**
     * Lets the player look around on both the x and y axis.
     */
    private void Look()
    {
        LookHorizontal();
        LookVertical();
    }

    /**
     * Lets the player use both the mouse and controller to look left and right.
     */
    private void LookHorizontal()
    {
        float horizontal = Input.GetAxisRaw("Mouse X") * mouseHorizontalLookSpeed;
        if (horizontal == 0)
        {
            horizontal = Input.GetAxisRaw("Right Horizontal") * controllerHorizontalLookSpeed * Time.deltaTime * 100;
        }
        transform.Rotate(0f, horizontal, 0f);
    }

    /**
     * Lets the player look up and down using both a mouse and controller. Will lock the player from looking  too far
     * up and too far down.
     */
    private void LookVertical()
    {
        float vertical = Input.GetAxisRaw("Mouse Y") * mouseVerticalLookSpeed;
        float verticalRotation = -vertical;
        if (vertical == 0)
        {
            vertical = -Input.GetAxisRaw("Right Vertical") * controllerVerticalLookSpeed * 100 * Time.deltaTime;
            verticalRotation = -vertical;
        }

        if(!(playerCamera.transform.rotation.x <= -0.40f && verticalRotation <= 0) && !(playerCamera.transform.rotation.x >= 0.40f && verticalRotation >= 0))
            playerCamera.transform.Rotate(verticalRotation, 0.0f, 0.0f);
    }

    public void SetMouseHorizontalLookSpeed(float horizontalLookSpeed)
    {
        mouseHorizontalLookSpeed = horizontalLookSpeed;
    }

    public void SetMouseVerticalLookSpeed(float verticalLookSpeed)
    {
        mouseVerticalLookSpeed = verticalLookSpeed;
    }

    public void SetControllerHorizontalLookSpeed(float horizontalLookSpeed)
    {
        controllerHorizontalLookSpeed = horizontalLookSpeed;
    }

    public void SetControllerVerticalLookSpeed(float verticalLookSpeed)
    {
        controllerVerticalLookSpeed = verticalLookSpeed;
    }
}
