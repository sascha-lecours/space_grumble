using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public Transform[] easyWaves = null; // Holds array of wavespawner objects
    public Transform[] mediumWaves = null;
    public Transform[] hardWaves = null;
    public float initialWaveInterval = 7f; // Time in seconds between waves at start
    public int numEasyWaves = 4;
    public int numMediumWaves = 3;
    public int numHardWaves = 2;

    private float nextWaveTime = 0f;
    private int index = 0;
    private float timeKeeper = 0f;
    private float waveInterval = 1f; // Set in Start method
    private float doubleEasyAsMedProportion = 0.25f;

    private void spawnWave(int wavenumber)
    {
        if (index < numEasyWaves)
        {
            Debug.Log("Generating Easy wave at wave index " + index);
            Instantiate(randomWaveInDifficulty(easyWaves));
        } else if (index < numEasyWaves + numMediumWaves)
        {
            Debug.Log("Generating Medium wave at wave index " + index);
            var i = Random.Range(0f, 1f); // Random chance to spawn multiple easy waves instead.
            if (i < doubleEasyAsMedProportion)
            {
                randomWaveInDifficulty(mediumWaves); // Placeholder
            }
            Instantiate((randomWaveInDifficulty(mediumWaves)));
        } else if (index < numEasyWaves + numMediumWaves + numHardWaves)
        {
            Debug.Log("Generating Hard wave at wave index " + index);
            Instantiate(randomWaveInDifficulty(hardWaves));
        }
    }

    private Transform randomWaveInDifficulty(Transform[] waveArray)
    {
        var i = Random.Range(0, waveArray.Length);
        return waveArray[i];
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
            if (index < numEasyWaves + numMediumWaves + numHardWaves)
            {
                spawnWave(index);
                index++;
                nextWaveTime += waveInterval;
                waveInterval = initialWaveInterval;
            } else
            {
                Debug.Log("Level finished spawning, self destructing.");
                Destroy(gameObject); // When level done being spawned, destroy self. TODO: Handle this differently.
            }
        }
    }
}
