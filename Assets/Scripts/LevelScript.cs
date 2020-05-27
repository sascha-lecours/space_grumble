using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public Transform[] easyWaves = null; // Holds array of wavespawner objects
    public Transform[] mediumWaves = null;
    public Transform[] hardWaves = null;
    public Transform[] bossWaves = null;
    public float initialDelay = 4f; // Seconds before first wave
    public float initialWaveInterval = 7f; // Time in seconds between waves at start
    public int numEasyWaves = 4;
    public int numMediumWaves = 3;
    public int numHardWaves = 2;

    private float nextWaveTime = 0f;
    private int index = 0;
    private float timeKeeper = 0f;
    private float waveInterval = 1f; // Set in Start method, time in seconds between waves
    private float waveIntervalIncreaseAtMedium = 0.5f;
    private float waveIntervalIncreaseAtHard = 0.5f;
    private float doubleEasyAsMedProportion = 0.35f;
    private float doubleEasyAsMedDelay = 2f; // Delay (s) between 2 easy waves as a medium
    private float easyAndMedAsHardProportion = 0.35f;
    private float easyAndMedAsHardDelay = 2f;
    private int bossIndex = 0;

    private void spawnWithDelay(Transform myObject, float delay)
    {
        Transform myDelayer = Instantiate(Resources.Load("Delayer", typeof(Transform))) as Transform;
        DelayScript ds = myDelayer.GetComponent<DelayScript>();
        if (ds != null)
        {
            ds.payload = myObject;
            ds.timeDelay = delay;
        }
    }

    private void spawnWave(int wavenumber)
    {
        if (index < numEasyWaves)
        {
            Debug.Log("Generating Easy wave at wave index " + index);
            Instantiate(randomWaveInDifficulty(easyWaves));
        } else if (index < numEasyWaves + numMediumWaves)
        {
            Debug.Log("Generating Medium wave at wave index " + index);
            if (waveInterval < initialWaveInterval + waveIntervalIncreaseAtMedium) waveInterval = initialWaveInterval + waveIntervalIncreaseAtMedium;
            var i = Random.Range(0f, 1f); // Random chance to spawn multiple easy waves instead.
            if (i < doubleEasyAsMedProportion)
            {
                Instantiate(randomWaveInDifficulty(easyWaves));
                spawnWithDelay(randomWaveInDifficulty(easyWaves), doubleEasyAsMedDelay);
            }
            else
            {
                Instantiate(randomWaveInDifficulty(mediumWaves));
            }
            
        } else if (index < numEasyWaves + numMediumWaves + numHardWaves)
        {
            
            Debug.Log("Generating Hard wave at wave index " + index);
            if (waveInterval < initialWaveInterval + waveIntervalIncreaseAtMedium + waveIntervalIncreaseAtHard) waveInterval = initialWaveInterval + waveIntervalIncreaseAtMedium + waveIntervalIncreaseAtHard;

            var i = Random.Range(0f, 1f); // Random chance to spawn an easy and a medium wave instead.
            if (i < easyAndMedAsHardProportion)
            {
                Instantiate(randomWaveInDifficulty(mediumWaves));
                spawnWithDelay(randomWaveInDifficulty(easyWaves), easyAndMedAsHardDelay);
            }
            else
            {
                Instantiate(randomWaveInDifficulty(hardWaves));
            }
            
        } else // If every single wave is done, spawn the boss.
        {
            Instantiate(bossWaves[bossIndex]);
        }
    }


    private Transform randomWaveInDifficulty(Transform[] waveArray)
    {
        var i = Random.Range(0, waveArray.Length);
        return waveArray[i];
    }

    private void Start()
    {
        waveInterval = initialWaveInterval; // Amount of time in seconds between waves
        timeKeeper -= initialDelay;
    }

    // Update is called once per frame
    void Update()
    {
        timeKeeper += Time.deltaTime;
        if (timeKeeper >= nextWaveTime)
        {
            if (index < numEasyWaves + numMediumWaves + numHardWaves + 1)
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
