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
            case "Grass2":
            case "Grass3":
            case "Grass4":
                PlayMusic(musicTracks[0]);
                break;
            case "Desert1":
            case "Desert2":
            case "Desert3":
            case "Desert4":
                PlayMusic(musicTracks[1]);
                break;
            case "Snow1":
            case "Snow2":
            case "Snow3":
            case "Snow4":
                PlayMusic(musicTracks[2]);
                break;
            case "Lava1":
            case "Lava2":
            case "Lava3":
            case "Lava4":
                PlayMusic(musicTracks[3]);
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
