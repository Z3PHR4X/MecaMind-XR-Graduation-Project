using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public MusicDisk disk;
    public AudioSource audioSource;

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.UnPause();
    }
    
    public void AdjustVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "MusicDisk")
        {
            disk = collision.gameObject.GetComponent<MusicDisk>();
            disk.isPlaying = true;
            
            audioSource.clip = disk.song;
            audioSource.Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "MusicDisk")
        {
            disk.isPlaying = false;
            
            disk = null;
            audioSource.clip = null;
            audioSource.Pause();
        }
    }
}
