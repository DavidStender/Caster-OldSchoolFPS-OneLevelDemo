using UnityEngine;

/**
 * Used to create a level objective game object that requires the player to kill all enemies listed.
 * Must be a child of the level manager game object.
 */
public class KillAllObjective : MonoBehaviour, IObjective
{
    /**The targets that must be eliminated**/
    [SerializeField]
    private GameObject[] targets;

    /**The parent game object level manager component**/
    private LevelManager levelManager;
    /**If the objective is complete or not**/
    private bool isObjectiveComplete;

    /**
     * Finds the level manager in the parent game object and sets up the listener for an
     * EnemyKilled event to check if the enemy that was killed was an objective target.
     */
    private void OnEnable()
    {
        levelManager = GetComponentInParent<LevelManager>();
        Enemy.EnemyKilled += CheckIfTarget;
    }

    /**
     * Removes the listen for the EnemyKilled event when this game object is deactivated.
     */
    private void OnDisable()
    {
        Enemy.EnemyKilled -= CheckIfTarget;
    }

    /**
     * When an EnemyKilled event happens the enemy is checked against the list of targets.
     * If the enemy is a target UpdateObjective is called.
     * 
     * GameObject (enemy): 
     */
    private void CheckIfTarget(GameObject enemy)
    {
        foreach(GameObject target in targets)
        {
            if(enemy == target)
            {
                UpdateObjective();
            }
        }
    }

    /**
     * Check if all targets have been eliminated and sets isObjectComplete to the result
     */
    private void UpdateObjective()
    {
        bool allTargetsKilled = true;
        foreach(GameObject target in targets)
        {
            if (target.GetComponent<Enemy>().GetIsAlive())
                allTargetsKilled = false;
        }
        isObjectiveComplete = allTargetsKilled;
    }

    /**
     * Returns if the objective is complete or not.
     */
    public bool CheckObjectiveComplete()
    {
        return isObjectiveComplete;
    }
}
