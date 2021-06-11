using UnityEngine;
using UnityEngine.UI;

/**
 * Attached to a player game object and controls updating the player's health and ammo UI components
 */
public class PlayerUIController : MonoBehaviour
{
    /**The text component for the health UI**/
    [SerializeField]
    private Text healthText;

    /**The text component for the ammo UI**/
    [SerializeField]
    private Text ammoText;

    /**The pause screen of the level**/
    [SerializeField]
    private GameObject pauseScreen;

    /**The text for the value of the mouse horizontal look speed slider**/
    [SerializeField]
    private Text mouseHorizontalLookSpeedText;

    /**The text for the value of the mouse vertical look speed slider**/
    [SerializeField]
    private Text mouseVerticalLookSpeedText;

    /**The text for the value of the controller horizontal look speed slider**/
    [SerializeField]
    private Text controllerHorizontalLookSpeedText;

    /**The text for the value of the controller vertical look speed slider**/
    [SerializeField]
    private Text controllerVerticalLookSpeedText;

    /**If the game is currently paused at the moment**/
    private bool isPaused = false;

    /**
     * Handles pauseing the game when the player presses the pause button.
     */
    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            pauseScreen.SetActive(isPaused);
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0.0f;
                pauseScreen.GetComponentInChildren<Slider>().Select();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
        }
    }

    /**
     * Updates the text that shows the value of the horizontal mouse slider.
     * 
     * float (value): The current value of the slider
     */
    public void SetMouseHorizontalLookSpeedText(float value)
    {
        mouseHorizontalLookSpeedText.text = $"{value.ToString("0.00")}";
    }

    /**
     * Updates the text that shows the value of the vertical mouse slider.
     * 
     * float (value): The current value of the slider
     */
    public void SetMouseVerticalLookSpeedText(float value)
    {
        mouseVerticalLookSpeedText.text = $"{value.ToString("0.00")}";
    }

    /**
     * Updates the text that shows the value of the horizontal controller slider.
     * 
     * float (value): The current value of the slider
     */
    public void SetControllerHorizontalLookSpeedText(float value)
    {
        controllerHorizontalLookSpeedText.text = $"{value.ToString("0.00")}";
    }

    /**
     * Updates the text that shows the value of the vertical controller slider.
     * 
     * float (value): The current value of the slider
     */
    public void SetControllerVerticalLookSpeedText(float value)
    {
        controllerVerticalLookSpeedText.text = $"{value.ToString("0.00")}";
    }

    /**
     * Updates the health UI to show the players current health
     */
    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    /**
     * Updates the ammo UI to show the current amount of bullets in the magazine
     */
    public void UpdateAmmoText(int ammoInMagazine, int totalMagazineSize)
    {
        ammoText.text = $"{ammoInMagazine} / {totalMagazineSize}";
    }
}
