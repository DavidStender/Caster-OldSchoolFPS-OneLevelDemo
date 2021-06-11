using UnityEngine;

/**
 * A component used to create a level game object.
 * Must be a child of the Game Manager game object.
 * Game object name must be the same name as the untiy scene.
 */
public class Level : MonoBehaviour
{
    /**The name of the level**/
    [SerializeField]
    private string levelName;

    /**The description of the level**/
    [SerializeField]
    private string levelDescription;

    /**The descripton for each objective in the level**/
    [SerializeField]
    private string[] levelObjectiveDescriptions;

    /**The name of the unity scene for the level**/
    [SerializeField]
    private string level;

    /**Has the level been completed yet**/
    [SerializeField]
    private bool levelCompleted = false;

    /**The fastest time the level has been completed in**/
    [SerializeField]
    private float fastestTime = 180f;

    /**If the most recent run was completed**/
    private bool previousRunLevelCompleted;
    /**What objectives were complete or not in the most recent rune**/
    private bool[] previousRunObjectiveList;
    /**The most recent runs time**/
    private float previousRunLevelTime;

    /**
     * Returns the fastest completion time for the level
     */
    public float GetFastestTime()
    {
        return fastestTime;
    }

    /**
     * Sets the fastest completion time of the level to newTime.
     * 
     * float (newTime): the new fastest completion time.
     */
    public void UpdatefastestTime(float newTime)
    {
        fastestTime = newTime;
    }

    /**
     * Returns level (the name of the unity scene)
     */
    public string GetLevel()
    {
        return level;
    }

    /**
     * Sets levelCompleted to be true
     */
    public void SetLevelCompleted()
    {
        levelCompleted = true;
    }

    /**
     * Returns pif the most recent level run was completed.
     */
    public bool GetPreviousRunLevelCompleted()
    {
        return previousRunLevelCompleted;
    }

    /**
     * Returns a list of the completed and failed objectives from the most recent run.
     */
    public bool[] GetPreviousRunObjectiveList()
    {
        return previousRunObjectiveList;
    }

    /**
     * Returns the most recent run level time.
     */
    public float GetPreviousRunLevelTime()
    {
        return previousRunLevelTime;
    }

    /**
     * Sets the results of the most recent level run.
     * 
     * bool (levelCompleted): If the most recent level run was completed.
     * bool[] (objectiveList): The list of completed and uncompleted objectives from the most recent run.
     * float (levelTime): The most recent runs time.
     */
    public void SetPreviousAttempt(bool levelCompleted, bool[] objectiveList, float levelTime)
    {
        previousRunLevelCompleted = levelCompleted;
        previousRunObjectiveList = objectiveList;
        previousRunLevelTime = levelTime;
    }

    /**
     * Resets the previous run results
     */
    public void ResetPreviousRun()
    {
        previousRunLevelCompleted = false;
        previousRunObjectiveList = null;
        previousRunLevelTime = -1;
    }
}
