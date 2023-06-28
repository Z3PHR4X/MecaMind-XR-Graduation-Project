using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

namespace MecaMind.UI
{
    public class MusicPicker : MonoBehaviour
    {
        [Header("Music Selection")]
        [SerializeField] private List<AudioClip> musicPlaylist;

        [Header("Setup")] 
        [SerializeField] private AudioSource musicPlayer;
        [SerializeField] private TMP_Dropdown musicSelector;
        [SerializeField] private Slider volumeSlider;

        private bool isPlaying = false;

        // Start is called before the first frame update
        void Start()
        {
            List<string> musicList = new List<string>();
            musicSelector.options.Clear();
            for (int i = 0; i < musicPlaylist.Count; i++)
            {
                string musicTitle = (i+1) + ". " + musicPlaylist[i];
                musicTitle = musicTitle.Replace("(UnityEngine.AudioClip)", "");
                musicList.Add(musicTitle);
                //Debug.Log("[MusicPicker] Added: " + i + " - " + musicTitle);
            }
            musicSelector.AddOptions(musicList);
            musicSelector.value = 0;
            musicSelector.RefreshShownValue();

            volumeSlider.value = musicPlayer.volume;
            
            musicPlayer.clip = musicPlaylist[0];
            musicPlayer.Play();
        }

        public void ChangeSong(int index)
        {
            if (index < musicPlaylist.Count)
            {
                musicPlayer.clip = musicPlaylist[index];
                musicPlayer.Play();
            }
        }

        public void TogglePlaying()
        {
            isPlaying = !isPlaying;
            if (isPlaying)
            {
                musicPlayer.Play();
            }
            else
            {
                musicPlayer.Pause();
            }
        }

        public void AdjustVolume(float volume)
        {
            musicPlayer.volume = volume;
        }
    }
}

