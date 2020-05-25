using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

    public float shakeDistance = 0.25f;
    public int shakePoints = 10;
    public int shakeSpeed = 10;

    // Use this for initialization
    void Start()
    {
        // Limited time to live to avoid any leak
        Destroy(gameObject, 3); // 3sec

        // Explosion Sound
        SoundEffectsHelper.Instance.MakeExplosionSound();

        // Add screenshake
        Camera.main.GetComponent<CameraControl>().Shake(shakeDistance, shakePoints, shakeSpeed); //intensity (distance), number of shakes, speed of movement
    }
    
}
