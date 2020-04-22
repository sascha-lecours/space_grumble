using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

    //TODO: add public variables for customization 

    // Use this for initialization
    void Start()
    {
        // Limited time to live to avoid any leak
        Destroy(gameObject, 3); // 3sec

        //Add screenshake
        Camera.main.GetComponent<CameraControl>().Shake(0.25f, 5, 10); //intensity (distance), number of shakes, speed of movement
    }
    
}
