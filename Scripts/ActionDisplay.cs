using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Displays the text of the action a Character is going to take
// aka the text over the enemies that indicate the next attack
public class ActionDisplay : MonoBehaviour {

    Ability h;

    Image _img;
    Text _txt;

    private void Start()
    {
        _img = GetComponentInChildren<Image>();
        _txt = GetComponentInChildren<Text>();
    }

    public void Display(Ability h, int damage, int magic)
    {
        GetComponentInChildren<Image>().sprite = h.icon;

        GetComponentInChildren<Text>().text = (damage + magic).ToString();
        if (damage + magic > 0) GetComponentInChildren<Text>().color = Color.Lerp(Color.red, Color.magenta, magic / (damage + magic));
        else GetComponentInChildren<Text>().color = Color.red;
    }


}
