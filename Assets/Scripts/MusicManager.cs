using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<Song> songs;
    public AudioSource loop;
    public float volMultiplier;
    Coroutine fading;
    public SoundEffect musicVolume;
    public Song currentSong = new Song();
    public static MusicManager instance;
    float dipMultiplier = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public IEnumerator _dipVolume(SoundEffect s)
    {
        dipMultiplier = 0.333f;
        yield return new WaitForSecondsRealtime(s.GetClip().length - 0.25f);
        dipMultiplier = 1;
    }

    public void dipVolume(SoundEffect s)
    {
        StartCoroutine(_dipVolume(s));
    }
    Song getSong(Environment env)
    {
        foreach (Song s in songs)
        {
            if (s.environment == env)
                return s;
        }
        return new Song();
    }

    Song getSong(string name)
    {
        foreach (Song s in songs)
        {
            if (s.name == name)
                return s;
        }
        return new Song();
    }
    private void Update()
    {
        loop.volume = GameOptions._MUSVOL / 10f * musicVolume.GetVolumeMultiplier() * (PauseScreenDisplay.isPaused ? 0.75f : 1) * (InventoryDisplay.isInventoryOpen ? 0.5f : 1) * dipMultiplier * volMultiplier;
    }

    public void switchEnvironment(int outL, Environment environment, int inL)
    {
        fadeToNewSong(outL, getSong(environment), inL);
    }

    public void switchSong(int outL, string songName, int inL)
    {
        fadeToNewSong(outL, getSong(songName), inL);
    }

    public void fadeToNewSong(int lengthOut, Song song, int lengthIn)
    {
        if (!song.Equals(currentSong))
        {
            if (fading != null)
                StopCoroutine(fading);
            fading = StartCoroutine(_fadeToNewSong(lengthOut, song, lengthIn));
        }
    }

    IEnumerator _fadeToNewSong(int lengthOut, Song song, int lengthIn)
    {
        currentSong = new Song(song);
        yield return null;
        volMultiplier = 1;
        for (int i = lengthOut; i > 0; --i)
        {
            volMultiplier = (float)i / lengthOut;
            yield return null;
        }
        volMultiplier = 0;
        yield return null;
        loop.Stop();
        loop.clip = song.loop;
        loop.Play();
        yield return null;
        for (int i = 0; i < lengthIn; ++i)
        {
            volMultiplier = (float)i / lengthIn;
            yield return null;
        }
        volMultiplier = 1;
    }
}

[System.Serializable]
public class Song
{
    public Environment environment;
    public string name;
    public AudioClip loop;

    public Song()
    {
        environment = Environment.NULL;
        name = "";
        loop = null;
    }

    public Song(Song s)
    {
        environment = s.environment;
        name = s.name;
        loop = s.loop;
    }
}
