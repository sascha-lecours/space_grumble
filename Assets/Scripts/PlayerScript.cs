﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    /// <summary>
    /// 1 - The speed of the ship
    /// </summary>
    public Vector2 speed = new Vector2(15, 12);

    // 2 - Store the movement and the component
    private Vector2 movement;
    private Rigidbody2D rigidbodyComponent;
    Animator anim;


    // Update is called once per frame
    void Update()
    {
        // 3 - Retrieve axis information
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // 4 - Movement per direction
        movement = new Vector2(
          speed.x * inputX,
          speed.y * inputY);

        // Update flame trail when moving downward or not.
        Transform PlayerFlame = transform.Find("PlayerFlame");
        anim = PlayerFlame.GetComponent<Animator>();

        if (PlayerFlame != null & inputY < 0)
        {
            anim.SetTrigger("flame_shrink");
        }
        else
        {
            anim.SetTrigger("flame_grow");
        }

        // 5 - Shooting
        bool shoot = Input.GetButton("Fire1");
        shoot |= Input.GetButton("Fire2");
        // Careful: For Mac users, ctrl + arrow is a bad idea
        
        
        if (shoot)
        {
            PlayerWeaponScript weapon = GetComponentInChildren<PlayerWeaponScript>();
            if (weapon != null)
            {
                // false because the player is not an enemy
               weapon.Attack(false);
            }
        }

        // 6 - Make sure we are not outside the camera bounds
        var dist = (transform.position - Camera.main.transform.position).z;

        var leftBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 0, dist)
        ).x;

        var rightBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(1, 0, dist)
        ).x;

        var topBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 0, dist)
        ).y;

        var bottomBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 1, dist)
        ).y;

        transform.position = new Vector3(
          Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
          Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
          transform.position.z
        );

    }

    void FixedUpdate() //Called each physics interaction
    {
        // 5 - Get the component and store the reference
        if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

        // 6 - Move the game object
        rigidbodyComponent.velocity = movement;
    }

  
    void OnCollisionEnter2D(Collision2D collision)
    {
        bool damagePlayer = false;

        // Collision with enemy
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            // Kill the enemy
            HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
            if (enemyHealth != null) enemyHealth.Damage(1);

            damagePlayer = true;
        }

        AsteroidScript asteroid = collision.gameObject.GetComponent<AsteroidScript>();
        if (asteroid != null)
        {
            // Kill the enemy
            HealthScript asteroidHealth = asteroid.GetComponent<HealthScript>();
            if (asteroidHealth != null) asteroidHealth.Damage(1);

            damagePlayer = true;
        }

        // Damage the player
        if (damagePlayer)
        {
            HealthScript playerHealth = this.GetComponent<HealthScript>();
            if (playerHealth != null) playerHealth.Damage(1);
        }
    }

    void OnDestroy()
    {
        // Game Over.
        var gameOver = FindObjectOfType<GameOverScript>();
        Debug.Log("GameOver Script = " + gameOver);
        if (gameOver != null)
        {
            gameOver.ShowButtons();
        }
        
    }
}