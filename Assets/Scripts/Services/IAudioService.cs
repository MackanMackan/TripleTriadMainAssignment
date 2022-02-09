using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioService : IService
{
    public void PlayOneShot(string clipName);
    public void PlayOneShot(AudioClip clip);

    public void PlayLoop(string clipName);
    public void PlayLoop(AudioClip clip);

    public void StopLooping(string clipName);
    public void StopLooping(AudioClip clip);
}
