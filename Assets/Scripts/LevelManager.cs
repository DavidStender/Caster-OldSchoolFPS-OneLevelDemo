using UnityEngine;

/**
 * Creates a game object to manager the events and objectives that need to be controlled in a level
 * The order of the objectives in the objectives array matters and will need to match the placement in the end level screen.
 */
public class LevelManager : MonoBehaviour
{
    /**The spot on the map that ends the level**/
    [SerializeField]
    private GameObject levelEndpoint;

    /**The gameobjects that have objective components**/
    [SerializeField]
    private GameObject[] objectives;

    /**The time spent on the level from start to finish**/
    private float levelTime;
    /**The Game manager for the game**/
    private GameManager gameManager;

    /**
     * Finds the game manager component in the current scene
     */
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    /**
     * Runs the level timer and handles pause input
     */
    void Update()
    {
        levelTime += Time.deltaTime;
    }

    /**
     * Is called when the player finishes the level. Checks the list of objectives to see if they were all completed.
     * Sets the previous run stats of the game manager and calls the EndLevel method of the game manager.
     */
    public void TriggerLevelEnd()
    {
        bool[] objectivesList = new bool[objectives.Length];
        int counter = 0;
        bool allObjectivesCompleted = true;
        foreach(GameObject objective in objectives)
        {
            bool objectiveCompleted = objective.GetComponent<IObjective>().CheckObjectiveComplete();
            if(!objectiveCompleted)
            {
                allObjectivesCompleted = false;
            }
            objectivesList[counter++] = objectiveCompleted;
        }
        Cursor.lockState = CursorLockMode.None;
        gameManager.GetCurrentLevel().SetPreviousAttempt(allObjectivesCompleted, objectivesList, levelTime);
        gameManager.EndLevel(allObjectivesCompleted, levelTime);
    }
}
