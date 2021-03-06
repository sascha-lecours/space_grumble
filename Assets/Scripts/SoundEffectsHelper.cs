﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsHelper : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static SoundEffectsHelper Instance;

    public AudioClip[] explosionSounds;
    public AudioClip[] playerShotSounds;
    public AudioClip enemyShotSound;
    public AudioClip[] hitHurtSounds;
    public AudioClip greenBossEnemySpawnSound;
    public float playerShotVolume = 0.4f;
    private float hitHurtVolume = 1f;
    private AudioSource MusicPlayer;

    // TODO: Make pool of explosion sounds and randomly select one when played

    void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundEffectsHelper!");
        }
        Instance = this;
    }

    void Start()
    {
        MusicPlayer = GetComponent<AudioSource>();
    }

    public void MakeExplosionSound()
    {
        var i = Random.Range(0, (explosionSounds.Length - 1));
        MakeSound(explosionSounds[i]);
    }

    public void MakePlayerShotSound()
    {
        var i = Random.Range(0, (playerShotSounds.Length - 1));
        MakeSound(playerShotSounds[i], playerShotVolume);
    }

    public void MakeHitHurtSound()
    {
        var i = Random.Range(0, (hitHurtSounds.Length - 1));
        MakeSound(hitHurtSounds[i], hitHurtVolume);
    }

    public void MakeEnemyShotSound()
    {
        MakeSound(enemyShotSound);
    }

    public void MakeGreenBossEnemySpawnSound()
    {
        MakeSound(greenBossEnemySpawnSound);
    }


    /// <summary>
    /// Play a given sound
    /// </summary>
    /// <param name="originalClip"></param>
    private void MakeSound(AudioClip originalClip, float volume=1f)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position, volume);
    }
}
