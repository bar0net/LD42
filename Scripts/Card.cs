using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName = "GameAssets/Card")]
public class Card : ScriptableObject {

    public string cardName;
    public string description;

    public Sprite artwork;
    public Sprite Background;

    [SerializeField]
    public Collection.TokenValue[] tokenValues;
}
