using System.Collections;
using UnityEngine;

/**
 * Bullet for a gun that travels instantly to its target
 */
public class BulletHitscan : MonoBehaviour
{
    /**The amount of damage the bullet does**/
    [SerializeField]
    private float damage = 1f;
    /**A trail of the bullets path**/
    private LineRenderer bulletLine;

    /**
     * Gets the line renderer component on the bullet
     */
    public void Awake()
    {
        bulletLine = GetComponent<LineRenderer>();
    }

    /**
     * Sends a raycast from the barrel of the gun to the center of the camera.
     * Bullet goes 100 units and only hits things on the default, EnemyHitBox, and
     * floor layers.
     */
    public void Shoot(float recoil, Camera camera, GameObject shooter)
    {
        float randomRecoilX = Random.Range(-recoil, recoil);
        float randomRecoilY = Random.Range(-recoil, recoil);
        Vector3 bloom = new Vector3(randomRecoilX, randomRecoilY/2, 0);
        if (Physics.Raycast(transform.position, camera.transform.forward + bloom, out RaycastHit hit, 100f, 769))
        {
            bulletLine.SetPosition(0, transform.position);
            bulletLine.SetPosition(1, hit.point);
            StartCoroutine("DrawBulletLine");

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, shooter);
            }
                
        }
    }

    /**
     * Activates and deactivates the line renderer component after a short period of time
     */
    IEnumerator DrawBulletLine()
    {
        bulletLine.enabled = true;
        yield return new WaitForSeconds(0.015f);
        bulletLine.enabled = false;
    }
}
