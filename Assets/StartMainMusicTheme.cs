using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMainMusicTheme : MonoBehaviour
{
    void Start()
    {
        ServiceLocator.GetAudioProvider().PlayLoop("royalentrance");
    }
}
