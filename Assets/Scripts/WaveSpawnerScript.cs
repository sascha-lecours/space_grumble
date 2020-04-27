using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerScript : MonoBehaviour
{
    public Transform[] enemies = null;
    public float offset = 2f; // Offset from screen edge
    public float spacing = 3f; // Space between enemies in wave

    //Enums for parameters
    public enum PatternTypes { horizontal_line_rightward, vertical_line_upward } //Add leftward/downward?
    public enum DirectionTypes { down, up, left, right, hitPlayer}
    public enum StartPositionsX { screen_left, screen_middle_x, screen_right }
    public enum StartPositionsY
    { screen_top, screen_middle_y, screen_bottom}

    //Specific parameters for this instance
    public PatternTypes Pattern = PatternTypes.horizontal_line_rightward;
    public DirectionTypes Direction = DirectionTypes.down;
    public StartPositionsX StartPositionX = StartPositionsX.screen_left;
    public StartPositionsY StartPositionY = StartPositionsY.screen_top;

    private Vector3 startPoint = new Vector3(0, 0, 0);
    private Vector3 spacingVector = new Vector3(1, 1, 0);
    private Vector3 directionVector = new Vector2(0, -1);
    private Transform playerTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnEnemies();
        Destroy(this);
    }

    void setStartPointCoordinates()
    {
        var lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        var upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        //Set X
        if (StartPositionX == StartPositionsX.screen_left)
        {
            startPoint.x = lowerLeft.x + offset;
        } else if (StartPositionX == StartPositionsX.screen_middle_x)
        {
            startPoint.x = (lowerLeft.x + upperRight.x) / 2;
        } else if (StartPositionX == StartPositionsX.screen_right)
        {
            startPoint.x = upperRight.x - offset;
        }

        //Set Y
        if (StartPositionY == StartPositionsY.screen_top)
        {
            startPoint.y = upperRight.y + offset;
        }
        else if (StartPositionY == StartPositionsY.screen_middle_y)
        {
            startPoint.y = (lowerLeft.y + upperRight.y) / 2;
        }
        else if (StartPositionY == StartPositionsY.screen_bottom)
        {
            startPoint.y = lowerLeft.y - offset;
        }
        

    }

    void setSpacingVector()
    {
        if (Pattern == PatternTypes.horizontal_line_rightward)
        {
            spacingVector = new Vector3(1, 0, 0);
        } else if (Pattern == PatternTypes.vertical_line_upward)
        {
            spacingVector = new Vector3(0, 1, 0);
        }
    }

    void setDirectionVector()
    {
        if (Direction == DirectionTypes.down)
        {
            directionVector = new Vector2(0, -1);
        } else if (Direction == DirectionTypes.left)
        {
            directionVector = new Vector2(-1, 0);
        } else if (Direction == DirectionTypes.up)
        {
            directionVector = new Vector2(0, 1);
        } else if (Direction == DirectionTypes.right)
        {
            directionVector = new Vector2(1, 0);
        } else if (Direction == DirectionTypes.hitPlayer)
        {
            directionVector = new Vector2(0, 0); //This will be set at spawn time instead
        }
    }

    void SpawnEnemies()
    {
        setStartPointCoordinates();
        setSpacingVector();
        setDirectionVector();

        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = Instantiate(enemies[i]) as Transform;
            enemy.position = startPoint + (i * spacing * spacingVector);
            if (Direction == DirectionTypes.hitPlayer)
            {
                directionVector = playerTransform.position - enemy.position;
                directionVector.Normalize();
            }
            enemy.GetComponent<MoveScript>().direction = directionVector;
        }
    }
}
