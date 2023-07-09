using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private AudioSource audioSource;
    private bool musicPaused;

    public AudioClip playerMoveSound;
    public AudioClip playerDeathSound;
    public AudioClip playerWinSound;

    protected new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen":
            case "Grass1":
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

    public void PlaySound(AudioClip sound, float volume)
    {
        audioSource.PlayOneShot(sound, volume);
    }
}
