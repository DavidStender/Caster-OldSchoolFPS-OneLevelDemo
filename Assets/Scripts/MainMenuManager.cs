using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Creates a game object that can be used to manage the main menu scene of a game.
 * Buttons are highlighted to allow game controllers to navigate the menu. The end screen objectives must be in the same order
 * that they are in the level manager for the level.
 */
public class MainMenuManager : MonoBehaviour
{
    /**The canvas game object that has the splash menu components**/
    [SerializeField]
    private GameObject splashMenu;

    /**The canvas game object that has the level selection screen components**/
    [SerializeField]
    private GameObject levelSelectionScreen;

    /**The canvas game objects for each level selection screen**/
    [SerializeField]
    private GameObject[] levelInfoScreens;

    /**The canvas game object hat has the level end screen**/
    [SerializeField]
    private GameObject endScreen;

    /**The list of level objectives for the most recent level**/
    [SerializeField]
    private GameObject[] endScreenObjectives;

    /**The level time text component on the end screen**/
    [SerializeField]
    private Text levelTimeText;

    /**The fastest time text component on the end screen**/
    [SerializeField]
    private Text FastestTimeText;

    /**The passFail Text component on the end screen**/
    [SerializeField]
    private Text passFailText;

    /**A button on the end screen that will either replay the most recent levl or take you back to the level select screen based
     * on if the level was complete or not**/
    [SerializeField]
    private Button retryLevelSelectButton;

    [SerializeField]
    private AudioClip screenTransitionSound;

    /**The game manager component of the game**/
    private GameManager gameManager;
    /**The current screen being displayed**/
    private GameObject currentScreen;
    /**The most level played**/
    private Level previousLevel;
    /**The audio source component of the menu manager**/
    private AudioSource audioSource;

    /**
     * Initializes the main menu. Finds the game manager, sets the splash screen to be displayed or checks
     * if an end level screen needs to be displyed. Sets the first button it finds to be selected.
     */
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        UpdateFastestTimes();
        splashMenu.SetActive(true);
        levelSelectionScreen.SetActive(false);
        endScreen.SetActive(false);
        currentScreen = splashMenu;
        if (gameManager.GetGameState() == GameState.EndScreen)
        {
            SetupEndScreen();
        }
        currentScreen.GetComponentInChildren<Button>().Select();
        audioSource = GetComponent<AudioSource>();
    }

    /**
     * Changes the screen being displayed to the screen passed in and highlights the first button found.
     * 
     * GameObject (screeen): The screen to be displayed.
     */
    public void ChangeScreen(GameObject screen)
    {
        currentScreen.SetActive(false);
        currentScreen = screen;
        currentScreen.SetActive(true);
        currentScreen.GetComponentInChildren<Button>().Select();
        audioSource.PlayOneShot(screenTransitionSound, 4f);
    }

    /**
     * Sets the game managers state to Level, the currentLevel to the level being loaded, and loads the level.
     * 
     * string (sceneName): The name of the unity scene to load.
     */
    public void PlayLevel(string sceneName)
    {
        gameManager.SetGameState(GameState.Level);
        gameManager.SetCurrentLevel(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    /**
     * Updates the text components of the level info screens to be the fastest times
     * The time text canvas game object must be the first child game object for a level selection screen 
     */
    private void UpdateFastestTimes()
    {
        Level[] level = gameManager.GetLevelList();
        for(int i=0; i<levelInfoScreens.Length; i++)
        {
            levelInfoScreens[i].GetComponentInChildren<Text>().text = $"{(Mathf.Floor(level[i].GetFastestTime()/60)).ToString("00")}:{(level[i].GetFastestTime() % 60).ToString("00")}";
        }
    }

    /**
     * Updates all the components of the end screen to be the results of the most recent level played.
     */
    private void SetupEndScreen()
    {
        Debug.Log("Setting up end screen");
        currentScreen.SetActive(false);
        currentScreen = endScreen;
        currentScreen.SetActive(true);

        previousLevel = gameManager.GetCurrentLevel();

        bool[] previousRunObjectives = gameManager.GetCurrentLevel().GetPreviousRunObjectiveList();
        for(int i=0; i<previousRunObjectives.Length; i++)
        {
            if (!previousRunObjectives[i])
                endScreenObjectives[i].GetComponent<Text>().color = Color.red;
            endScreenObjectives[i].SetActive(true);
        }

        if(gameManager.GetCurrentLevel().GetPreviousRunLevelCompleted())
        {
            passFailText.text = "Passed";
            passFailText.color = Color.green;

            retryLevelSelectButton.onClick.AddListener(() => { ChangeScreen(levelSelectionScreen); });
            retryLevelSelectButton.GetComponentInChildren<Text>().text = "Level Select";
        }
        else
        {
            passFailText.text = "Fail";
            passFailText.color = Color.red;

            retryLevelSelectButton.onClick.AddListener(() => { PlayLevel(previousLevel.gameObject.name); });
            retryLevelSelectButton.GetComponentInChildren<Text>().text = "Retry";
        }

        levelTimeText.text = $"{(Mathf.Floor(gameManager.GetCurrentLevel().GetPreviousRunLevelTime() / 60)).ToString("00")}:{(gameManager.GetCurrentLevel().GetPreviousRunLevelTime() % 60).ToString("00")}";
        FastestTimeText.text = $"{(Mathf.Floor(gameManager.GetCurrentLevel().GetFastestTime() / 60)).ToString("00")}:{(gameManager.GetCurrentLevel().GetFastestTime() % 60).ToString("00")}";

        

        gameManager.GetCurrentLevel().ResetPreviousRun();
        gameManager.SetCurrentLevel("Menu");
    }
}
