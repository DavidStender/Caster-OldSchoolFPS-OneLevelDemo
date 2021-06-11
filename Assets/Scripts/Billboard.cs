using UnityEngine;

/**
 * Is used to turn a 2d sprite or texture into 2d billoard
 */
public class Billboard : MonoBehaviour {

    /**The target to face towards**/
    private GameObject target;

    /**
     * Finds the target when the scene loads
     */
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    /**
     * Keeps the game object facing the target
     */
    void Update () {
        transform.forward = target.transform.forward;
	}
}
