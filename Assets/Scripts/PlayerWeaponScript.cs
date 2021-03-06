﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour {
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

    //--------------------------------
    // 2 - Cooldown
    //--------------------------------
    private float shootCooldown;

    public bool multibarrelled = true;
    public Vector3[] shotOffsets = { new Vector3(-0.25f, 0f, 0f), new Vector3(0.25f, 0f, 0f) }; // Distance to offset shot. cycles through array.
    private int nextBarrel = 0;

    



    void Start()
    {
        shootCooldown = 0f;
    }

    void Update()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    //--------------------------------
    // 3 - Shooting from another script
    //--------------------------------

    /// <summary>
    /// Create a new projectile if possible
    /// </summary>
    public void Attack(bool isEnemy)
    {
        if (CanAttack)
        {
            Vector3[] tempOffset = { new Vector3(0f, 0f, 0f) };
            if (multibarrelled)
            {
                tempOffset = shotOffsets;
            }
            shootCooldown = shootingRate;

            // Create a new shot
            var shotTransform = Instantiate(shotPrefab) as Transform;

            // Assign position
            shotTransform.position = transform.position + tempOffset[nextBarrel];

            // Cycle through the barrels on the weapons
            nextBarrel++;
            if (nextBarrel > tempOffset.Length-1)
            {
                nextBarrel = 0;
            }


            // The is enemy property
            ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
            if (shot != null)
            {
                shot.isEnemyShot = isEnemy;
            }

            // Make the weapon shot always towards it
            MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
            if (move != null)
            {
                move.direction = this.transform.up;
            }

            //Make shot sound
            SoundEffectsHelper.Instance.MakePlayerShotSound();

            //Add screenshake
            //Camera.main.GetComponent<CameraControl>().Shake(0.1f, 1, 3); //intensity (distance), number of shakes, speed of movement
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
