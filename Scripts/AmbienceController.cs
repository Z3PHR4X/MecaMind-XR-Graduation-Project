using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceController : MonoBehaviour
{
    [Header("Settings")] 
    public bool enable = true;
    public float defaultVolume = 0.1f;
    public float duckVolume = 0.0f;
    
    [Header("Debug")]
    public float targetVolume;
    
    private MusicPlayerV2 musicPlayer;
    private AudioSource ambience;

    private void Awake()
    {
        if (enable)
        {
            musicPlayer = FindObjectOfType<MusicPlayerV2>();
        }
        ambience = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ambience.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            if (musicPlayer.audioSource.isPlaying)
            {
                targetVolume = duckVolume;
                ambience.volume = Mathf.Lerp(ambience.volume, duckVolume, 0.4f * Time.deltaTime);
            }
            else
            {
                targetVolume = defaultVolume;
                ambience.volume = Mathf.Lerp(ambience.volume, defaultVolume, 0.1f * Time.deltaTime);
            }
        }
        else
        {
            targetVolume = defaultVolume;
            ambience.volume = Mathf.Lerp(ambience.volume, defaultVolume, 0.1f * Time.deltaTime);
        }
    }
}
