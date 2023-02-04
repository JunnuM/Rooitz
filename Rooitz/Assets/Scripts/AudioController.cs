using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton
public class AudioController : MonoBehaviour
{
    #region Singleton instance and initialization
    private static AudioController instance;
    private void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }
    private void Start() {
        if(playMusicOnStart) ToggleMusic(true);
        else
        {
            instance.musicSource.playOnAwake = false;
            ToggleMusic(false);
        }
    }
    #endregion

    #region References and variables
    [Header("Variables")]
    [SerializeField] private bool playMusicOnStart;

    [Header("Audio clips")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;

    [SerializeField] private AudioClip music_0, music_1;
    [SerializeField] private AudioClip[] windSounds;
    #endregion

    #region Audio clip playing functions
    public static void ToggleMusic(bool _on)
    {
        if(_on == instance.musicSource.isPlaying) return;
        
        if(_on) instance.musicSource.Play();
        else instance.musicSource.Pause();
    }
    public static void PlayMusicByID(int _id)
    {
        if(_id == 0) instance.musicSource.clip = instance.music_0;
        else instance.musicSource.clip = instance.music_1;
    }

    public static void PlayRandomWindSound()
    {
        int clip = Random.Range(0, instance.windSounds.Length);
        instance.effectSource.PlayOneShot(instance.windSounds[clip]);
    }
    #endregion

    #region Wind sound loop
    public static void StartWindSoundLoop(float _minWait, float _maxWait, float _maxLoopTime)
    {
        instance.StartCoroutine(LoopWindSounds(_minWait, _maxWait, _maxLoopTime));
    }

    static IEnumerator LoopWindSounds(float _minWait, float _maxWait, float _maxLoopTime)
    {
        float startTime = Time.time;
        while(Time.time < startTime + _maxLoopTime)
        {
            PlayRandomWindSound();
            yield return new WaitForSeconds(Random.Range(_minWait, _maxWait));
        }
    }
    #endregion
}