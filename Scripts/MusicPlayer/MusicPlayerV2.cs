using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MusicPlayerV2 : MonoBehaviour
{
    //V2 has PlaylistDisk compatibility
    public float volume = 0.2f;
    public AudioSource audioSource;
    public TMP_Text songDisplay;
    public Slider progressBar;
    public WindController windController;
    
    [Header("Debug")]
    public MusicDisk mDisk;
    public PlaylistDisk pDisk;

    private AudioClip curSong;
    private float songIntensity;

    private void Update()
    {
        if (!pDisk && !mDisk)
        {
            String hour;
            if (System.DateTime.Now.Hour < 10)
            {
                hour = "0" + System.DateTime.Now.Hour.ToString();
            }
            else
            {
                hour = System.DateTime.Now.Hour.ToString();
            }

            String minute;
            if (System.DateTime.Now.Minute < 10)
            {
                minute = "0" + System.DateTime.Now.Minute.ToString();
            }
            else
            {
                minute = System.DateTime.Now.Minute.ToString();
            }
        
            songDisplay.text = hour + ":" + minute;  
        }

        if (audioSource.isPlaying)
        {
            progressBar.value += Time.deltaTime;
        }

        if (windController)
        {
            windController.ChangeStrength(GetAveragedVolume());
        }

        audioSource.volume = Mathf.Lerp(audioSource.volume, volume, 1.3f * Time.deltaTime);
    }

    private void PlayList()
    {
        if (!audioSource.isPlaying)
        {
            //Debug.Log("PlayList is doing its thing");
            int curIndex = pDisk.index;
            
            curSong = pDisk.songs[curIndex];
            audioSource.clip = curSong;
            Invoke("NextSong", audioSource.clip.length);
            audioSource.Play();
            progressBar.maxValue = audioSource.clip.length;
            progressBar.value = 0;

            string musicTitle = curSong.name;
            musicTitle = musicTitle.Replace("(UnityEngine.AudioClip)", ""); 
            songDisplay.text = "(" + (curIndex + 1) + "/" + pDisk.songs.Count + ") " + musicTitle;
        }
    }

    public void NextSong()
    {
        //Debug.Log("Skipping song!");
        CancelInvoke();
        if (pDisk)
        { 
            //Increment index
            pDisk.index++;

            if (pDisk.index >= pDisk.songs.Count)
            {
                pDisk.index = 0;
            }
            audioSource.Stop();
            PlayList();
        }
    }
    
    public void LastSong()
    {
        //Debug.Log("Skipping song!");
        CancelInvoke();
        if (pDisk)
        { 
            //Increment index
            pDisk.index--;

            if (pDisk.index < 0)
            {
                pDisk.index = pDisk.songs.Count-1;
            }
            audioSource.Stop();
            PlayList();
        }
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.UnPause();
    }
    
    public void AdjustVolume(float newVolume)
    {
        volume = newVolume;
    }
    
    //This part contains modified code from: https://forum.unity.com/threads/updating-the-intensity-with-audio.761846/
    //Original code by: IronDestruct
    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        audioSource.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }

        songIntensity = a;
        return a/3; //Increase value to decrease intensity
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "MusicDisk")
        {
            mDisk = collision.gameObject.GetComponent<MusicDisk>();
            mDisk.isPlaying = true;
            
            string musicTitle = mDisk.song.name; 
            musicTitle = musicTitle.Replace("(UnityEngine.AudioClip)", "");
            songDisplay.text = musicTitle;

            audioSource.loop = true;
            audioSource.clip = mDisk.song;
            audioSource.Play();
            progressBar.maxValue = audioSource.clip.length;
            progressBar.value = 0;
        }

        if (collision.gameObject.tag == "PlaylistDisk")
        {
            //Debug.Log("Disk found!");
            pDisk = collision.gameObject.GetComponent<PlaylistDisk>();
            pDisk.isPlaying = true;
            audioSource.loop = false;
            PlayList();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "MusicDisk")
        {
            mDisk = collision.gameObject.GetComponent<MusicDisk>();
            mDisk.isPlaying = false;
            
            mDisk = null;
            audioSource.Pause();
            audioSource.clip = null;
            progressBar.value = 0;
        }

        if (collision.gameObject.tag == "PlaylistDisk")
        {
            CancelInvoke();
            pDisk = collision.gameObject.GetComponent<PlaylistDisk>();
            pDisk.isPlaying = false;

            pDisk = null;
            audioSource.Pause();
            audioSource.clip = null;
            progressBar.value = 0;
        }
    }
}
