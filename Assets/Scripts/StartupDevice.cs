using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StartupDevice : MonoBehaviour
{
    public AudioSource audioSource;
    public TMP_Text songDisplay;
    public String defaultText;
    public Slider progressBar;

    [Header("Debug")]
    public StartupDisk startupDisk;
    public String sceneToLoad;

    private AudioClip curSong;
    private bool isLoading;

    private void Start()
    {
        audioSource.loop = true;
        isLoading = false;
        songDisplay.text = defaultText;
    }

    private void PlayList()
    {
        if (!audioSource.isPlaying)
        {
            curSong = startupDisk.song;
            audioSource.clip = curSong;
            audioSource.Play();
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "StartupDisk")
        {
            startupDisk = collision.gameObject.GetComponent<StartupDisk>();
            startupDisk.isPlaying = true;
            
            sceneToLoad = startupDisk.sceneToLoad;
            audioSource.clip = startupDisk.song;
            audioSource.Play();
            
            if (!isLoading)
            {
                //start async operation
                songDisplay.text = "Loading. Please wait..";
                StartCoroutine(LoadScene(sceneToLoad));
                Debug.Log("Loading scene: " + sceneToLoad);
                isLoading = true;
            }
        }
    }
    
    IEnumerator LoadScene(string scene)
    {
        //create async operation
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            progressBar.value = asyncOperation.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
