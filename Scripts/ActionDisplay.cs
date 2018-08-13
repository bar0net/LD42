using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        GetComponentInChildren<Text>().color = Color.Lerp(Color.red, Color.cyan, magic / (damage + magic));
    }


}
