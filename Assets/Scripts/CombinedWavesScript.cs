using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedWavesScript : MonoBehaviour
{
    public Transform[] waveSpawners = null;

    // Instantiate all waveSpawners, then destroy self.
    void Start()
    {
        for (int i = 0; i < waveSpawners.Length; i++)
        {
            Instantiate(waveSpawners[i]);
        }
        Destroy(gameObject);
    }
}
