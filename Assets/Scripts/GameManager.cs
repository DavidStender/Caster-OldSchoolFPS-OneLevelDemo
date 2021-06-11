using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * The states the game can be in.
 */
public enum GameState
{
    Menu,
    Level,
    EndScreen
}

/**
 * A component class used to manage the game. Will change scenes between the main menu and a level.
 * Will also handle passing information between the last level played and the main menu manager.
 */
public class GameManager : MonoBehaviour
{
    /**The current state the game is in.**/
    [SerializeField]
    private GameState gameState;

    /**The current level being played (will be null if on the main menu)**/
    [SerializeField]
    private Level currentLevel;

    /**The list of levels in the game**/
    [SerializeField]
    private Level[] levelList;

    /**A singleton pattern instance used to stop there from being more then one game manager in a scene**/
    [SerializeField]
    private static GameManager instance;

    private MainMenuManager mainMenuManager;

    /**
     * sets up an instance of a game manager to stop more then one game manager from being present in a scene.
     * Sets gameState to Menu and finds the main menu manager in the scene.
     * 
     * This object will not be destoryed on load.
     */
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        gameState = GameState.Menu;
        if (gameState == GameState.Menu)
        {
            mainMenuManager = GameObject.FindGameObjectWithTag("Main Menu Manager").GetComponent<MainMenuManager>();
        }
        DontDestroyOnLoad(this);
    }

    /**
     * Returns the current gameState
     */
    public GameState GetGameState()
    {
        return gameState;
    }

    /**
     * Sets gameState to the passed in state
     * 
     * GameState (newGameState): The new game state
     */
    public void SetGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    /**
     * Returns the current level being played. Will be null if on the main menu.
     */
    public Level GetCurrentLevel()
    {
        return currentLevel;
    }

    /**
     * Sets currentLevel to the corresponding level of passed in level name.
     * The level must be a child of the game manager.
     * 
     * string (levelName): The name of the to look for from the level list.
     */
    public void SetCurrentLevel(string levelName)
    {
        if(levelName == "Menu")
        {
            currentLevel = null;
            return;
        }
        foreach(Level level in levelList)
        {
            if(level.name == levelName)
            {
                currentLevel = level;
                return;
            }
        }
    }

    /**
     * Returns the list of levels.
     */
    public Level[] GetLevelList()
    {
        return levelList;
    }

    /**
     * Ends the current level being played, changes back to the main menu, and sets gameState to be EndScreen.
     * If the level objectives were completed then the level will be set to completed.
     * If the level was completed then the completion time is checked against the fastest time and updates
     * the time if it was beat.
     * 
     * bool (completedObjectives): If the level objectives were completed.
     * float (levelTime): The time spent on the level.
     */
    public void EndLevel(bool completedObjectives, float levelTime)
    {
        gameState = GameState.EndScreen;
        SceneManager.LoadScene("Main Menu");
        if(completedObjectives)
        {
            currentLevel.SetLevelCompleted();
        }
        if (completedObjectives && levelTime < currentLevel.GetFastestTime())
        {
            currentLevel.UpdatefastestTime(levelTime);
        }
    }
}
