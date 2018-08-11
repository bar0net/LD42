using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName = "GameAssets/Card")]
public class Card : ScriptableObject {
    public string cardName;
    public string description;

    public Sprite artwork;
    public Sprite Background;

    public int atack;
    public int defense;
    public int occult;
}
