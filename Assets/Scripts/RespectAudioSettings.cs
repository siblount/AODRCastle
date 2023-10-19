using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class RespectAudioSettings : MonoBehaviour
{
    public bool Music;
    public AudioSource src;
    // Start is called before the first frame update

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }
    void Start()
    {
        if (Music) GameSettings.AddMusicSource(src);
        else GameSettings.AddSFXSource(src);
    }

    private void OnEnable()
    {
        src.volume = Music ? GameSettings.MusicVolume : GameSettings.SFXVolume;
    }

    private void OnDestroy()
    {
        if (Music) GameSettings.RemoveMusicSource(src);
        else GameSettings.RemoveSFXSource(src);
    }
}
