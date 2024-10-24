using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public Sound[] sounds;
    public Music[] music;

    public MusicEnum musicToPlay;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
        }

        foreach (Music sound in music)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
        }
    }

    private void Start()
    {
        PlayMusic(musicToPlay);
    }

    public void ChangeVolumeGradually(float startVolume, float targetVolume, float modifier, AudioSource audioSource)
    {
        StartCoroutine(ChangeVolumeCoroutine(startVolume, targetVolume, modifier, audioSource));
    }

    IEnumerator ChangeVolumeCoroutine(float startVolume, float targetVolume, float modifier, AudioSource audioSource)
    {
        audioSource.volume = startVolume;
        if (targetVolume > startVolume)
        {
            while (audioSource.volume < targetVolume)
            {
                audioSource.volume += modifier;
                yield return null;
            }
        }
        else
        {
            while (audioSource.volume > targetVolume)
            {
                audioSource.volume += modifier;
                yield return null;
            }
            audioSource.Stop();
        }
    }


    #region Sound
    public void PlaySound(SoundEnum soundName, int percentageOfVolume = 100)
    {
        //Essentially to be able to find the enums
        Sound soundGiven = Array.Find(sounds, sound => sound.soundName == soundName);

        soundGiven.volume = soundGiven.volume * (percentageOfVolume / 100f);
        soundGiven?.audioSource.Play();
    }

    public enum SoundEnum
    {
        UIhover,
        UIclick,
        pickUp
    }

    #endregion

    #region Music
    public void PlayMusic(MusicEnum musicName)
    {
        Music soundGiven = Array.Find(music, sound => sound.musicName == musicName);
        ChangeVolumeGradually(0.05f, soundGiven.volume, 0.001f, soundGiven.audioSource);
        soundGiven.audioSource.Play();

        soundGiven.audioSource.loop = true;
    }
    public enum MusicEnum
    {
        mainMenu,
        levelMusic,
        none
    }

    #endregion
}
