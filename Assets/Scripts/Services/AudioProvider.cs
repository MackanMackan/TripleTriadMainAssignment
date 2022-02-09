using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioProvider : IAudioService
{
    private string path;
    private int amountOfSources = 20;
    List<AudioSource> audioSources;
    List<AudioClip> audioClips;
    Dictionary<string, AudioClip> audioLibrary;
    GameObject parent;
    public void Initialize()
    {
        path = "SFX/";
        parent = new GameObject("AudioSources");
        CreateAudioSources();
        LoadAudioClipsToList();
    }
    private void CreateAudioSources()
    {
        
        audioSources = new List<AudioSource>();
        for (int i = 0; i < amountOfSources; i++)
        {
            AudioSource source = new GameObject("AudioSource (created at runtime)").AddComponent<AudioSource>();
            source.transform.SetParent(parent.transform);
            AddAudioSourcesToList(source);
        }
    }

    private void LoadAudioClipsToList()
    {
        audioLibrary = new Dictionary<string, AudioClip>();
        audioClips = new List<AudioClip>();
        audioClips.AddRange(Resources.LoadAll<AudioClip>(path));
        foreach (var clip in audioClips)
        {
            audioLibrary.Add(clip.name.ToLower(),clip);
        }
    }

    private void AddAudioSourcesToList(AudioSource source)
    {
        audioSources.Add(source);
    }

    public void PlayLoop(string clipName)
    {
        PlayLoop(audioLibrary[clipName.ToLower()]);
    }

    public void PlayLoop(AudioClip clip)
    {
        AudioSource source = GetAvailableAudioSource();
        source.PlayOneShot(clip);
        source.loop = true;
    }

    public void PlayOneShot(string clipName)
    {
        PlayOneShot(audioLibrary[clipName.ToLower()]);
    }
    public void PlayOneShot(AudioClip clip)
    {
        AudioSource source = GetAvailableAudioSource();
        source.PlayOneShot(clip);
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return CreateNewAudioSource();
    }

    private AudioSource CreateNewAudioSource()
    {
        AudioSource source = new GameObject("AudioSource (created at runtime)").AddComponent<AudioSource>();
        source.transform.SetParent(parent.transform);
        AddAudioSourcesToList(source);
        return source;
    }


    public void StopLooping(string clipName)
    {
        throw new System.NotImplementedException();
    }

    public void StopLooping(AudioClip clip)
    {
        throw new System.NotImplementedException();
    }

    public void Uninitialize()
    {
        foreach (var source in audioSources)
        {
            MonoBehaviour.Destroy(source);
        }
        audioSources.Clear();
        audioClips.Clear();
        audioLibrary.Clear();
    }
}
