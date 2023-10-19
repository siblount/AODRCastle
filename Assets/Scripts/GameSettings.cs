using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public static event Action GamePaused, GameUnpaused;
    public static float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = Mathf.Clamp01(value);
            Instance.PropograteAudioVolumeChange();
        }
    }
    public static float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = Mathf.Clamp01(value);
            Instance.PropograteAudioVolumeChange();
        }
    }
    public static FightSettings Difficulty;
    public static GameSettings Instance;
    public static bool Paused
    {
        get => paused;
        set
        {
            paused = value;
            HandlePause();
        }
    }
    private static bool paused;
    private static float previousTimeScale = -1;
    private static float musicVolume = 1;
    private static float sfxVolume = 0.45f;
    [SerializeField] private GameObject player;
    public static NotifySet<AudioSource> MusicSources = new(); 
    public static NotifySet<AudioSource> SFXSources = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogWarning("GameSettings object was already instanced...");
            Destroy(this);
            return;
        }
        Difficulty = FightSettings.Easy;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        previousTimeScale = Time.timeScale;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public static void AddMusicSource(AudioSource source)
    {
        source.volume = musicVolume;
        MusicSources.Add(source);
        SFXSources.Remove(source);
    }

    public static void RemoveMusicSource(AudioSource source)
    {
        MusicSources.Remove(source);
    }

    public static void AddSFXSource(AudioSource source)
    {
        source.volume = sfxVolume;
        SFXSources.Add(source);
        MusicSources.Remove(source);
    }

    public static void RemoveSFXSource(AudioSource source) => SFXSources.Remove(source);


    private void PropograteAudioVolumeChange()
    {
        foreach (var a in MusicSources)
        {
            if (a == null) continue;
            a.volume = musicVolume;
        }
        foreach (var a in SFXSources)
        {
            if (a == null) continue;
            a.volume = sfxVolume;
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode __)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RemoveNullAudioSources();
        var audioSources = FindObjectsOfType<AudioSource>(true);
        foreach (var src in audioSources)
        {
            if (MusicSources.Contains(src) || SFXSources.Contains(src)) continue;
            if (!src.playOnAwake || !src.gameObject.name.Contains("MusicSource") || src.gameObject.name.Contains("SoundEffect"))
                SFXSources.Add(src);
            else MusicSources.Add(src);
        }
        PropograteAudioVolumeChange();
        if (scene.name.Contains("Main")) Player.ResetPlayer();
    }

    void RemoveNullAudioSources()
    {
        var musicSources = new AudioSource[MusicSources.Count];
        var sfxSources = new AudioSource[SFXSources.Count];
        MusicSources.CopyTo(musicSources, 0);
        SFXSources.CopyTo(sfxSources, 0);
        foreach (var src in musicSources)
        {
            if (src == null) MusicSources.Remove(src);
        }
        foreach (var src in sfxSources)
        {
            if (src == null) SFXSources.Remove(src);
        }
    }

    private static void HandlePause()
    {
        if (!paused)
        {
            Time.timeScale = previousTimeScale;
            Instance.player.GetComponent<PlayerMovement>().enabled = true;
            GameUnpaused?.Invoke();
        } else
        {
            Time.timeScale = 0;
            Instance.player.GetComponent<PlayerMovement>().enabled = false;
            GamePaused?.Invoke();
        }
    }
}
