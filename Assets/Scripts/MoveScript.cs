﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simply moves the current game object
/// </summary>
public class MoveScript : MonoBehaviour
{
    public Vector2 startSpeed = new Vector2(0f, 0f);
    
    public Vector2 speed = new Vector2(10f, 10f);
    public Vector2 direction = new Vector2(-1, 0);
    public float acceleration = 1f; // Range from 0 to 1, 1 being instant.

    private Vector2 curSpeed = new Vector2(0f, 0f);
    private Vector2 movement;
    private Rigidbody2D rigidbodyComponent;

    private void Start()
    {
        curSpeed = startSpeed;
    }

    void Update()
    {
        curSpeed.x =  Mathf.Lerp(curSpeed.x, speed.x, (acceleration * Time.deltaTime));
        curSpeed.y = Mathf.Lerp(curSpeed.y, speed.y, (acceleration * Time.deltaTime));

        movement = new Vector2(
          curSpeed.x * direction.x,
          curSpeed.y * direction.y);
    }

    void FixedUpdate()
    {
        if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

        // Apply movement to the rigidbody
        rigidbodyComponent.velocity = movement;
    }
}
