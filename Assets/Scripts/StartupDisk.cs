using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class StartupDisk : MonoBehaviour
{
    [Header("Setup")] 
    public String sceneToLoad;
    public AudioClip song;
    public Texture diskCover;

    [Header("Debug")]
    public bool isPlaying = false;

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
        render.materials[1].mainTexture = diskCover;
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
