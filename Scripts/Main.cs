using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    public void LoadScene(string scene)
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(scene);
    }
}
