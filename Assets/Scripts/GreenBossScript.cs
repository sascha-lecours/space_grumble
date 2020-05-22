using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBossScript : MonoBehaviour
{
    // TODO: Remember to uncheck "active" on this gameobject's prefab when wakeup sequence is implemented

    public float floatstrength = 0.5f; // Determines vertical float distance from start point
    public Transform spawnedEnemy = null;
    public float spawnInterval = 1.5f; // Time in seconds between spawning helpers

    private int movementX = -1; // Should hold int value from 1 to -1
    private MoveScript ms = null;
    private HealthScript hs = null;
    private Transform myTransform = null;
    private float originalY = 0f; // 0 here corresponds to a special "null" state
    private float floatY = 0f;
    private bool timerStarted = false;
    private float spawnTimer = 0f;
    private float startYPosition = 5.2f;

    // Start is called before the first frame update
    void Start()
    {
        ms = GetComponent<MoveScript>();
        hs = GetComponent<HealthScript>();
        myTransform = GetComponent<Transform>();
        spawnTimer = spawnInterval;
        
    }

    // TODO: Spawn above screen and (remain invincible until?) reaching start point 2/3 of the way up.


    // On recurring timer, shoot homing projectile cluster

    // TODO: "Warning" intro should appear before/during boss activation step.

    void SpawnTimerFunction()
    {
        if (!timerStarted)
        {
            timerStarted = true;
        }
        if (timerStarted)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                SpawnEnemy();
                spawnTimer = spawnInterval;
            }
        }
    }

    void SpawnEnemy()
    {
        var spawnedEnemyTransform = Instantiate(spawnedEnemy) as Transform;
        spawnedEnemyTransform.position = transform.position + new Vector3(-movementX*0.8f, 0.1f, 0f);
        var spawnedEnemyMovescript = spawnedEnemyTransform.GetComponent<MoveScript>();
        spawnedEnemyMovescript.direction = ms.direction + new Vector2(-movementX*1.2f, 0f);
        SoundEffectsHelper.Instance.MakeGreenBossEnemySpawnSound();
    }
    
    void UpdateXDirection()
    {
        if (ms != null)
        {
            ms.direction.x = movementX;
        }
    }

    void HoverOnYAxis()
    {
        if (originalY == 0) originalY = transform.position.y; // set default Y on activation
        // Float along a small sinusoidal pattern without using Movescript
        floatY = transform.position.y;
        floatY = originalY + (Mathf.Sin(Time.time)) * floatstrength;
        Vector3 temp = transform.position;
        temp.y = floatY;
        myTransform.position = temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (hs.active)
        {
            SpawnTimerFunction(); // Starts/decrements/resets spawn timer and spawns enemies
            UpdateXDirection();
            HoverOnYAxis();
        }
        else
        {
            ms.direction = new Vector2(0, -1);
            if (transform.position.y <= startYPosition)
            {
                hs.active = true;
                ms.direction = new Vector2(0, 0);
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a bumper?
        BumperScript bumper = otherCollider.gameObject.GetComponent<BumperScript>();
        if (bumper != null)
        {
            if (movementX == -1 && bumper.bumperSide == BumperScript.Bumpers.left)
            {
                movementX = 1;
            }
            else if (movementX == 1 && bumper.bumperSide == BumperScript.Bumpers.right)
            { 
                movementX = -1;
            }
        }
    }
}
