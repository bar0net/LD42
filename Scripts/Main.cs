using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    private void Start()
    {
        if (Overlord._instance != null) DestroyImmediate(Overlord._instance);
    }

    public void LoadScene(string scene)
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(scene);
    }
}
