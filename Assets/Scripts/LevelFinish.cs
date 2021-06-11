using UnityEngine;

/**
 * Used to create a trigger zone for the player to walk into to finish the level.
 */
public class LevelFinish : MonoBehaviour
{
    /**The level manager for the level**/
    [SerializeField]
    private LevelManager LevelManager;

    /**
     * Triggers the end of the level when the player walks into the trigger zone.
     */
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            LevelManager.TriggerLevelEnd();
        }
    }
}
