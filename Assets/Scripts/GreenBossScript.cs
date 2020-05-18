using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBossScript : MonoBehaviour
{
    // TODO: Remember to uncheck "active" on this gameobject's prefab when wakeup sequence is implemented

    private int movementX = -1; // Should hold int value from 1 to -1
    private int movementY = 0; // Should hold int value from 1 to -1
    private MoveScript ms = null;
    private Transform myTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        ms = GetComponent<MoveScript>();
        myTransform = GetComponent<Transform>();
    }

    // TODO: Spawn above screen and (remain invincible until?) reaching start point 2/3 of the way up.
    // Movement: move horizontally from one side to the other, almost to the screen edges.
    // Oscillate vertically just slightly, for aesthetics

    // On recurring timer, shoot homing projectile cluster

    // On separate recurring timer, spawn green enemies (from self, or offscreen?)

    // TODO: "Warning" intro should appear before/during boss activation step.

    // Update is called once per frame
    void Update()
    {
        if (ms != null)
        {
            ms.direction.x = movementX;
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
