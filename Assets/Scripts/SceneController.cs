using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChange(string name)    //Responsible for changing scenes (menu -> game -> menu...)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
    }
}
