using UnityEngine;

/**
 * A component used to create a gun game object for a player to use that is a child of 
 * the main camera which is the child of the player.
 */
public class Gun : MonoBehaviour
{
    /****/
    [SerializeField]
    private bool automatic;

    /**The rate of fire for the gun**/
    [SerializeField]
    private float rateOfFire = 5f;

    /**The amount of recoil applied to the gun after firing**/
    [SerializeField]
    private float recoilAmount = 1f;

    /**The amount bullets currently in the magazine**/
    [SerializeField]
    private int ammoInMagazine = 13;

    /**Does the gun have infinite ammo or not**/
    [SerializeField]
    private bool hasInfiniteAmmo = false;

    /**The muzzle of the gun game object**/
    [SerializeField]
    private GameObject muzzle;

    /**The bullet the gun will fire**/
    [SerializeField]
    private GameObject bullet;

    /**How fast the recoil will reset back to zero**/
    [SerializeField]
    private float recoilDecoy = 20f;

    /**The sound the gun plays when fired**/
    [SerializeField]
    private AudioClip fireSound;

    /**The sound the gun plays when its reloaded**/
    [SerializeField]
    private AudioClip reloadSound;

    /**The sound the gun plays when trying to fire with no ammo**/
    [SerializeField]
    private AudioClip emptyFireSound;

    /**The in game time when the gun can fire next**/
    private float timeToNextFire;
    /**How much recoil the gun currently has**/
    private float currentRecoilAmount;
    /**The amount of bullets the magazine can hold**/
    private int magazineSize;
    /**The total amount of a gun can have**/
    private int totalAmmo = 100; // Currently unused
    /**The current amount of ammo the player is carrying for the gun**/
    private int currentAmmo = 100; // Currently unused
    /**If the gun is currently reloading or not**/
    private bool isReloading = false;
    /**If the gun can fire (Used in a none automatic gun)**/
    private bool canFire = true;

    /**The animator component of the gun**/
    private Animator animator;
    /**The main camera component of the game**/
    private Camera mainCamera;
    /**The audio source component of the gun**/
    private AudioSource audioSource;
    /**The player component attached to the player**/
    private Player player;
    /**The player ui component**/
    private PlayerUIController playerUIController;

    /**
     * Initializes variables. Sets timeToNextFire and currentRecoilAmount equal to zero.
     * Sets the magazine size to be equal to ammoInMagazie. Get the components on the game object and
     * parent game object. This inclueds the animator, mainCamera, audioSource, player, playerUIController.
     */
    private void Awake()
    {
        timeToNextFire = 0f;
        currentRecoilAmount = 0f;
        magazineSize = ammoInMagazine;

        animator = GetComponent<Animator>();
        mainCamera = GetComponentInParent<Camera>();
        audioSource = GetComponent<AudioSource>();
        player = GetComponentInParent<Player>();
        playerUIController = GetComponentInParent<PlayerUIController>();
    }

    /**
     * Initializes the the player ui to get the magazine size and current ammo in the magazine.
     */
    private void Start()
    {
        playerUIController.UpdateAmmoText(ammoInMagazine, magazineSize);
    }

    /**
     * Used to managae recoil for the gun.
     * If the gun has recoil it will be reduced down to zero over time independent of frame rate
     */
    private void Update()
    {
        if (currentRecoilAmount > 0)
        {
            currentRecoilAmount -= Time.deltaTime/ recoilDecoy;
        }
        else if (currentRecoilAmount < 0)
            currentRecoilAmount = 0;
    }

    /**
     * Returns current value of canFire.
     */
    public bool GetCanFire()
    {
        return canFire;
    }

    /**
     * Sets canFire to be true.
     */
    public void ResetCanFire()
    {
        canFire = true;
    }

    /**
     * Controls if the gun can fire a bullet or not
     */
    public void Shoot()
    {
        if(ammoInMagazine > 0 && !isReloading && Time.time > timeToNextFire && (canFire || automatic))
        {
            animator.SetTrigger("Fired");
            audioSource.PlayOneShot(fireSound);
            audioSource.pitch = .4f;
            GameObject firedBullet = Instantiate(bullet, muzzle.transform.position, transform.rotation);
            firedBullet.transform.parent = transform;
            firedBullet.GetComponent<BulletHitscan>().Shoot(currentRecoilAmount, mainCamera, player.gameObject);
            Destroy(firedBullet, 5f);

            timeToNextFire = (1f / rateOfFire) + Time.time;
            currentRecoilAmount += recoilAmount;
            ammoInMagazine -= 1;
            playerUIController.UpdateAmmoText(ammoInMagazine, magazineSize);
        }
        else if(ammoInMagazine == 0 && (canFire || automatic))
        {
            audioSource.PlayOneShot(emptyFireSound);
            audioSource.pitch = 1f;
        }
        if (!automatic)
            canFire = false;
    }

    /**
     * Reloads the gun magazine back to full capacity. Does not check if a reload is need.
     * CheckValidReload must be called to check if the reload is need before calling this reload
     * method.
     */
    public void Reload()
    {
        audioSource.PlayOneShot(reloadSound);
        audioSource.pitch = 1f;
        isReloading = true;
        int magazine = 0;
        int ammoReloaded = magazineSize - ammoInMagazine;

        if (currentAmmo >= magazineSize - ammoInMagazine || hasInfiniteAmmo)
            magazine = magazineSize;
        else
            magazine = currentAmmo + ammoInMagazine;

        if (!hasInfiniteAmmo)
            if (currentAmmo >= ammoReloaded)
                currentAmmo -= ammoReloaded;
            else
                currentAmmo = 0;

        ammoInMagazine = magazine;
        playerUIController.UpdateAmmoText(ammoInMagazine, magazineSize);
    }

    /**
     * Checks if the gun can reload. Checks if the magazine is not full and that the gun is not 
     * currently reloading.
     */
    public bool CheckValidReload()
    {
        if (currentAmmo > 0 && ammoInMagazine < magazineSize && !isReloading)
            return true;
        else
            return false;
    }

    /**
     * Sets isReloading to false.
     */
    public void ResetIsReloading()
    {
        isReloading = false;
    }
}
