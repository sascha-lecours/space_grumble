using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiExplosionScript : MonoBehaviour
{
    public Transform[] Explosions = null;
    public float explosionInterval = 0.5f;
    public float explosionRangeX = 1f;
    public float explosionRangeY = 0.6f;

    private float timeKeeper = 0f;
    private int index = 0;
    private Transform myTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timeKeeper += Time.deltaTime;
        if (timeKeeper >= explosionInterval)
        {
            if (index < Explosions.Length)
            {
                var tempPosition = myTransform.position;
                Transform myExplosion = Instantiate(Explosions[index]);
                tempPosition += new Vector3(Random.Range(-explosionRangeX, explosionRangeX), Random.Range(-explosionRangeY, explosionRangeY), 0);
                myExplosion.position = tempPosition;

                index++;
                timeKeeper = 0;
            }
            else
            {
                Destroy(gameObject); // When explosions done being spawned, destroy self.
            }
        }
    }
}

