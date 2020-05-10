using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayScript : MonoBehaviour
{
    public float timeDelay = 0f; // Amount of seconds before spawning object
    public GameObject payload = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        timeDelay -= Time.deltaTime;
        if (timeDelay <= 0)
        {
            if(payload != null)
            {
                Instantiate(payload);
            }
            Destroy(gameObject);
        }

    }
}
