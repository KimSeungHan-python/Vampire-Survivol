using UnityEngine;
using System.Collections.Generic;
using System.Collections;  
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f; // Ensure time scale is reset when changing scenes
    }
}
