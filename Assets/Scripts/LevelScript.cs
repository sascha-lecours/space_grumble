using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public Transform[] easyWaves = null; // Holds array of wavespawner objects
    public Transform[] mediumWaves = null;
    public Transform[] hardWaves = null;
    public float initialWaveInterval = 7f; // Time in seconds between waves at start

    private float nextWaveTime = 0f;
    private int index = 0;
    private float timeKeeper = 0f;
    private float waveInterval = 1f; // Set in Start method

    private Transform getWave(int wavenumber)
    {
        // TODO: This whole thing basically, lol
        var i = Random.Range(0, easyWaves.Length);
        return easyWaves[i];
    }

    private void Start()
    {
        waveInterval = initialWaveInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timeKeeper += Time.deltaTime;
        if (timeKeeper >= nextWaveTime)
        {
            Instantiate(getWave(index));
            index++;
            nextWaveTime += waveInterval;
        }
    }
}
