using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicDisk : MonoBehaviour
{
    [Header("Setup")]
    public AudioClip song;
    public TMP_Text songDisplay;

    [Header("Debug")]
    public bool isPlaying = false;

    private Color diskColor;
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
        diskColor = Random.ColorHSV(0,1,0.8f,1,0.5f,1);
        render.materials[1].color = diskColor;
        
        string musicTitle = song.name;
        musicTitle = musicTitle.Replace("(UnityEngine.AudioClip)", "");
        gameObject.name = "Music Disk (" + musicTitle + ")";
        songDisplay.text = musicTitle;
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
