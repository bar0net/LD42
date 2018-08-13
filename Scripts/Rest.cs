using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Rest : MonoBehaviour {
    const string _TEXT_ = "You will gain {0} HP but you will gain {1} insanity";
    const float _HP_BASE_ = 50;
    const float _HP_RATIO_ = 0.5f;
    const float _INSANITY_BASE_ = 5;
    const float _INSTANITY_RATIO_ = 0.5f;

    public Text description;

    int hpGain;
    int insanityGain;

	// Use this for initialization
	void Start () {
        int health = PlayerPrefs.GetInt("player_health",100);

        hpGain = (int)(_HP_BASE_ + _HP_RATIO_ * Overlord._instance.enemyLevel);
        insanityGain = (int)(_INSANITY_BASE_ + _INSTANITY_RATIO_ * (health + hpGain));

        description.text = string.Format(_TEXT_ ,hpGain, insanityGain);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Confirm()
    {
        PlayerPrefs.SetInt("player_health", PlayerPrefs.GetInt("player_health", 100) + hpGain);
        Overlord._instance.enemyLevel += insanityGain;
        LoadScene();
    }

    public void LoadScene()
    {
        Overlord._instance.eventsCompleted += 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map");
    }
}
