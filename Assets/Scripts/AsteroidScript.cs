using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public float size = 2f; //3 = Very big, 2 = normal, 1 = tiny. Affects speed of rotation and remaining breaks
    private float speedRotate = 10;
    public Transform[] smallerAsteroids = null;
    private MoveScript thisAsteroidMoveScript = null;

    // Start is called before the first frame update
    void Start()
    {
        speedRotate = Random.Range((25 / size), (70 / size));
        thisAsteroidMoveScript=GetComponent<MoveScript>();
    }

    void Awake()
    {
        transform.Rotate(0f, 0f, Random.Range(0f, 360f));
    }

    //Break into smaller asteroids when destroyed (called by HealthScript)
    public void BreakApart()
    {
        int i = Random.Range(0, smallerAsteroids.Length);
        if (smallerAsteroids.Length > 0) { 
            var brokenAsteroidTransform1 = Instantiate(smallerAsteroids[i]) as Transform;
            brokenAsteroidTransform1.position = transform.position - new Vector3(0.75f, 0f, 0f);
            var brokenAsteroidMovescript = brokenAsteroidTransform1.GetComponent<MoveScript>();
            brokenAsteroidMovescript.direction = thisAsteroidMoveScript.direction - new Vector2(0.4f, 0.2f);

            i = Random.Range(0, smallerAsteroids.Length);
            var brokenAsteroidTransform2 = Instantiate(smallerAsteroids[i]) as Transform;
            brokenAsteroidTransform2.position = transform.position + new Vector3(0.75f, 0f, 0f);
            brokenAsteroidMovescript = brokenAsteroidTransform2.GetComponent<MoveScript>();
            brokenAsteroidMovescript.direction = thisAsteroidMoveScript.direction + new Vector2(0.4f, 0.2f); new Vector2(0.4f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
    }
}
