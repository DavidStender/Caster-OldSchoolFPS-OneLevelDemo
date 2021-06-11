using UnityEngine;

/**
 * Used to create an objective on a level to collect one or many treasures.
 */
public class CollectTreasureObjective : MonoBehaviour, IObjective
{
    /**The level manager for the current level**/
    [SerializeField]
    private LevelManager levelManager;

    /**The treasures to be collected**/
    [SerializeField]
    private GameObject[] treasures;

    /**Is the objective complete or not**/
    private bool isObjectiveComplete = false;

    /**
     * Gets all treasure delegate events on the level and links them to UpdateObjective.
     */
    private void OnEnable()
    {
        Treasure.TreasurePickedUp += UpdateObjective;
    }

    /**
     * Removes all the instances of treasure from UpdateObjective.
     */
    private void OnDisable()
    {
        Treasure.TreasurePickedUp -= UpdateObjective;
    }

    /**
     * Each time a treasure is collected the list of treasures is checked to see if all of them
     * have been collected. When they are all collected sets isObjectiveComplete to true.
     */
    private void UpdateObjective(Treasure treasure)
    {
        bool allTreasuresCollected = true;
        foreach (GameObject t in treasures)
        {
            if (t.activeInHierarchy)
            {
                allTreasuresCollected = false;
            }
        }
        isObjectiveComplete = allTreasuresCollected;
    }

    /**
     * Used to check if the objective was completed.
     */
    public bool CheckObjectiveComplete()
    {
        return isObjectiveComplete;
    }
}
