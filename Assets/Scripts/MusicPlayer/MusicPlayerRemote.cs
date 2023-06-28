using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MusicPlayerRemote : MonoBehaviour
{
    [Header("Target")]
    public MusicPlayerV2 musicPlayer;

    [Header("Settings")] 
    [SerializeField] private float volumeIncrement = 0.05f;

    [Header("SteamVR")] 
    [SerializeField] private SteamVR_Input_Sources controlHandType;
    [SerializeField] private SteamVR_Action_Boolean increaseVolumeAction;
    [SerializeField] private SteamVR_Action_Boolean decreaseVolumeAction;
    [SerializeField] private SteamVR_Action_Vector2 skipSongAction;
    [SerializeField] private SteamVR_Action_Vibration hapticAction;

    
    [Header("Other")] 
    [SerializeField] private GameObject statusLight;

    private float volume;
    private bool isPlaying;
    private Renderer render;

    private void Start()
    {
        volume = musicPlayer.audioSource.volume;
        isPlaying = musicPlayer.audioSource.isPlaying;

        render = statusLight.GetComponent<Renderer>();
    }

    public void UseRemote()
    {
        if (increaseVolumeAction.GetStateDown(controlHandType))
        {
            IncreaseVolume();
            UpdateLight();
            
            if (SteamVR.active)
            {
                Pulse(0.05f, 150, 30, controlHandType);
            }
        }

        if (decreaseVolumeAction.GetStateDown(controlHandType))
        {
            DecreaseVolume();
            UpdateLight();
            
            if (SteamVR.active)
            {
                Pulse(0.05f, 150, 20, controlHandType);
            }
        }

        if (skipSongAction.GetAxis(controlHandType).y > 0.8f)
        {
            SkipSong();
            UpdateLight();
        }

        if (skipSongAction.GetAxis(controlHandType).y < -0.8f)
        {
            PreviousSong();
            UpdateLight();
        }
    }

    public void AssignHand()
    {
        string handName = this.transform.parent.name;
        if (handName.Contains("(right)"))
        {
            controlHandType = SteamVR_Input_Sources.RightHand;
        }
        else if(handName.Contains("(left)"))
        {
            controlHandType = SteamVR_Input_Sources.LeftHand;
        }
    }

    private void TogglePlayback()
    {
        if (isPlaying)
        {
            isPlaying = false;
            Debug.Log("pausing");
            musicPlayer.Pause(); 
        }

        if (!isPlaying)
        {
            Debug.Log("playing");
            isPlaying = true;
            musicPlayer.Resume();
        }
    }

    private void SkipSong()
    {
        musicPlayer.NextSong();
    }

    private void PreviousSong()
    {
        musicPlayer.LastSong();
    }

    private void IncreaseVolume()
    {
        volume += volumeIncrement;
        musicPlayer.AdjustVolume(volume);
    }

    private void DecreaseVolume()
    {
        volume -= volumeIncrement;
        musicPlayer.AdjustVolume(volume);
    }

    private void UpdateLight()
    {
        render.material.SetColor("_EmissionColor", Color.red);
        Invoke("ResetLight", 0.5f);
    }

    private void ResetLight()
    {
        render.material.SetColor("_EmissionColor", Color.black);
    }
    
    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
