using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField]
    private float sfx_volume;
    private AudioSource[] sources = new AudioSource[10];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        for (int i = 0; i < 10; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].playOnAwake = false;
            sources[i].loop = false;
        }
    }
    /// <summary>
    /// Plays a sound effect
    /// </summary>
    /// <param name="sound"></param>
    public void playSoundEffect(SoundEffect sound)
    {
        if (sound == null)
        {
            Debug.LogWarning("Sound effect does not exist!");
            return;
        }
        for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].isPlaying || i == sources.Length - 1)
            {
                sources[i].Stop();
                sources[i].clip = sound.GetClip();
                sources[i].pitch = sound.GetPitch();
                sources[i].volume = sfx_volume * sound.GetVolumeMultiplier();
                sources[i].Play();
                break;
            }
        }
    }
    /// <summary>
    /// Plays a sound effect with a volume multiplier (maximum 1)
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="volume"></param>
    public void playSoundEffect(SoundEffect sound, float volume)
    {
        if (sound == null)
        {
            Debug.LogWarning("Sound effect does not exist!");
            return;
        }
        if (volume > 1)
            volume = 1;
        for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].isPlaying || i == sources.Length - 1)
            {
                sources[i].Stop();
                sources[i].clip = sound.GetClip();
                sources[i].pitch = sound.GetPitch();
                sources[i].volume = sfx_volume * sound.GetVolumeMultiplier() * volume;
                sources[i].Play();
                break;
            }
        }
    }

    public void setSFXVolume(float magnitude)
    {
        sfx_volume = magnitude;
    }

    public float getSFXVolume()
    {
        return sfx_volume;
    }
}
