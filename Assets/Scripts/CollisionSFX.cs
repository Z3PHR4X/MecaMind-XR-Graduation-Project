using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSFX : MonoBehaviour
{
    public AudioClip sfx;
    private AudioSource sfxSource;

    private void Start()
    {
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        
        sfxSource.clip = sfx;
        sfxSource.volume = 0.2f;
    }

    private void OnCollisionEnter(Collision other)
    {
        sfxSource.Play();
    }
}
