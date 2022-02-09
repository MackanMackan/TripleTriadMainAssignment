using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static IAudioService audioService;
    private static ServiceLocator instance;
    public static ServiceLocator Instance { get { return instance; } }
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        SetAudioProvider(new AudioProvider());
    }
    public static IAudioService GetAudioProvider()
    {
        return audioService;
    }
    public static void SetAudioProvider(IAudioService newService)
    {
        if (audioService != null)
        {
            audioService.Uninitialize();
        }

        audioService = newService;
        audioService.Initialize();
    }

}
