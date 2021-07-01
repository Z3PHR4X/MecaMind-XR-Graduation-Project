using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaylistDisk : MonoBehaviour
{
    [Header("Setup")]
    public String playlistName;
    public Texture playlistCover;
    public List<AudioClip> songs;

    [Header("Debug")]
    public bool isPlaying = false;
    public int index = 0;
    
    private Renderer render;
    private Animation anim;
    

    private void Awake()
    {
        render = GetComponent<Renderer>();
        anim = GetComponent<Animation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        render.materials[1].mainTexture = playlistCover;
        
        gameObject.name = "Playlist Disk (" + playlistName + ")";
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            anim.Play();
        }
        else
        {
            anim.Stop();
        }
    }
}
