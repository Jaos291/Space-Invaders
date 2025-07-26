using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // --- Singleton ---
    public static AudioManager Instance { get; private set; }

    // --- Serialized fields ---
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip uiButtonClip;
    [SerializeField] private AudioClip countdownClip;
    [SerializeField] private AudioClip countdownLastSecondClip;
    [SerializeField] private AudioClip playerShootClip;
    [SerializeField] private AudioClip enemyShootClip;
    [SerializeField] private AudioClip enemyDestroyedClip;
    [SerializeField] private AudioClip playerLostLifeClip;
    [SerializeField] private AudioClip soundtrackStartScreen;
    [SerializeField] private AudioClip soundtrackGameplay;
    [SerializeField] private AudioClip soundtrackGameOver;
    [SerializeField] private AudioClip soundtrackVictory;

    // --- Private fields ---
    private Dictionary<string, AudioClip> sfxClips;

    // --- Unity event methods ---
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitClips();
    }

    // --- Private methods ---
    private void InitClips()
    {
        sfxClips = new Dictionary<string, AudioClip>
        {
            { "UIButton", uiButtonClip },
            { "Countdown", countdownClip },
            { "CountdownLastSecond", countdownLastSecondClip },
            { "PlayerShoot", playerShootClip },
            { "EnemyShoot", enemyShootClip },
            { "EnemyDestroyed", enemyDestroyedClip },
            { "PlayerLostLife", playerLostLifeClip }
        };
    }

    // --- Public methods ---
    public void PlaySFX(string sfxName)
    {
        if (sfxClips.ContainsKey(sfxName) && sfxClips[sfxName] != null)
        {
            sfxSource.PlayOneShot(sfxClips[sfxName]);
        }
    }

    public void PlayMusic(string musicName)
    {
        AudioClip clip = null;
        switch (musicName)
        {
            case "StartScreen":
                clip = soundtrackStartScreen;
                break;
            case "Gameplay":
                clip = soundtrackGameplay;
                break;
            case "GameOver":
                clip = soundtrackGameOver;
                break;
            case "Victory":
                clip = soundtrackVictory;
                break;
        }
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
