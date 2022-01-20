using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private static MySceneManager instance;
    public static MySceneManager Instance { get { return instance; } }
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
    }
    public void LoadScene(string sceneName)
    {
        if (sceneName.Equals("Menu"))
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = 60;
        }
        SceneManager.LoadScene(sceneName);
    }
}
