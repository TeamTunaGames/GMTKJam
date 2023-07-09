using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private AudioSource audioSource;
    private bool musicPaused;

    protected new void Awake()
    {
        base.Awake();
    }
    private void Update()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen":
            case "Level1":
                PlayMusic(musicTracks[0]);
                break;
        }
    }
    public void PlayMusic(AudioClip song)
    {
        if (audioSource.isPlaying != song && !musicPaused)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(song);
        }
    }

    public void PauseMusic()
    {
        musicPaused = true;
        audioSource.Pause();
    }

    public void UnpauseMusic()
    {
        musicPaused = false;
        audioSource.UnPause();
    }
}
