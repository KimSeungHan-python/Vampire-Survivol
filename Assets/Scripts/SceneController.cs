using UnityEngine;
using System.Collections.Generic;
using System.Collections;  
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
