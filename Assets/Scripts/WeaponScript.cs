using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    //--------------------------------
    // 1 - Designer variables
    //--------------------------------

    /// <summary>
    /// Projectile prefab for shooting
    /// </summary>
    public Transform shotPrefab;

    /// <summary>
    /// Cooldown in seconds between two shots
    /// </summary>
    public float shootingRate = 0.25f;
    public bool shotless = false;
    public bool randomizeShotStart = false;
    public Vector3 shotOriginOffset = new Vector3(0, 0, 0);
    public Vector2 shotDirection = new Vector2(0, -1);
    public bool aimAtPlayer = false;
    public int burstShots = 1;
    public float bulletSpread = 0f; // max range to change vectors when shooting
    public float initialShotDelay = 0f;

    private HealthScript myHealthscript;
    private Vector2 tempShotDirection = new Vector2(0, -1);
    private Transform playerTransform = null;
    
    //--------------------------------
    // 2 - Cooldown
    //--------------------------------

    private float shootCooldown;
    private float maxRandomizationCooldownIncrease = 0.7f;

    void Start()
    {
        shootCooldown = 0f;
        myHealthscript = GetComponent<HealthScript>();
        if (randomizeShotStart)
        {
            shootCooldown = Random.Range(0f, (shootingRate * maxRandomizationCooldownIncrease));
        }
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        shootCooldown += initialShotDelay;
    }

    void Update()
    {
        if (shootCooldown > 0 && myHealthscript.active)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    public Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    //--------------------------------
    // 3 - Shooting from another script
    //--------------------------------

    /// <summary>
    /// Create a new projectile if possible
    /// </summary>
    public void Attack(bool isEnemy)
    {
        if (!shotless & CanAttack)
        {
            shootCooldown = shootingRate;
            tempShotDirection = shotDirection;

            for (var i=0; i<burstShots; i++)
            {
                // Create a new shot
                var shotTransform = Instantiate(shotPrefab) as Transform;

                // Assign position
                shotTransform.position = transform.position + shotOriginOffset;

                ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
                if (shot != null)
                {
                    shot.isEnemyShot = isEnemy;
                }

                // Make the weapon shot always towards it
                MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
                if (move != null)
                {
                    if (aimAtPlayer && playerTransform)
                    {
                        var directionVector = new Vector2(0, 0);
                        directionVector = playerTransform.position - shotTransform.position;
                        directionVector.Normalize();
                        move.direction.x = directionVector.x + (Random.Range(-bulletSpread, bulletSpread));
                        move.direction.y = directionVector.y + (Random.Range(-bulletSpread, bulletSpread));
                    }
                    else
                    {
                        move.direction.x = tempShotDirection.x + (Random.Range(-bulletSpread, bulletSpread));
                        move.direction.y = tempShotDirection.y + (Random.Range(-bulletSpread, bulletSpread));
                        Vector2 swapShotDirection = tempShotDirection; // Used to store temporary values
                        tempShotDirection.x = swapShotDirection.y; // Rotates tempShot 90 degrees
                        tempShotDirection.y = -swapShotDirection.x; // These lines rotate 90 degrees
                        
                    }

                }
            }
            

            

            SoundEffectsHelper.Instance.MakeEnemyShotSound(); // TODO: Get this as public var?
        }
    }

    /// <summary>
    /// Is the weapon ready to create a new projectile?
    /// </summary>
    public bool CanAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
    }
}
