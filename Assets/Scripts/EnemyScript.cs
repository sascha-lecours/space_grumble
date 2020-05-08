using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public bool hunt_player = false;
    private WeaponScript[] weapons;
    private Transform playerTransform = null;
    private MoveScript ms;
    private Vector2 tempDirection;
    private Transform myTransform;

    void Awake()
    {
        // Retrieve the weapon only once
        weapons = GetComponentsInChildren<WeaponScript>();
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Start()
    {
        ms = GetComponent<MoveScript>();
        myTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (hunt_player)
        {
            tempDirection = playerTransform.position - myTransform.position;
            tempDirection.Normalize();
            ms.direction = tempDirection;
        }
        foreach (WeaponScript weapon in weapons)
        {
            // Auto-fire
            if (weapon != null && weapon.CanAttack)
            {
                weapon.Attack(true);
            }
        }
    }
}
